using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Shared.DialogModal
{
    public partial class DIalogModalFInal : Form
    {
        private readonly ICompraSrcRepository repo;
        public DIalogModalFInal()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            repo = new CompraSrcRepository();
        }

        private void DIalogModalFInal_Load(object sender, EventArgs e)
        {

        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {

            repo.InsertarEliminarComprobanteSrc(ExtraStatic.idRecepcion).GetAwaiter();

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
