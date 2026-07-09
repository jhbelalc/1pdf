using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using _1pdf.Services;

namespace _1pdf
{
    /// <summary>
    /// About window styled to match the neon 1PDF logo:
    /// dark navy background with neon red and neon blue accents.
    /// </summary>
    public class AboutForm : Form
    {
        private static readonly Color BackgroundColor = Color.FromArgb(18, 14, 30);
        private static readonly Color NeonRed = Color.FromArgb(255, 45, 60);
        private static readonly Color NeonBlue = Color.FromArgb(90, 216, 255);
        private static readonly Color SoftWhite = Color.FromArgb(235, 235, 245);

        public AboutForm(LocalizationService localization)
        {
            Text = localization.GetString("AboutTitle");
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(460, 620);
            BackColor = BackgroundColor;

            var logo = new PictureBox
            {
                Image = localization.GetImage("Logo"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(320, 320),
                Location = new Point((ClientSize.Width - 320) / 2, 24)
            };

            var titleLabel = new Label
            {
                Text = "1PDF",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = NeonRed,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 50),
                Location = new Point(0, 356)
            };

            string version = Application.ProductVersion.Split('+')[0];
            var versionLabel = new Label
            {
                Text = localization.GetString("AboutVersion", version),
                Font = new Font("Segoe UI", 11),
                ForeColor = NeonBlue,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 24),
                Location = new Point(0, 406)
            };

            var taglineLabel = new Label
            {
                Text = localization.GetString("AboutTagline"),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Italic),
                ForeColor = SoftWhite,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width - 40, 40),
                Location = new Point(20, 434)
            };

            var developedByLabel = new Label
            {
                Text = localization.GetString("AboutDevelopedBy"),
                Font = new Font("Segoe UI", 9),
                ForeColor = NeonBlue,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 20),
                Location = new Point(0, 486)
            };

            var authorLabel = new Label
            {
                Text = "John Harold Belalcazar Lozano",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = SoftWhite,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 26),
                Location = new Point(0, 506)
            };

            var emailLink = new LinkLabel
            {
                Text = "jhbelalc@gmail.com",
                Font = new Font("Segoe UI", 10),
                LinkColor = NeonBlue,
                ActiveLinkColor = NeonRed,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 22),
                Location = new Point(0, 534)
            };
            emailLink.LinkClicked += (s, e) => OpenExternal("mailto:jhbelalc@gmail.com");

            var githubLink = new LinkLabel
            {
                Text = "github.com/jhbelalc/1pdf",
                Font = new Font("Segoe UI", 10),
                LinkColor = NeonBlue,
                ActiveLinkColor = NeonRed,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 22),
                Location = new Point(0, 558)
            };
            githubLink.LinkClicked += (s, e) => OpenExternal("https://github.com/jhbelalc/1pdf");

            var licenseLabel = new Label
            {
                Text = "MIT License",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(140, 140, 160),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ClientSize.Width, 20),
                Location = new Point(0, 588)
            };

            Controls.AddRange(new Control[]
            {
                logo, titleLabel, versionLabel, taglineLabel,
                developedByLabel, authorLabel, emailLink, githubLink, licenseLabel
            });
        }

        private static void OpenExternal(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
                // Opening a browser/mail client is best-effort only.
            }
        }
    }
}
