using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class Importar : Form
    {
        private readonly MainComprasSrc mainForm;
        private readonly List<Tuple<string, string>> codigosYDocumentos; // Lista de tuplas (codigoCompra, documentoProveedor)
        private readonly ICompraSrcInputPort compra;

        public Importar(MainComprasSrc main, List<Tuple<string, string>> CodigosYDocumentos)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            mainForm = main;
            compra = new CompraSrcAdapter();
            codigosYDocumentos = CodigosYDocumentos; // Recibimos la lista de tuplas
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            int mesSeleccionado = cbMes.SelectedIndex + 1;
            int anioSeleccionado = Int32.Parse(cbAño.SelectedItem.ToString());

            var resultado = MessageBox.Show(
                "¿Desea continuar utilizando la aplicación?\nTu importación se actualizará apenas salgas de la aplicación",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                //CAMBIO ED
                //foreach (var numCompra in numCompras)
                //{
                //    var data = await compra.InsertCompra(mesSeleccionado, anioSeleccionado, numCompra);

                //    if (!data)
                //    {
                //        mainForm.ShowToast($"Error al importar los datos de la compra {numCompra}.", "error");
                //    }
                //}

                //ControlStatic.Cierre = true;
            }
            else
            {
                string comprasEnProceso = "Compras marcadas como 'En proceso':\n";

                foreach (var item in DataStaticDto.data)
                {
                   
                    var codDoc = codigosYDocumentos.Find(c => c.Item1 == item.idCompraSerie);
                    if (codDoc != null)
                    {
                        if (item.DocumentoProveedor != codDoc.Item2) 
                        {
                            MessageBox.Show($"El documento proveedor para la compra {item.idCompraSerie} no coincide.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; 
                        }

                        item.Estado = "En proceso";
                        comprasEnProceso += $"- {item.idCompraSerie}\n";
                    }
                }

                MessageBox.Show(comprasEnProceso, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainForm.ShowToast("Las compras seleccionadas se han marcado como 'En proceso'.", "info");
            }

            this.Close();
        }

    }
}
