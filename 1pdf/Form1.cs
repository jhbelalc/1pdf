using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _1pdf.Models;
using _1pdf.Services;
using PdfiumPdfDocument = PdfiumViewer.PdfDocument;

namespace _1pdf
{
    /// <summary>
    /// Main window. Responsible only for UI wiring and state reflection.
    /// Business logic lives in Services/ and Models/.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly PdfFileList _fileList = new PdfFileList();
        private readonly PdfMergeService _mergeService = new PdfMergeService();
        private readonly PdfPreviewService _previewService = new PdfPreviewService();
        private readonly LocalizationService _localization = new LocalizationService();
        private readonly PdfPasswordStore _passwords = new PdfPasswordStore();

        private int _totalPages;

        // Drag-to-reorder state: index being dragged and where the
        // mouse went down, so a drag only starts past the threshold.
        private int _dragIndex = -1;
        private Point _dragStart;

        public Form1()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Use the app icon (already sized for small rendering)
            // on the About button, like the other buttons' icons.
            using (var appIcon = new Icon(Icon, new Size(24, 24)))
            {
                btnAbout.Image = appIcon.ToBitmap();
            }
            InitializeLanguageSelector();
            ApplyLocalization();

            listBox1.AllowDrop = true;
            listBox1.DragEnter += ListBox1_DragEnter;
            listBox1.DragDrop += ListBox1_DragDrop;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.MouseDoubleClick += OpenViewer_DoubleClick;
            listBox1.MouseDown += ListBox1_MouseDown;
            listBox1.MouseMove += ListBox1_MouseMove;
            listBox1.MouseUp += (s, e) => _dragIndex = -1;
            listBox1.DragOver += ListBox1_DragOver;
            pictureBox1.DoubleClick += OpenViewer_DoubleClick;
        }

        #region Localization

        private void InitializeLanguageSelector()
        {
            comboBoxLanguage.Items.AddRange(new string[] { "English", "Español" });
            comboBoxLanguage.SelectedIndex = _localization.IsSpanish ? 1 : 0;

            comboBoxLanguage.SelectedIndexChanged += (s, e) =>
            {
                string culture = comboBoxLanguage.SelectedIndex == 0 ? "en-US" : "es-ES";
                _localization.SetCulture(culture);
                ApplyLocalization();
            };
        }

        private void ApplyLocalization()
        {
            try
            {
                Text = _localization.GetString("FormTitle");
                btnRemove.Text = _localization.GetString("ButtonRemove");
                btnArriba.Text = _localization.GetString("ButtonUp");
                btnAbajo.Text = _localization.GetString("ButtonDown");
                lblNombre.Text = _localization.GetString("LabelName");
                btnSelectFiles.Text = _localization.GetString("ButtonSelectFiles");
                btnGenerar.Text = _localization.GetString("ButtonGenerate");
                btnAbout.Text = _localization.GetString("AboutTitle");
                UpdatePageLabel();
                // Repaint so the "select a PDF" prompt drawn in
                // pictureBox1_Paint picks up the new language.
                pictureBox1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying localization: " + ex.Message);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (var about = new AboutForm(_localization))
            {
                about.ShowDialog(this);
            }
        }

        #endregion

        #region File list management

        private void RefreshListBox(int selectedIndex = -1)
        {
            listBox1.Items.Clear();
            foreach (string file in _fileList.Files)
            {
                listBox1.Items.Add(file);
            }

            if (selectedIndex >= 0 && selectedIndex < listBox1.Items.Count)
            {
                listBox1.SelectedIndex = selectedIndex;
            }
        }

        private void ListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _dragIndex = listBox1.IndexFromPoint(e.Location);
            _dragStart = e.Location;
        }

        private void ListBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _dragIndex < 0) return;

            // Only start dragging once the mouse leaves the system drag
            // rectangle, so plain clicks and double-clicks keep working.
            var dragSize = SystemInformation.DragSize;
            if (Math.Abs(e.X - _dragStart.X) < dragSize.Width &&
                Math.Abs(e.Y - _dragStart.Y) < dragSize.Height)
            {
                return;
            }

            int index = _dragIndex;
            _dragIndex = -1;
            listBox1.DoDragDrop(index, DragDropEffects.Move);
        }

        private void ListBox1_DragEnter(object sender, DragEventArgs e)
        {
            SetDragEffect(e);
        }

        private void ListBox1_DragOver(object sender, DragEventArgs e)
        {
            SetDragEffect(e);
        }

        private static void SetDragEffect(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ListBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)))
            {
                int fromIndex = (int)e.Data.GetData(typeof(int));
                int toIndex = listBox1.IndexFromPoint(
                    listBox1.PointToClient(new Point(e.X, e.Y)));

                // Dropping below the last item moves it to the end.
                if (toIndex < 0) toIndex = _fileList.Count - 1;

                int newIndex = _fileList.Move(fromIndex, toIndex);
                RefreshListBox(newIndex);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (PdfFileList.AreAllPdf(files))
                {
                    AddFiles(files);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    AddFiles(openFileDialog.FileNames);
                }
            }
        }

        /// <summary>
        /// Adds files to the list, prompting for the password of any
        /// protected PDF. Files whose password prompt is cancelled are
        /// not added, so preview and merge never hit password errors.
        /// </summary>
        private void AddFiles(IEnumerable<string> paths)
        {
            var added = _fileList.AddRange(paths);

            foreach (string file in added)
            {
                if (!TryEnsurePassword(file))
                {
                    _fileList.Remove(file);
                    MessageBox.Show(
                        _localization.GetString("FileNotAdded", Path.GetFileName(file)),
                        _localization.GetString("PasswordTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            RefreshListBox();
        }

        /// <summary>
        /// Verifies the file can be opened, asking the user for a password
        /// when it is protected. Passwords live only in the in-memory store
        /// for this session, so a file already unlocked (even after being
        /// removed and re-added) is not asked again. Returns false when the
        /// user cancels the prompt.
        /// </summary>
        private bool TryEnsurePassword(string filePath)
        {
            bool previousAttemptFailed = false;

            while (true)
            {
                try
                {
                    using (PdfiumPdfDocument.Load(filePath, _passwords.Get(filePath))) { }
                    return true;
                }
                catch (PdfiumViewer.PdfException ex)
                    when (ex.Error == PdfiumViewer.PdfError.PasswordProtected)
                {
                    using (var prompt = new PasswordPromptForm(
                        Path.GetFileName(filePath), previousAttemptFailed, _localization))
                    {
                        if (prompt.ShowDialog(this) != DialogResult.OK)
                        {
                            return false;
                        }

                        _passwords.Set(filePath, prompt.Password);
                        previousAttemptFailed = true;
                    }
                }
                catch
                {
                    // Not a password problem (corrupt file, I/O error, ...):
                    // let it surface through the preview/merge error handling.
                    return true;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count == 0) return;

            // Remove from the highest index down so the remaining
            // indices stay valid while deleting.
            var indices = listBox1.SelectedIndices.Cast<int>()
                .OrderByDescending(i => i);
            foreach (int index in indices)
            {
                _fileList.RemoveAt(index);
            }

            RefreshListBox();
            ClearPreview();
        }

        private void btnArriba_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count != 1) return;

            int newIndex = _fileList.MoveUp(listBox1.SelectedIndex);
            RefreshListBox(newIndex);
        }

        private void btnAbajo_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count != 1) return;

            int newIndex = _fileList.MoveDown(listBox1.SelectedIndex);
            RefreshListBox(newIndex);
        }

        #endregion

        #region Output name

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                saveFileDialog.Title = "Guardar archivo PDF";
                saveFileDialog.FileName = "DocumentoCompleto.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtnombre.Text = saveFileDialog.FileName;
                }
            }
        }

        private void lblNombre_Click(object sender, EventArgs e)
        {
            if (_fileList.Count <= 1)
            {
                MessageBox.Show(
                    _localization.GetString("NotSelected"),
                    _localization.GetString("ErrorNotSelected"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            txtnombre.Text = _fileList.BuildDefaultOutputName();
        }

        #endregion

        #region Merge

        private void button1_Click(object sender, EventArgs e)
        {
            if (_fileList.Count <= 1) return;

            if (string.IsNullOrWhiteSpace(txtnombre.Text))
            {
                MessageBox.Show(
                    _localization.GetString("MissingParameterMessage"),
                    _localization.GetString("MissingParameterTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            var outputFile = txtnombre.Text;

            try
            {
                _mergeService.Merge(_fileList.Files, outputFile, _passwords.Get);

                MessageBox.Show(
                    _localization.GetString("FileCreatedSuccess", outputFile),
                    _localization.GetString("SuccessTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _localization.GetString("FileCreationError", outputFile) +
                    Environment.NewLine + ex.Message,
                    _localization.GetString("ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Preview

        private void OpenViewer_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 1) return;

            string file = listBox1.SelectedItem.ToString();
            var viewer = new PdfViewerForm(file, _localization, _passwords.Get(file));
            viewer.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Preview only applies to a single selected file; with a
            // multi-selection (or none) the preview area shows the prompt.
            if (listBox1.SelectedItems.Count != 1)
            {
                ClearPreview();
                return;
            }

            if (numericUpDownPage.Value != 1)
            {
                // Triggers numericUpDownPage_ValueChanged, which renders page 1.
                numericUpDownPage.Value = 1;
            }
            else
            {
                ShowPreview(listBox1.SelectedItem.ToString(), 1);
            }
        }

        private void numericUpDownPage_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                ShowPreview(listBox1.SelectedItem.ToString(), (int)numericUpDownPage.Value);
            }
        }

        private void ClearPreview()
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
            _totalPages = 0;
            UpdatePageLabel();
            pictureBox1.Invalidate();
        }

        private void ShowPreview(string filePath, int pageNumber)
        {
            try
            {
                var result = _previewService.RenderPage(
                    filePath, pageNumber, pictureBox1.Width, pictureBox1.Height,
                    _passwords.Get(filePath));

                _totalPages = result.TotalPages;
                numericUpDownPage.Maximum = result.TotalPages;
                UpdatePageLabel();

                if (!result.IsValidPage)
                {
                    MessageBox.Show(
                        _localization.GetString("TextInvalidPage"),
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                pictureBox1.Image = result.Image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Preview error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdatePageLabel()
        {
            label1.Text = _totalPages > 0
                ? _localization.GetString("LabelPages", (int)numericUpDownPage.Value, _totalPages)
                : string.Empty;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                string prompt = _localization.GetString("TextSelectPreview");
                using (Font myFont = new Font("Arial", 14))
                {
                    e.Graphics.DrawString(prompt, myFont, Brushes.Black, new Point(2, 2));
                }
            }
        }

        #endregion
    }
}
