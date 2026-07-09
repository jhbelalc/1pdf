
using System.Windows.Forms;

namespace _1pdf
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Add this to the designer file
        private System.Windows.Forms.PictureBox pictureBox1;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnGenerar = new Button();
            openFileDialog1 = new OpenFileDialog();
            btnSelectFiles = new Button();
            txtnombre = new TextBox();
            lblNombre = new Label();
            btnArchivo = new Button();
            listBox1 = new ListBox();
            btnRemove = new Button();
            btnAbajo = new Button();
            btnArriba = new Button();
            pictureBox1 = new PictureBox();
            numericUpDownPage = new NumericUpDown();
            label1 = new Label();
            comboBoxLanguage = new ComboBox();
            btnAbout = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPage).BeginInit();
            SuspendLayout();
            // 
            // btnGenerar
            // 
            btnGenerar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGenerar.Image = Properties.Resources.Document;
            btnGenerar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnGenerar.Location = new System.Drawing.Point(779, 413);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new System.Drawing.Size(150, 50);
            btnGenerar.TabIndex = 0;
            btnGenerar.Text = "Convertir 1 PDF";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += button1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSelectFiles
            // 
            btnSelectFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectFiles.Image = Properties.Resources.add;
            btnSelectFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnSelectFiles.Location = new System.Drawing.Point(615, 12);
            btnSelectFiles.Name = "btnSelectFiles";
            btnSelectFiles.Size = new System.Drawing.Size(150, 50);
            btnSelectFiles.TabIndex = 2;
            btnSelectFiles.Text = "Seleccionar o arrastre y suelte";
            btnSelectFiles.UseVisualStyleBackColor = true;
            btnSelectFiles.Click += button2_Click;
            // 
            // txtnombre
            // 
            txtnombre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtnombre.Location = new System.Drawing.Point(615, 94);
            txtnombre.Multiline = true;
            txtnombre.Name = "txtnombre";
            txtnombre.Size = new System.Drawing.Size(314, 81);
            txtnombre.TabIndex = 3;
            // 
            // lblNombre
            // 
            lblNombre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblNombre.AutoSize = true;
            lblNombre.Cursor = Cursors.Hand;
            lblNombre.Location = new System.Drawing.Point(615, 76);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new System.Drawing.Size(234, 15);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre nuevo PDF, clic AQUÍ para sugerir.";
            lblNombre.Click += lblNombre_Click;
            // 
            // btnArchivo
            // 
            btnArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArchivo.Location = new System.Drawing.Point(905, 181);
            btnArchivo.Name = "btnArchivo";
            btnArchivo.Size = new System.Drawing.Size(24, 23);
            btnArchivo.TabIndex = 5;
            btnArchivo.Text = "...";
            btnArchivo.UseVisualStyleBackColor = true;
            btnArchivo.Click += btnArchivo_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new System.Drawing.Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(597, 169);
            listBox1.TabIndex = 6;
            // 
            // btnRemove
            // 
            btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemove.Image = Properties.Resources.bad;
            btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnRemove.Location = new System.Drawing.Point(615, 293);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(150, 50);
            btnRemove.TabIndex = 7;
            btnRemove.Text = "Borrar";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAbajo
            // 
            btnAbajo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAbajo.Image = Properties.Resources.Down;
            btnAbajo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnAbajo.Location = new System.Drawing.Point(615, 241);
            btnAbajo.Name = "btnAbajo";
            btnAbajo.Size = new System.Drawing.Size(150, 50);
            btnAbajo.TabIndex = 7;
            btnAbajo.Text = "Abajo";
            btnAbajo.UseVisualStyleBackColor = true;
            btnAbajo.Click += btnAbajo_Click;
            // 
            // btnArriba
            // 
            btnArriba.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArriba.Image = Properties.Resources.Up;
            btnArriba.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnArriba.Location = new System.Drawing.Point(615, 188);
            btnArriba.Name = "btnArriba";
            btnArriba.Size = new System.Drawing.Size(150, 50);
            btnArriba.TabIndex = 7;
            btnArriba.Text = "Arriba";
            btnArriba.UseVisualStyleBackColor = true;
            btnArriba.Click += btnArriba_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new System.Drawing.Point(12, 188);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(597, 281);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // numericUpDownPage
            // 
            numericUpDownPage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            numericUpDownPage.Location = new System.Drawing.Point(619, 376);
            numericUpDownPage.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownPage.Name = "numericUpDownPage";
            numericUpDownPage.Size = new System.Drawing.Size(73, 23);
            numericUpDownPage.TabIndex = 8;
            numericUpDownPage.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownPage.ValueChanged += numericUpDownPage_ValueChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Cursor = Cursors.Hand;
            label1.Location = new System.Drawing.Point(619, 358);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(62, 15);
            label1.TabIndex = 9;
            label1.Text = "Ir a página";
            // 
            // comboBoxLanguage
            // 
            comboBoxLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBoxLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLanguage.FormattingEnabled = true;
            comboBoxLanguage.Location = new System.Drawing.Point(799, 19);
            comboBoxLanguage.Name = "comboBoxLanguage";
            comboBoxLanguage.Size = new System.Drawing.Size(121, 23);
            comboBoxLanguage.TabIndex = 10;
            //
            // btnAbout
            //
            btnAbout.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAbout.Location = new System.Drawing.Point(615, 413);
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new System.Drawing.Size(150, 50);
            btnAbout.TabIndex = 11;
            btnAbout.Text = "About 1PDF";
            btnAbout.UseVisualStyleBackColor = true;
            btnAbout.Click += btnAbout_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(941, 471);
            Controls.Add(btnAbout);
            Controls.Add(comboBoxLanguage);
            Controls.Add(label1);
            Controls.Add(numericUpDownPage);
            Controls.Add(pictureBox1);
            Controls.Add(btnArriba);
            Controls.Add(btnAbajo);
            Controls.Add(btnRemove);
            Controls.Add(listBox1);
            Controls.Add(btnArchivo);
            Controls.Add(lblNombre);
            Controls.Add(txtnombre);
            Controls.Add(btnSelectFiles);
            Controls.Add(btnGenerar);
            Name = "Form1";
            Text = "Unir PDFs en uno solo";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnSelectFiles;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Button btnArchivo;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAbajo;
        private System.Windows.Forms.Button btnArriba;
        private NumericUpDown numericUpDownPage;
        private Label label1;
        private ComboBox comboBoxLanguage;
        private Button btnAbout;
    }
}

