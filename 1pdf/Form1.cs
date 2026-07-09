using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using _1pdf.Models;
using _1pdf.Services;

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

        private int _totalPages;

        public Form1()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeLanguageSelector();
            ApplyLocalization();

            listBox1.AllowDrop = true;
            listBox1.DragEnter += ListBox1_DragEnter;
            listBox1.DragDrop += ListBox1_DragDrop;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.MouseDoubleClick += OpenViewer_DoubleClick;
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

        private void ListBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void ListBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (PdfFileList.AreAllPdf(files))
                {
                    _fileList.AddRange(files);
                    RefreshListBox();
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
                    _fileList.AddRange(openFileDialog.FileNames);
                    RefreshListBox();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index != -1)
            {
                _fileList.RemoveAt(index);
                RefreshListBox();
            }
        }

        private void btnArriba_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1) return;

            int newIndex = _fileList.MoveUp(index);
            RefreshListBox(newIndex);
        }

        private void btnAbajo_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1) return;

            int newIndex = _fileList.MoveDown(index);
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
                _mergeService.Merge(_fileList.Files, outputFile);

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
            if (listBox1.SelectedItem == null) return;

            var viewer = new PdfViewerForm(listBox1.SelectedItem.ToString(), _localization);
            viewer.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
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
        }

        private void numericUpDownPage_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ShowPreview(listBox1.SelectedItem.ToString(), (int)numericUpDownPage.Value);
            }
        }

        private void ShowPreview(string filePath, int pageNumber)
        {
            try
            {
                var result = _previewService.RenderPage(
                    filePath, pageNumber, pictureBox1.Width, pictureBox1.Height);

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
