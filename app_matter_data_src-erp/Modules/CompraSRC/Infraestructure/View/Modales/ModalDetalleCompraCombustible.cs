using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class ModalDetalleCompraCombustible : Form
    {
        private readonly int empresa;

        public ModalDetalleCompraCombustible(string codigo, string emp, List<List<object>> data)
        {
            InitializeComponent();

            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 35;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblEmpresa.Text = emp;
            lblCode.Text = codigo;

            // Validación de txtScop para máximo 17 caracteres
            txtScop.MaxLength = 17;

            // Asegurar que las columnas 5 y 6 existen y son editables
            if (dataTable.Columns.Count >= 6)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (i == 4 || i == 5) // Solo TEMP y API deben ser editables
                    {
                        dataTable.Columns[i].ReadOnly = false;
                        dataTable.Columns[i].DefaultCellStyle.BackColor = Color.LightYellow; // Resaltar en amarillo
                        dataTable.Columns[i].DefaultCellStyle.Font = new Font(dataTable.DefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        dataTable.Columns[i].ReadOnly = true;
                    }
                }
            }

            dataTable.EditingControlShowing += DataTable_EditingControlShowing;

            foreach (var row in data)
            {
                dataTable.Rows.Add(row.ToArray());
            }
        }

        private void DataTable_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataTable.CurrentCell.ColumnIndex == 4 || dataTable.CurrentCell.ColumnIndex == 5) // TEMP y API
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress -= ColumnNumeric_KeyPress;
                    textBox.KeyPress += ColumnNumeric_KeyPress;
                    textBox.Leave -= ColumnNumeric_Leave;
                    textBox.Leave += ColumnNumeric_Leave;
                }
            }
        }

        private void ColumnNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Permitir solo números y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Evitar más de un punto decimal
            if (e.KeyChar == '.' && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }

            // Restringir a máximo dos dígitos enteros antes del punto decimal
            if (char.IsDigit(e.KeyChar) && textBox.Text.Length < 3 && !textBox.Text.Contains("."))
            {
                int parsedValue = int.Parse(textBox.Text + e.KeyChar);
                if (parsedValue > 99)
                {
                    e.Handled = true;
                }
            }
        }

        private void ColumnNumeric_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (decimal.TryParse(textBox.Text, out decimal value))
            {
                if (value > 99.99m)
                {
                    textBox.Text = "99.99"; // Forzar el máximo
                }
                else
                {
                    textBox.Text = value.ToString("0.00"); // Asegurar formato con dos decimales
                }
            }
            else
            {
                textBox.Text = "0.00"; // Valor predeterminado en caso de entrada no válida
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"SCOP: {txtScop.Text}");

            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (!row.IsNewRow) // Evitar la fila vacía al final
                {
                    sb.AppendLine($"Código: {lblCode.Text}, Empresa: {lblEmpresa.Text}, TEMP: {row.Cells[4].Value}, API: {row.Cells[5].Value}");
                }
            }

            MessageBox.Show(sb.ToString(), "Datos Guardados", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
