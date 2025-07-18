using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyMusicPlayer
{
    public partial class HelpDialog : Form
    {
        private System.ComponentModel.IContainer? components = null;
        private Label? lblVersion;
        private Label? lblDevelopmentMessage;
        private TextBox? txtDonationUrl;
        private ActionButton? btnClose;
        private ActionButton? btnCopyUrl;

        public HelpDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblVersion = new Label();
            this.lblDevelopmentMessage = new Label();
            this.txtDonationUrl = new TextBox();
            this.btnClose = new ActionButton();
            this.btnCopyUrl = new ActionButton();
            this.SuspendLayout();

            // Form properties
            this.Text = "Help - MyMusicPlayer";
            this.Size = new Size(500, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ShowIcon = false;

            // Version label
            this.lblVersion.Text = "Jerry Player Version 1.1.1";
            this.lblVersion.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblVersion.ForeColor = ColorTranslator.FromHtml("#D2691E");
            this.lblVersion.Location = new Point(20, 20);
            this.lblVersion.Size = new Size(450, 40);
            this.lblVersion.TextAlign = ContentAlignment.MiddleCenter;

            // Development message label
            this.lblDevelopmentMessage.Text = "Development is time consuming.\nPlease consider supporting:";
            this.lblDevelopmentMessage.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.lblDevelopmentMessage.ForeColor = Color.White;
            this.lblDevelopmentMessage.Location = new Point(20, 80);
            this.lblDevelopmentMessage.Size = new Size(450, 60);
            this.lblDevelopmentMessage.TextAlign = ContentAlignment.MiddleCenter;

            // Donation URL text box
            this.txtDonationUrl.Text = "https://buymeacoffee.com/jerryplayer";
            this.txtDonationUrl.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.txtDonationUrl.Location = new Point(20, 160);
            this.txtDonationUrl.Size = new Size(350, 30);
            this.txtDonationUrl.ReadOnly = true;
            this.txtDonationUrl.BackColor = Color.FromArgb(60, 60, 65);
            this.txtDonationUrl.ForeColor = Color.White;
            this.txtDonationUrl.BorderStyle = BorderStyle.FixedSingle;

            // Copy URL button
            this.btnCopyUrl.Text = "Copy";
            this.btnCopyUrl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.btnCopyUrl.Location = new Point(380, 160);
            this.btnCopyUrl.Size = new Size(90, 30);
            this.btnCopyUrl.BorderRadius = 8;
            this.btnCopyUrl.UseVisualStyleBackColor = false;
            this.btnCopyUrl.Click += new EventHandler(this.BtnCopyUrl_Click);

            // Close button
            this.btnClose.Text = "Close";
            this.btnClose.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.btnClose.Location = new Point(195, 220);
            this.btnClose.Size = new Size(100, 35);
            this.btnClose.BorderRadius = 8;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);

            // Add controls to form
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblDevelopmentMessage);
            this.Controls.Add(this.txtDonationUrl);
            this.Controls.Add(this.btnCopyUrl);
            this.Controls.Add(this.btnClose);

            this.ResumeLayout(false);
        }

        private void BtnCopyUrl_Click(object? sender, EventArgs e)
        {
            try
            {
                if (txtDonationUrl != null)
                {
                    Clipboard.SetText(txtDonationUrl.Text);
                    MessageBox.Show("URL copied to clipboard!", "Copied", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy URL: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
