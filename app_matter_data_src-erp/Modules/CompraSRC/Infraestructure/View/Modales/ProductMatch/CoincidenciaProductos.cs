using app_matter_data_src_erp.Forms.DialogView.ProductMatch;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class CoincidenciaProductos : Form
    {
        private int currentPage = 1;
        private int rowsPerPage = 4;
        private int totalRows;
        private string rucRecuperado;

        private readonly MainComprasSrc mainForm;
        private readonly ICompraSrcRepository _repo;
        private readonly List<CompraDetalleDto> detallesCompra;
        private readonly int _index;
        public CoincidenciaProductos(MainComprasSrc main, string codigoCompra, List<CompraDetalleDto> detallesCompra, string ruc, int index)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 45;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dataTable.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.dataTable.Columns[1].DefaultCellStyle.ForeColor = Color.Chocolate;
            this.dataTable.Columns[1].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Underline);

            this.dataTable.Columns[2].HeaderCell.Style.BackColor = Color.LightSteelBlue;
            _repo = new CompraSrcRepository();
            _index = index;
            dataTable.CellClick += dataTable_CellClick;
            dataTable.CellPainting += dataTable_CellPainting;
            lblNumeroCompra.Text = codigoCompra;
           

            this.detallesCompra = detallesCompra;
            this.mainForm = main;
            this.rucRecuperado = ruc;

            LoadData(detallesCompra);
        }
        private async Task LoadData(List<CompraDetalleDto> detallesCompra)
        {
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 45;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataTable.Rows.Clear();

            totalRows = detallesCompra.Count;
            List<CoincidenciaProdSrcDto> coincidencias = new List<CoincidenciaProdSrcDto>();
            try
            {
                coincidencias = await _repo.ObtenerCoincidenciasProdSrcPorRuc(this.rucRecuperado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener coincidencias por RUC: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                var detalle = detallesCompra[i];
                var coincidencia = coincidencias.FirstOrDefault(c => c.NombreProdSrc == detalle.Descripcion);

                if (coincidencia != null)
                {
                    dataTable.Rows.Add(coincidencia.IdProductoErp, coincidencia.NombreProdErp, coincidencia.NombreProdSrc);
                }
                else
                {
                    try
                    {
                        var response = await _repo.searchProducts(detalle.Descripcion);
                        if (response != null && response.Any())
                        {
                            var firstProduct = response.First();
                            dataTable.Rows.Add(firstProduct.ProductId, firstProduct.ProductName, detalle.Descripcion);
                        }
                        else
                        {
                            dataTable.Rows.Add("", "", detalle.Descripcion);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al consumir la API: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataTable.Rows.Add("", "", detalle.Descripcion);
                    }
                }
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
                LoadData(detallesCompra);
            }
        }

        private void nextPageButton_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData(detallesCompra);
            }
        }


        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;

                if (columnName == "Column2")
                {
                    var selectedRow = dataTable.Rows[e.RowIndex];
                    string productId = selectedRow.Cells[0].Value.ToString();
                    string productName = selectedRow.Cells[1].Value.ToString();

                    var modal = new BuscarProducto(productId, productName);
                    modal.StartPosition = FormStartPosition.CenterScreen;
                    modal.TopMost = true;

                    modal.ShowDialog();

                    if (!string.IsNullOrEmpty(modal.GetSelectedProduct()))
                    {

                        selectedRow.Cells[1].Value = modal.SelectedProductName;
                        selectedRow.Cells[0].Value = modal.SelectedProductId;
                    }
                }
            }
        }

        private void dataTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var icon = Properties.Resources.iconEdit;
                int iconX = e.CellBounds.Right - 25;
                int iconY = e.CellBounds.Top + (e.CellBounds.Height - icon.Height) / 2;
                e.Graphics.DrawImage(icon, new Point(iconX, iconY));

                e.Handled = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach(DataGridViewRow row in dataTable.Rows)
                {
                    string idProductoErp2 = row.Cells[0].Value.ToString();
                    string nombreProdErp2 = row.Cells[1].Value.ToString();
                    string nombreProdSrc2 = row.Cells[2].Value.ToString();

                    string mensaje = $"Producto ERP ID: {idProductoErp2}\n" +
                                     $"Nombre Producto ERP: {nombreProdErp2}\n" +
                                     $"Nombre Producto SRC: {nombreProdSrc2}";

                   // MessageBox.Show(mensaje, "Detalles del Producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        var idProductoErp = row.Cells[0].Value.ToString();
                        var nombreProdErp = row.Cells[1].Value.ToString();
                        var nombreProdSrc = row.Cells[2].Value.ToString();

                        var productoInsertar = new InsertProdCuencidenciaDto
                        {
                            IdProductoErp = idProductoErp,
                            NombreProdErp = nombreProdErp,
                            NombreProdSrc = nombreProdSrc,
                            RucEmpresa = rucRecuperado
                        };
                        var response = _repo.InsertProdCuencidencia(productoInsertar).GetAwaiter();
                        DataStaticDto.data[_index].Coicidencia = "Revisado";
                       
                        if (mainForm != null)
                        {
                            mainForm.ShowToast("Se registro con exito.", "success");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Se registro con exito!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {                        
                if (mainForm != null)
                {
                    mainForm.ShowToast($"Ocurrió un error: {ex.Message}", "error");

                }
                else
                {
                    MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}



