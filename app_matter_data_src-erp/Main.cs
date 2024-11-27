using app_matter_data_src_erp.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp
{
    public partial class Main : Form
    {
        private Button activeButton;
        private UserControl activeControl;
        public Main()
        {
            InitializeComponent();
            LoadUserControl(new UCImportacionesCompra(), btnOption1);
        }

        private void LoadUserControl(UserControl userControl, Button senderButton)
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

        private void HighlightButton(Button button)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.White;
                activeButton.ForeColor = Color.Black;
            }

            button.BackColor = Color.Blue;
            button.ForeColor = Color.White;

            activeButton = button;
        }


        private void btnOption1_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UCImportacionesCompra(), (Button)sender);
        }

        private void btnOption2_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UCComprasImportadas(), (Button)sender);
        }
    }
}
