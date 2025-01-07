using System;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ErrorImportacion : Form
    {
        public ErrorImportacion(string error)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            lblErrores.Text = error;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
