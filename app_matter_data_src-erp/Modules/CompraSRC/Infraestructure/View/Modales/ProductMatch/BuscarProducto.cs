using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView.ProductMatch
{

    public partial class BuscarProducto : Form
    {
        private CoincidenciaProductos parentForm;
        public BuscarProducto(CoincidenciaProductos parent)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.StartPosition = FormStartPosition.CenterScreen;
            SetPlaceholder(txtSearch, "¿Qué deseas buscar?");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            this.Close();

            parentForm.Show(); 
            parentForm.Activate(); 
        }
    }
}
