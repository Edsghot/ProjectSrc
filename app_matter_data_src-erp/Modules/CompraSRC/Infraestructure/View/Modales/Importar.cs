using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class Importar : Form
    {

        private readonly MainComprasSrc mainForm;
        private readonly List<string> numCompras;
        private readonly ICompraSrcInputPort compra;
        public Importar(MainComprasSrc main, List<string> NumCompras)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            mainForm = main;
            compra = new CompraSrcAdapter();
            numCompras = NumCompras;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            int mesSeleccionado = cbMes.SelectedIndex + 1;

            // Asegúrate de que cbAño.SelectedValue devuelva el valor correcto
            int anioSeleccionado = Int32.Parse(cbAño.SelectedItem.ToString());

            foreach (var numCompra in numCompras)
            {
                var data = await compra.InsertCompra(mesSeleccionado, anioSeleccionado, numCompra);

                if (data)
                {
                    mainForm.ShowToast($"Datos de la compra {numCompra} importados correctamente.", "success");
                }
                else
                {
                    mainForm.ShowToast($"Error al importar los datos de la compra {numCompra}.", "error");
                }
            }

            this.Close();
        }
    }
}
