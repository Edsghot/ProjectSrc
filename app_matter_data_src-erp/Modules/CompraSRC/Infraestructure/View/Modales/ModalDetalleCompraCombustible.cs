using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ModalDetalleCompraCombustible : Form
    {
        private readonly int empresa;
        public ModalDetalleCompraCombustible(string codigo,string emp, List<List<object>> data)
        {
            InitializeComponent();
     
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 35;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.StartPosition = FormStartPosition.CenterScreen;

            lblEmpresa.Text = emp;
            lblCode.Text = codigo;

            foreach (var row in data)
            {
                dataTable.Rows.Add(row.ToArray());
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
