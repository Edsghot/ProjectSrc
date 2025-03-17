using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Constantes;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class Importar : Form
    {
        private readonly MainComprasSrc mainForm;
        private readonly List<Tuple<string, string>> codigosYIdRecepcion;
        private readonly ICompraSrcInputPort compra;

        public Importar(MainComprasSrc main, List<Tuple<string, string>> CodigosYDocumentos)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            mainForm = main;
            compra = new CompraSrcAdapter();
            codigosYIdRecepcion = CodigosYDocumentos;
            this.Load += Importar_Load;
        }
        private void Importar_Load(object sender, EventArgs e)
        {
         
            int mesActual = DateTime.Now.Month;
            int anioActual = DateTime.Now.Year;

            cbMes.SelectedIndex = mesActual - 1; 
            cbAño.SelectedItem = anioActual.ToString();
            ActualizarMensaje();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cbMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarMensaje();
        }

        private void cbAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarMensaje();
        }
        private void ActualizarMensaje()
        {
            if (cbMes.SelectedIndex != -1 && cbAño.SelectedItem != null)
            {
                string mesSeleccionado = cbMes.SelectedItem.ToString();
                string anioSeleccionado = cbAño.SelectedItem.ToString();
                lblMensaje.Text = $"Las compras se migrarán al\nperíodo {mesSeleccionado} {anioSeleccionado}.";
            }
        }
        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            int mesSeleccionado = cbMes.SelectedIndex + 1;
            int anioSeleccionado = Int32.Parse(cbAño.SelectedItem.ToString());
            foreach (var idRecepcion in codigosYIdRecepcion)
            {
                var id = DataStaticDto.data.FirstOrDefault(x => x.IdRecepcion == idRecepcion.Item2);

                if(id.Combustible && (id.Scop == "" || id.Scop == null))
                {
                    MessageBox.Show("Debe agregar el SCOP antes de migrarlo por que se trata de productos de combustible");
                    this.Close();
                    return;
                }

            }


            foreach (var idRecepcion in codigosYIdRecepcion)
            {
                var id = DataStaticDto.data.FirstOrDefault(x => x.IdRecepcion == idRecepcion.Item2);
               
                var data = await compra.InsertCompra(mesSeleccionado, anioSeleccionado, idRecepcion.Item2);

                if (!data)
                {
                    mainForm.ShowToast($"Error al importar los datos de la compra {idRecepcion.Item1}.", "error");
                    return;
                }
                if (id != null)
                {
                    id.Estado = StatusConstant.EnProceso;
                }

            }
            await compra.updateConfiguration(1);

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
                mainForm.ShowToast("Las compras seleccionadas se han marcado como 'En proceso'.", "info");
            }

            this.Close();
        }

    }
}
