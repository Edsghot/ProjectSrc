using app_matter_data_src_erp.Forms.DialogView;
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class EditarCompra : Form
    {
        public EditarCompra(string codigo)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text = codigo;
        }

        private void lblCoincidencia_Click(object sender, EventArgs e)
        {
            CoincidenciaProductos modal = new CoincidenciaProductos();
            modal.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            var modal = new DialogModal("¿Estás seguro?", "Para realizar una nueva modificación tendrás que hacerla directamente desde el ERP de forma manual.", "question", "", this);
            modal.ShowDialog();
        }
    }
}
