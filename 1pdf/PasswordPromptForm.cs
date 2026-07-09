using System.Drawing;
using System.Windows.Forms;
using _1pdf.Services;

namespace _1pdf
{
    /// <summary>
    /// Modal dialog that asks for the password of a protected PDF.
    /// The entered password is exposed via <see cref="Password"/>;
    /// storing it is the caller's responsibility.
    /// </summary>
    public class PasswordPromptForm : Form
    {
        private readonly TextBox _passwordTextBox;

        public string Password => _passwordTextBox.Text;

        /// <param name="fileName">Display name of the protected file.</param>
        /// <param name="wrongPassword">True when a previous attempt failed.</param>
        public PasswordPromptForm(string fileName, bool wrongPassword, LocalizationService localization)
        {
            Text = localization.GetString("PasswordTitle");
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(420, 150);

            string message = localization.GetString("PasswordPrompt", fileName);
            if (wrongPassword)
            {
                message = localization.GetString("PasswordWrong") + "\r\n" + message;
            }

            var messageLabel = new Label
            {
                Text = message,
                AutoSize = false,
                Size = new Size(ClientSize.Width - 24, 55),
                Location = new Point(12, 10)
            };

            _passwordTextBox = new TextBox
            {
                UseSystemPasswordChar = true,
                Size = new Size(ClientSize.Width - 24, 23),
                Location = new Point(12, 70)
            };

            var okButton = new Button
            {
                Text = localization.GetString("ButtonOk"),
                DialogResult = DialogResult.OK,
                Size = new Size(90, 28),
                Location = new Point(ClientSize.Width - 200, 108)
            };

            var cancelButton = new Button
            {
                Text = localization.GetString("ButtonCancel"),
                DialogResult = DialogResult.Cancel,
                Size = new Size(90, 28),
                Location = new Point(ClientSize.Width - 102, 108)
            };

            Controls.AddRange(new Control[] { messageLabel, _passwordTextBox, okButton, cancelButton });
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }
    }
}
