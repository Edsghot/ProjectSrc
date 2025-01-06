using app_matter_data_src_erp.Forms;
using app_matter_data_src_erp.Forms.Overlay;
using FontAwesome.Sharp; 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace app_matter_data_src_erp
{
    public partial class MainComprasSrc : Form
    {
        private IconButton activeButton; 
        private UserControl activeControl;

        private OverlayForm overlay;
        public MainComprasSrc()
        {
            InitializeComponent();         
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadUserControl(new UCImportacionesCompra(), btnOption1);
        }
        // ------------------------------------------------------------- TOAST
        public void ShowToast(string message, string type)
        {
            var toast = new Toast(message, type)
            {
                Location = new Point(this.Width - 630, this.Height - 105),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            this.Controls.Add(toast);
            toast.BringToFront();
        }

        // ------------------------------------------------------------- OVERLAY
        public void ShowOverlay()
        {
            if (overlay == null || overlay.IsDisposed)
            {
                overlay = new OverlayForm(this);
            }
            overlay.Show();
            overlay.BringToFront();
        }

        public void HideOverlay()
        {
            overlay?.Hide();
        }

        // ------------------------------------------------------------- SIDEBAR

        private void LoadUserControl(UserControl userControl, IconButton senderButton)
        {
            if (activeControl != null)
            {
                pnlContainer.Controls.Remove(activeControl);
            }

            activeControl = userControl;
            activeControl.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(activeControl);

            HighlightButton(senderButton);
        }

        private void HighlightButton(IconButton button)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.White;
                activeButton.ForeColor = Color.Black;
                activeButton.IconColor = Color.Black;
            }

            button.BackColor = Color.Blue;
            button.ForeColor = Color.White;
            button.IconColor = Color.White;

            activeButton = button; 
        }

        private void btnOption1_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UCImportacionesCompra(), (IconButton)sender);
        }

        private void btnOption2_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UCComprasImportadas(), (IconButton)sender);
        }
    }
}
