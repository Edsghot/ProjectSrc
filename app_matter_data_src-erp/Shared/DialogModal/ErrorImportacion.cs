using System;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ErrorImportacion : Form
    {
        public ErrorImportacion(string error, string codigoCompra)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            lblErrores.Text = FormatErrorMessage(error);
            lblCodigo.Text = codigoCompra;
        }

        private string FormatErrorMessage(string error)
        {
            var errors = error.Split(new[] { ", " }, StringSplitOptions.None);
            string formattedErrors = "";

            foreach (var err in errors)
            {
                formattedErrors += $"✅ {err.Trim()}\n";
            }

            return formattedErrors;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
