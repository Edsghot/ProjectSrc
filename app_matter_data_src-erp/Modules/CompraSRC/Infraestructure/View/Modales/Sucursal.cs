using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class Sucursal : Form
    {

        private readonly MainComprasSrc mainForm;
        private readonly int _index;
        public Sucursal(MainComprasSrc mainForm, string direccion,int index)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = mainForm;
            txtDireccion.Text = string.IsNullOrEmpty(direccion) ? "Pendiente" : direccion;
            _index = index;
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            DataStaticDto.data[_index].Sucursal = txtDireccion.Text;

            mainForm.ShowToast("Datos de la sucursal añadidos con éxito.", "success");
            this.Close();
        }
    }
}
