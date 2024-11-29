using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ModalDetalleCompra : Form
    {
        public ModalDetalleCompra(string codigo, List<List<object>> data)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text =  codigo;

            foreach (var row in data)
            {
                dataGridView1.Rows.Add(row.ToArray());  
            }
        }
    }
}
