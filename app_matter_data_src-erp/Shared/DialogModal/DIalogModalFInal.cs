using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using System;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Shared.DialogModal
{
    public partial class DIalogModalFInal : Form
    {
        public DIalogModalFInal()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void DIalogModalFInal_Load(object sender, EventArgs e)
        {

        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Desea continuar utilizando la aplicación?\nTu importación se actualizará apenas salgas de la aplicación",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                  ControlStatic.CierreTotal = true;
            }
            else
            {
                ControlStatic.CierreDIalogvIew = true;
                ControlStatic.CierreModalEditar = true;
                ControlStatic.actualizarData = true;
                this.Close();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
