using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ModalDetalleCompra : Form
    {
        public ModalDetalleCompra(string codigo, List<List<object>> data)
        {
            InitializeComponent();
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text =  codigo;

            foreach (var row in data)
            {
                dataTable.Rows.Add(row.ToArray());  
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
