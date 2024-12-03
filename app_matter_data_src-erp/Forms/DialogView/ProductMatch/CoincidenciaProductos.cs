using app_matter_data_src_erp.Forms.DialogView.ProductMatch;
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
    public partial class CoincidenciaProductos : Form
    {
        private int currentPage = 1;
        private int rowsPerPage = 4;
        private int totalRows;
        public CoincidenciaProductos()
        {
            InitializeComponent();
            LoadData();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 35;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataTable.CellClick += dataTable_CellClick; dataTable.CellPainting += dataTable_CellPainting;

        }

        private void LoadData()
        {

            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 35;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataTable.Rows.Clear();

            var data = new object[,]
            {
                { "046", "Bdisel db5 ", "diesel db5" },
                { "046", "Producto 02 ", "diesel db5" },
                { "046", "Producto 02 ", "diesel db5" },
                { "046", "Producto 05 ", "diesel db5" },
                { "046", "Producto 04 ", "diesel db5" },
                { "046", "Producto 03 ", "diesel db5" },
            };

            totalRows = data.GetLength(0);


                for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
                {
                    dataTable.Rows.Add(data[i, 0], data[i, 1], data[i, 2]);
                }
          

            UpdatePagination();
        }
        // Botones filtrado de tabla
        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            pageNumberLabel.Text = currentPage.ToString();

            previousPageButton.Enabled = currentPage > 1;
            nextPageButton.Enabled = currentPage < totalPages;
        }

        private void previousPageButton_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void nextPageButton_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }


        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;          

                if (columnName == "Column2")
                {
                    this.Close();
                    BuscarProducto modal = new BuscarProducto();
                    modal.ShowDialog();

                }            
            }
        }

        private void dataTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0) 
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var icon = Properties.Resources.iconEdit; 
                int iconX = e.CellBounds.Right - 20; 
                int iconY = e.CellBounds.Top + (e.CellBounds.Height - icon.Height) / 2;
                e.Graphics.DrawImage(icon, new Point(iconX, iconY));

                e.Handled = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
