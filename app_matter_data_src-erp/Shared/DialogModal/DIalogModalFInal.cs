using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
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
        private readonly ICompraSrcImportadosInputPort port;
        public DIalogModalFInal()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            repo = new CompraSrcRepository();
            port = new CompraSrcImportadosAdapter();
        }

        private void DIalogModalFInal_Load(object sender, EventArgs e)
        {

        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            
            await port.InsertarDelTemporalActualizar(ExtraStatic.idRecepcion);
            var data = await port.GetAllByIdRecepcion(ExtraStatic.idRecepcion);
            repo.UpdateConfiguracionInicial(1).GetAwaiter().GetResult();
            repo.InsertarEliminarComprobanteSrc(ExtraStatic.idRecepcion,data.NCompraErp,data.IdPeriodo,data.FechaLlegada?? DateTime.Now,data.Scop).GetAwaiter();
            
            var resultado = MessageBox.Show(
                "¿Desea dejar de utilizar la aplicación?\nTu importación se actualizará apenas salgas de la aplicación",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                this.Close();
                ControlStatic.CierreModalEditar = true;
                ControlStatic.CierreDIalogvIew = true;
                ControlStatic.CierreTotal = true;
             
            }
            else
            {
                this.Close();
                ControlStatic.CierreModalEditar = true;
                ControlStatic.CierreDIalogvIew = true;
            }  
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
