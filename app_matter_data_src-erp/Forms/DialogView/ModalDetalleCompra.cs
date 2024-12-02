﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ModalDetalleCompra : Form
    {
        public ModalDetalleCompra(string codigo, List<List<object>> data)
        {
            InitializeComponent();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text =  codigo;

            foreach (var row in data)
            {
                dataGridView1.Rows.Add(row.ToArray());  
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
