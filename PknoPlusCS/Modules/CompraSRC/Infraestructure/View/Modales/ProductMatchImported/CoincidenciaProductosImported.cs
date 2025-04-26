

    using PknoPlusCS.Forms.DialogView.ProductMatch;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PknoPlusCS.Forms.DialogView
{
    public partial class CoincidenciaProductosImported : Form
    {
        private int currentPage = 1;
        private int rowsPerPage = 4;
        private int totalRows;
        private string rucRecuperado;
        private readonly MainComprasSrc mainForm;
        private readonly ICompraSrcRepository _repo;
        private readonly List<CompraTemporalMonitoreoSrcDto> detallesCompra;
        private readonly ICompraSrcInputPort compra;
        private readonly int _index;
        private readonly string CodigoCompraCompleto;
        public CoincidenciaProductosImported(MainComprasSrc parent, List<CompraTemporalMonitoreoSrcDto> compras, string codigoCompra,string ruc)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            compra = new CompraSrcAdapter();
            this.dataTable.RowTemplate.Height = 45;
            this.dataTable.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.dataTable.Columns[1].DefaultCellStyle.ForeColor = Color.Chocolate;
            this.dataTable.Columns[1].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Underline);

            this.dataTable.Columns[2].HeaderCell.Style.BackColor = Color.LightSteelBlue;
            _repo = new CompraSrcRepository();
            dataTable.CellClick += dataTable_CellClick;
            dataTable.CellPainting += dataTable_CellPainting;
            lblNumeroCompra.Text = codigoCompra;
            CodigoCompraCompleto = codigoCompra;


            this.detallesCompra = compras;
            this.rucRecuperado = ruc;


            LoadData(detallesCompra).GetAwaiter().GetResult();
        }
        private async Task LoadData(List<CompraTemporalMonitoreoSrcDto> detallesCompra)
        {
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 45;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataTable.Rows.Clear();
            var coincidencia2 = new List<CompraTemporalMonitoreoSrcDto>();

            totalRows = detallesCompra.Count;
            List<CoincidenciaProdSrcDto> coincidencias = new List<CoincidenciaProdSrcDto>();
            try
            {
                coincidencias =  _repo.ObtenerCoincidenciasProdSrcPorRuc(this.rucRecuperado);

                 coincidencia2 =  _repo.ObtenerCompraMonitoreoTemporalPorIdRecepcion(ExtraStatic.idRecepcion);
                txtScop.Text = coincidencia2[0].Scop;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener coincidencias por RUC: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                var detalle = detallesCompra[i];
                var coincidencia = coincidencias.FirstOrDefault(c => c.NombreProdSrc == detalle.NomProductoSrc);
                var con = coincidencia2.FirstOrDefault(c => c.NomProductoSrc == detalle.NomProductoSrc);
                if (coincidencia != null)
                {
                    dataTable.Rows.Add(coincidencia.IdProductoErp, coincidencia.NombreProdErp, coincidencia.NombreProdSrc, con.Api,con.Temperatura);
                }
                else
                {
                    try
                    {
                        var response =  _repo.searchProducts(detalle.NomProductoSrc);
                        if (response != null && response.Any())
                        {
                            var firstProduct = response.First();
                            dataTable.Rows.Add(firstProduct.ProductId, firstProduct.ProductName, detalle.NomProductoSrc);
                        }
                        else
                        {
                            dataTable.Rows.Add("", "", detalle.NomProductoSrc);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al consumir la API: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataTable.Rows.Add("", "", detalle.NomProductoSrc);
                    }
                }
            }
            foreach (DataGridViewColumn column in dataTable.Columns)
            {
                column.ReadOnly = true;
            }
           
            txtScop.MaxLength = 17;

            dataTable.Columns[3].ReadOnly = false; 
            dataTable.Columns[4].ReadOnly = false;

            dataTable.Columns[3].DefaultCellStyle.BackColor = Color.LightYellow; 
            dataTable.Columns[3].DefaultCellStyle.Font = new Font(dataTable.DefaultCellStyle.Font, FontStyle.Bold);
            dataTable.Columns[4].DefaultCellStyle.BackColor = Color.LightYellow; 
            dataTable.Columns[4].DefaultCellStyle.Font = new Font(dataTable.DefaultCellStyle.Font, FontStyle.Bold);
            dataTable.EditingControlShowing += DataTable_EditingControlShowing;

            UpdatePagination();
        }



        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            pageNumberLabel.Text = currentPage.ToString();

            previousPageButton.Enabled = currentPage > 1;
            nextPageButton.Enabled = currentPage < totalPages;
        }

        private async void previousPageButton_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData(detallesCompra);
            }
        }

        private async void nextPageButton_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                await LoadData(detallesCompra);
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


        //ultimo
        private void DataTable_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataTable.CurrentCell.ColumnIndex == 3 || dataTable.CurrentCell.ColumnIndex == 4) 
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

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }


            if (e.KeyChar == '.' && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }

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
                    textBox.Text = "99.99"; 
                }
                else
                {
                    textBox.Text = value.ToString("0.00"); 
                }
            }
            else
            {
                textBox.Text = "0.00"; 
            }
        }



        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()) ||
                    row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ||
                    row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                {
                    MessageBox.Show("Debe completar todas las filas antes de continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                foreach (DataGridViewRow row in dataTable.Rows)
                {
                    string idProductoErp2 = row.Cells[0].Value.ToString();
                    string nombreProdErp2 = row.Cells[1].Value.ToString();
                    string nombreProdSrc2 = row.Cells[2].Value.ToString();
                    var api = decimal.Parse(row.Cells[3].Value.ToString());
                    var temp = decimal.Parse(row.Cells[4].Value.ToString());

                    var productoInsertar = new InsertProdCuencidenciaDto
                    {
                        IdProductoErp = idProductoErp2,
                        NombreProdErp = nombreProdErp2,
                        NombreProdSrc = nombreProdSrc2,
                        RucEmpresa = rucRecuperado
                    };

                    await compra.EscanearDCompra(idProductoErp2, nombreProdSrc2);
                    _repo.InsertProdCuencidencia(productoInsertar);

                    var arr = CodigoCompraCompleto.Split('-');

                     _repo.ActualizarProductoCompraTemporalMonitoreoSRC(idProductoErp2, arr[1], arr[0], nombreProdSrc2, api, temp,txtScop.Text);
                    DataStaticDto.data[_index].Coicidencia = "Revisado";
                }

                if (mainForm != null)
                {
                    mainForm.ShowToast("Se registró con éxito.", "success");
                }
                else
                {
                    MessageBox.Show("Se registró con éxito.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Close();
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


        private void dataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}



