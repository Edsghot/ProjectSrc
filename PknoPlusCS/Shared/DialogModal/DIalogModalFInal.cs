using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Static;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Windows.Forms;

namespace PknoPlusCS.Shared.DialogModal
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
            
            port.InsertarDelTemporalActualizar(ExtraStatic.idRecepcion).GetAwaiter().GetResult();
            var data = await port.GetAllByIdRecepcion(ExtraStatic.idRecepcion);
             repo.UpdateConfiguracionInicial(1);
            repo.InsertarEliminarComprobanteSrc(ExtraStatic.idRecepcion,data.NCompraErp,data.IdPeriodo,data.FechaLlegada?? DateTime.Now,data.Scop);
            
            var resultado = MessageBox.Show(
                "La edicion del registro se realizara en breve... \n Desea cerrar el formulario?", "Confirmación",
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

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
