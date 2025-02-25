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
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            int mesSeleccionado = cbMes.SelectedIndex + 1;
            int anioSeleccionado = Int32.Parse(cbAño.SelectedItem.ToString());
            foreach (var idRecepcion in codigosYIdRecepcion)
            {
                var data = await compra.InsertCompra(mesSeleccionado, anioSeleccionado, idRecepcion.Item2);

                if (!data)
                {
                    mainForm.ShowToast($"Error al importar los datos de la compra {idRecepcion.Item1}.", "error");
                    return;
                }
                var id = DataStaticDto.data.FirstOrDefault(x => x.IdRecepcion == idRecepcion.Item2);
                if (id != null)
                {
                    id.Estado = StatusConstant.EnProceso;
                }
            }

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
