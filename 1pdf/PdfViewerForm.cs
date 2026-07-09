using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1pdf.Services;
using PdfiumPdfDocument = PdfiumViewer.PdfDocument;

namespace _1pdf
{
    /// <summary>
    /// Non-modal window that displays the full content of a PDF file,
    /// one page at a time with navigation and zoom controls.
    /// The document is loaded and each page is rendered on a background
    /// thread so a large or damaged file never blocks the main window.
    /// Renders pages directly via Pdfium instead of using the
    /// PdfiumViewer.PdfViewer control, whose embedded resources rely on
    /// BinaryFormatter and crash on modern .NET.
    /// </summary>
    public class PdfViewerForm : Form
    {
        private const int MaxRenderDimension = 4000;
        private const double MinZoom = 1.0;
        private const double MaxZoom = 5.0;
        private const double ZoomStep = 1.25;

        private readonly string _filePath;
        private readonly LocalizationService _localization;

        private readonly Panel _scrollPanel;
        private readonly PictureBox _pictureBox;
        private readonly Label _loadingLabel;
        private readonly Button _btnPrev;
        private readonly Button _btnNext;
        private readonly NumericUpDown _pageSelector;
        private readonly Label _pageCountLabel;
        private readonly Button _btnZoomOut;
        private readonly Button _btnZoomIn;
        private readonly Label _zoomLabel;

        private PdfiumPdfDocument _document;
        private bool _rendering;
        private bool _renderQueued;
        private int _currentPage = 1;
        private double _zoom = 1.0;

        public PdfViewerForm(string filePath, LocalizationService localization)
        {
            _filePath = filePath;
            _localization = localization;

            Text = Path.GetFileName(filePath);
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(900, 700);
            MinimumSize = new Size(400, 300);
            KeyPreview = true;

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 36 };

            _btnPrev = new Button { Text = "<", Width = 32, Height = 26, Left = 8, Top = 5, Enabled = false };
            _btnNext = new Button { Text = ">", Width = 32, Height = 26, Left = 44, Top = 5, Enabled = false };
            _pageSelector = new NumericUpDown { Minimum = 1, Maximum = 1, Width = 70, Left = 84, Top = 7, Enabled = false };
            _pageCountLabel = new Label { AutoSize = true, Left = 160, Top = 10 };
            _btnZoomOut = new Button { Text = "−", Width = 32, Height = 26, Left = 250, Top = 5, Enabled = false };
            _btnZoomIn = new Button { Text = "+", Width = 32, Height = 26, Left = 286, Top = 5, Enabled = false };
            _zoomLabel = new Label { AutoSize = true, Left = 324, Top = 10, Text = "100%" };

            toolbar.Controls.AddRange(new Control[]
            {
                _btnPrev, _btnNext, _pageSelector, _pageCountLabel,
                _btnZoomOut, _btnZoomIn, _zoomLabel
            });

            _pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            _scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.DarkGray
            };
            _scrollPanel.Controls.Add(_pictureBox);

            _loadingLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14),
                Text = _localization.GetString("ViewerLoading")
            };

            Controls.Add(_scrollPanel);
            Controls.Add(_loadingLabel);
            Controls.Add(toolbar);
            _loadingLabel.BringToFront();

            _btnPrev.Click += (s, e) => _pageSelector.Value = Math.Max(1, _pageSelector.Value - 1);
            _btnNext.Click += (s, e) => _pageSelector.Value = Math.Min(_pageSelector.Maximum, _pageSelector.Value + 1);
            _pageSelector.ValueChanged += async (s, e) => await RenderPageAsync((int)_pageSelector.Value);

            _btnZoomIn.Click += (s, e) => SetZoom(_zoom * ZoomStep);
            _btnZoomOut.Click += (s, e) => SetZoom(_zoom / ZoomStep);
            _scrollPanel.Resize += (s, e) => UpdateLayout();
            ResizeEnd += (s, e) => _ = RenderPageAsync(_currentPage);

            MouseWheel += Form_MouseWheel;
            _scrollPanel.MouseWheel += Form_MouseWheel;
            _pictureBox.MouseWheel += Form_MouseWheel;

            KeyDown += (s, e) =>
            {
                if (!e.Control) return;
                if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) SetZoom(_zoom * ZoomStep);
                else if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract) SetZoom(_zoom / ZoomStep);
            };

            Shown += async (s, e) => await LoadDocumentAsync();
            FormClosed += (s, e) =>
            {
                _pictureBox.Image?.Dispose();
                _document?.Dispose();
            };
        }

        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys != Keys.Control) return;
            SetZoom(e.Delta > 0 ? _zoom * ZoomStep : _zoom / ZoomStep);
        }

        private void SetZoom(double zoom)
        {
            double newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, zoom));
            if (newZoom == _zoom) return;

            _zoom = newZoom;
            _zoomLabel.Text = $"{_zoom * 100:0}%";
            // Rescale the current bitmap immediately for responsiveness,
            // then re-render at the new resolution for sharpness.
            UpdateLayout();
            _ = RenderPageAsync(_currentPage);
        }

        /// <summary>
        /// Sizes the page image to the fit-to-window size multiplied by the
        /// zoom factor, centered when smaller than the viewport.
        /// </summary>
        private void UpdateLayout()
        {
            var image = _pictureBox.Image;
            if (image == null) return;

            var client = _scrollPanel.ClientSize;
            if (client.Width <= 0 || client.Height <= 0) return;

            double imageRatio = (double)image.Width / image.Height;
            double clientRatio = (double)client.Width / client.Height;

            int fitWidth, fitHeight;
            if (clientRatio > imageRatio)
            {
                fitHeight = client.Height;
                fitWidth = (int)(fitHeight * imageRatio);
            }
            else
            {
                fitWidth = client.Width;
                fitHeight = (int)(fitWidth / imageRatio);
            }

            int width = (int)(fitWidth * _zoom);
            int height = (int)(fitHeight * _zoom);

            _pictureBox.SuspendLayout();
            _pictureBox.Size = new Size(width, height);
            _pictureBox.Location = new Point(
                Math.Max(0, (client.Width - width) / 2),
                Math.Max(0, (client.Height - height) / 2));
            _pictureBox.ResumeLayout();
        }

        private async Task LoadDocumentAsync()
        {
            try
            {
                _document = await Task.Run(() => PdfiumPdfDocument.Load(_filePath));

                if (IsDisposed)
                {
                    _document.Dispose();
                    return;
                }

                _pageSelector.Maximum = _document.PageCount;
                _pageSelector.Enabled = true;
                _btnPrev.Enabled = true;
                _btnNext.Enabled = true;
                _btnZoomIn.Enabled = true;
                _btnZoomOut.Enabled = true;

                await RenderPageAsync(1);
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;

                MessageBox.Show(
                    _localization.GetString("ViewerLoadError", _filePath) +
                    Environment.NewLine + ex.Message,
                    _localization.GetString("ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }

        private async Task RenderPageAsync(int pageNumber)
        {
            if (_document == null) return;

            bool pageChanged = pageNumber != _currentPage;
            _currentPage = pageNumber;

            // Coalesce: if a render is in flight, remember that a newer
            // request exists and let the running loop pick it up.
            if (_rendering)
            {
                _renderQueued = true;
                return;
            }

            _rendering = true;
            try
            {
                do
                {
                    _renderQueued = false;
                    int page = _currentPage;
                    var renderSize = GetRenderSize(page);

                    var image = await Task.Run(() =>
                        _document.Render(page - 1, renderSize.Width, renderSize.Height, forPrinting: false));

                    if (IsDisposed)
                    {
                        image.Dispose();
                        return;
                    }

                    _pictureBox.Image?.Dispose();
                    _pictureBox.Image = image;
                    _loadingLabel.Visible = false;
                    if (pageChanged)
                    {
                        _scrollPanel.AutoScrollPosition = Point.Empty;
                        pageChanged = false;
                    }
                    UpdateLayout();

                    _pageCountLabel.Text = _localization.GetString(
                        "LabelPages", page, _document.PageCount);
                }
                while (_renderQueued);
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;

                MessageBox.Show(
                    _localization.GetString("ViewerLoadError", _filePath) +
                    Environment.NewLine + ex.Message,
                    _localization.GetString("ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _rendering = false;
            }
        }

        /// <summary>
        /// Pixel size to render the page at: the on-screen size at the current
        /// zoom, multiplied by the display DPI scale so text stays sharp,
        /// capped to keep memory usage bounded.
        /// </summary>
        private Size GetRenderSize(int pageNumber)
        {
            var pageSize = _document.PageSizes[pageNumber - 1];
            double pageRatio = pageSize.Width / pageSize.Height;

            var client = _scrollPanel.ClientSize;
            int fitWidth = Math.Max(100, client.Width);
            int fitHeight = Math.Max(100, client.Height);
            if ((double)fitWidth / fitHeight > pageRatio)
            {
                fitWidth = (int)(fitHeight * pageRatio);
            }
            else
            {
                fitHeight = (int)(fitWidth / pageRatio);
            }

            double dpiScale = DeviceDpi / 96.0;
            double scale = _zoom * dpiScale;
            int width = (int)(fitWidth * scale);
            int height = (int)(fitHeight * scale);

            if (width > MaxRenderDimension || height > MaxRenderDimension)
            {
                double cap = Math.Min(
                    (double)MaxRenderDimension / width,
                    (double)MaxRenderDimension / height);
                width = (int)(width * cap);
                height = (int)(height * cap);
            }

            return new Size(width, height);
        }
    }
}
