using app_matter_data_src_erp.Forms;
using FontAwesome.Sharp; 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace app_matter_data_src_erp
{
    public partial class Main : Form
    {
        private IconButton activeButton; 
        private UserControl activeControl;

        public Main()
        {
            InitializeComponent();
            LoadUserControl(new UCImportacionesCompra(), btnOption1);
        }

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
