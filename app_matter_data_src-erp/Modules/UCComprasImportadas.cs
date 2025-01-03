
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class UCComprasImportadas : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 15;
        private int totalRows;

        public UCComprasImportadas()
        {
            InitializeComponent();
            dataTable.CellClick += dataTable_CellClick; dataTable.CellFormatting += dataTable_CellFormatting;

            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataTable.ColumnHeadersHeight = 45;

            cbMes.SelectedItem = SeleccionesGlobales.MesSeleccionado;
            cbAño.SelectedItem = SeleccionesGlobales.AñoSeleccionado;

            if (DatosGlobales.Compras.Count > 0)
            {
                LoadData();
            }

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
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
                { "DG5T 279913", "Ejemplo", "29/10/2024",250.00, 45.00, 295.00, "Importado", "Editar" },
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

            for (int i = 0; i < totalRows; i++)
            {
                var compra = new Compra
                {
                    Codigo = data[i, 0].ToString(),
                    Nombre = data[i, 1].ToString(),
                    Fecha = data[i, 2].ToString(),
                    Precio = Convert.ToDecimal(data[i, 3]),
                    Descuento = Convert.ToDecimal(data[i, 4]),
                    Total = Convert.ToDecimal(data[i, 5]),
                    Estado = data[i, 6].ToString(),
                    Accion = data[i, 7].ToString()
                };

                DatosGlobales.Compras.Add(compra);        
            }

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

                OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);


                if (columnName == "Column8")
                {
                    var modal = new DialogModal("¡Importante!", "Al editar esta compra, se eliminará la importación registrada en el ERP y se creara un nuevo registro de importación.", "warning", codigoCompra);
                    overlayForm.ShowOverlayWithModal(modal);
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

        private async void btnLimpiar_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            DatosGlobales.Compras.Clear();
            SeleccionesGlobales.MesSeleccionado = null; 
            SeleccionesGlobales.AñoSeleccionado = null;
            cbMes.Text = "Seleccione el mes";
            cbAño.Text = "Seleccione el año";

            pictureNone.Visible = true;
            currentPage = 1;  
            UpdatePagination(); 
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var mainForm = (MainComprasSrc)this.FindForm();
            dataTable.Rows.Clear();
            currentPage = 1;
           
            if (cbMes.Text != "Seleccione el mes" && cbAño.Text != "Seleccione el mes")
            {
                pictureNone.Visible = false;
                mainForm.ShowOverlay();
                await Task.Delay(3000);
                LoadData();
                mainForm.HideOverlay();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un mes y un año.",
                                "Campos vacíos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }

        }

        private void cbMes_SelectedIndexChanged(object sender, EventArgs e)
        {          
            SeleccionesGlobales.MesSeleccionado = cbMes.SelectedItem.ToString();
        }

        private void cbAño_SelectedIndexChanged(object sender, EventArgs e)
        {          
            SeleccionesGlobales.AñoSeleccionado = cbAño.SelectedItem.ToString();
        }
    }
}
