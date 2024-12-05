
using app_matter_data_src_erp.Forms.DialogView;
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.LoadData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class UCComprasImportadas : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 9;
        private int totalRows;
        private LoadingPanel loadingPanel;

        public UCComprasImportadas()
        {
            InitializeComponent();
            dataTable.CellClick += dataTable_CellClick; dataTable.CellFormatting += dataTable_CellFormatting;
            loadingPanel = new LoadingPanel();  
            this.Controls.Add(loadingPanel);  
            loadingPanel.ShowLoading(false);

            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void LoadData()
        {


            dataTable.Rows.Clear();

            var data = new object[,]
            {
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
            };

            totalRows = data.GetLength(0);

            if (totalRows == 0)
            {
                pictureNone.Visible = true;
            }
            else
            {
                pictureNone.Visible = false;

                for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
                {
                    dataTable.Rows.Add(data[i, 0], data[i, 1], data[i, 2], data[i, 3], data[i, 4],
                                       data[i, 5], data[i, 6], data[i, 7]);
                }
            }

            UpdatePagination();
        }
        // Botones filtrado de tabla
        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            label3.Text = currentPage.ToString();

            iconButton4.Enabled = currentPage > 1;
            iconButton1.Enabled = currentPage < totalPages;
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
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;
                var codigoCompra = dataTable.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();
                if (columnName == "Column8")
                {
                    var modal = new DialogModal("¡Importante!", "Al editar esta compra, se eliminará la importación registrada en el ERP y se creara un nuevo registro de importación.", "warning", codigoCompra);
                    modal.ShowDialog();
                }
            }
        }
        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataTable.Columns[e.ColumnIndex].Name == "Column8")
            {
                e.CellStyle.ForeColor = Color.Chocolate;

                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            loadingPanel.ShowLoading(true);
            Task.Delay(1000).ContinueWith(t =>
            {
                this.Invoke((Action)(() =>
                {
                    loadingPanel.ShowLoading(false);
                    LoadData();
                }));
            });
        }

    }
}
