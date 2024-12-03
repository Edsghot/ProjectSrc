using app_matter_data_src_erp.Forms.DialogView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class UCImportacionesCompra : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 9;
        private int totalRows;

        public UCImportacionesCompra()
        {
            InitializeComponent();
            dataTable.CellFormatting += dataTable_CellFormatting;
            dataTable.CellClick += dataTable_CellClick;
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
                { "DG5T 279", "2024/11/01", "Pendiente", "Pendiente", "Observación 1...........", 250.00, 18.00, 118.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/02", "Pendiente", "Pendiente", "Observación 2...........", 200.00, 36.00, 236.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/03", "Pendiente", "Pendiente", "Observación 3...........", 150.00, 27.00, 177.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/04", "Pendiente", "Pendiente", "Observación 4...........", 120.00, 21.60, 141.60, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/05", "Pendiente", "Pendiente", "Observación 5...........", 250.00, 45.00, 295.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/01", "Pendiente", "Pendiente", "Observación 1...........", 250.00, 18.00, 118.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/02", "Pendiente", "Pendiente", "Observación 2...........", 200.00, 36.00, 236.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/03", "Pendiente", "Pendiente", "Observación 3...........", 150.00, 27.00, 177.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/04", "Pendiente", "Pendiente", "Observación 4...........", 120.00, 21.60, 141.60, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/05", "Pendiente", "Pendiente", "Observación 5...........", 250.00, 45.00, 295.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/06", "Pendiente", "Pendiente", "Observación 6...........", 210.00, 19.00, 121.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/01", "Pendiente", "Pendiente", "Observación 1...........", 250.00, 18.00, 118.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/02", "Pendiente", "Pendiente", "Observación 2...........", 200.00, 36.00, 236.00, "Pendiente", "Pendiente" },
                { "DG5F 279", "2024/11/03", "Pendiente", "Pendiente", "Observación 3...........", 150.00, 27.00, 177.00, "Pendiente", "Pendiente" },
                { "DG5F 279", "2024/11/04", "Pendiente", "Pendiente", "Observación 4...........", 120.00, 21.60, 141.60, "Pendiente", "Pendiente" },
                { "DG5F 279", "2024/11/05", "Pendiente", "Pendiente", "Observación 5...........", 250.00, 45.00, 295.00, "Pendiente", "Pendiente" },
                { "DG5F 279", "2024/11/02", "Pendiente", "Pendiente", "Observación 2...........", 200.00, 36.00, 236.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/03", "Pendiente", "Pendiente", "Observación 3...........", 150.00, 27.00, 177.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/04", "Pendiente", "Pendiente", "Observación 4...........", 120.00, 21.60, 141.60, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/05", "Pendiente", "Pendiente", "Observación 5...........", 250.00, 45.00, 295.00, "Pendiente", "Pendiente" },
                { "DG5T 279", "2024/11/06", "Pendiente", "Pendiente", "Observación 6...........", 210.00, 19.00, 121.00, "Pendiente", "Pendiente" }
            };

            totalRows = data.GetLength(0);

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                dataTable.Rows.Add(data[i, 0], data[i, 1], data[i, 2], data[i, 3], data[i, 4],
                                   data[i, 5], data[i, 6], data[i, 7], data[i, 8], data[i, 9]);
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

        // TABLA DATOS
        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataTable.Columns[e.ColumnIndex].Name == "Column3" ||
                dataTable.Columns[e.ColumnIndex].Name == "Column4" ||
                dataTable.Columns[e.ColumnIndex].Name == "Column9" ||
                dataTable.Columns[e.ColumnIndex].Name == "Column10")
            {
                if (e.Value?.ToString() == "Pendiente")
                {
                    e.CellStyle.ForeColor = Color.Orange;
                    e.CellStyle.SelectionForeColor = Color.Orange;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.SelectionForeColor = Color.White;
                }
            }

            if (dataTable.Columns[e.ColumnIndex].Name == "Column1")
            {
                e.CellStyle.ForeColor = Color.Brown; 
            }
        }

        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;

                if (columnName == "Column1")
                {
                    var codigoCompra = dataTable.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();

                    List<List<object>> tablaDatos = new List<List<object>>
                    {
                        new List<object> { "02150", "Pollo Freso Entero 2.8 - 3.0", "UNI", 18, 52.14, 52.14, 10.40, 542.26 },
                        new List<object> { "02162", "Pollo Freso  Brasa 1.85 - 3.0", "UNI", 20, 45.62, 45.62, 12.50, 644.25 },
                        new List<object> { "03150", "Pechuga pollo 2.8 - 3.0", "UNI", 34, 54.85, 45.62, 14.70, 344.72 },

                    };

                    List<List<object>> tablaDatosCombsutible = new List<List<object>>
                    {
                        new List<object> { "41510000", 9, 100.00, "GLL", "Endura super diesel B5", 36.90, 77.10, 10.6997, "-", 0.00 ,"-"},

                    };

                    ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible(codigoCompra, tablaDatosCombsutible);
                    //ModalDetalleCompra modal = new ModalDetalleCompra(codigoCompra, tablaDatos);
                    modal.ShowDialog(); 
                }

                if (columnName == "Column3")
                {         
                    Sucursal modal = new Sucursal();
                    modal.ShowDialog();

                }

                if (columnName == "Column4")
                {
                    AsientoTipo modal = new AsientoTipo();
                    modal.ShowDialog();

                }

                if (columnName == "Column9")
                {
                    MessageBox.Show("Aqui sale fecha", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (columnName == "Column10")
                {
                    CoincidenciaProductos modal = new CoincidenciaProductos();
                    modal.ShowDialog();
                }
            }
        }

        // DATA LOADING
        private void UCImportacionesCompra_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            Importar modal = new Importar();
            modal.ShowDialog();
        }
    }
}
