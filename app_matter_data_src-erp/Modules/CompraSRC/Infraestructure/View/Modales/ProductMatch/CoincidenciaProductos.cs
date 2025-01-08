using app_matter_data_src_erp.Forms.DialogView.ProductMatch;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class CoincidenciaProductos : Form
    {
        private int currentPage = 1;
        private int rowsPerPage = 4;
        private int totalRows;

        private readonly MainComprasSrc mainForm;
        private readonly List<CompraDetalleDto> detallesCompra;
        public CoincidenciaProductos(MainComprasSrc mainForm, string codigoCompra, List<CompraDetalleDto> detallesCompra)
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

            dataTable.CellClick += dataTable_CellClick;
            dataTable.CellPainting += dataTable_CellPainting;
            lblNumeroCompra.Text = codigoCompra;
            LoadData(detallesCompra);
            this.detallesCompra = detallesCompra;
            this.mainForm = mainForm;
        }

        private void LoadData(List<CompraDetalleDto> detallesCompra)
        {
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 45;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataTable.Rows.Clear();

            totalRows = detallesCompra.Count;

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                var detalle = detallesCompra[i];
                dataTable.Rows.Add("046", "Bdisel db5", detalle.Descripcion);
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
                    this.Hide();
                    BuscarProducto modal = new BuscarProducto(this);
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

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            mainForm.ShowToast("Se realizaron los cambios con éxito.", "success");
            this.Close();
        }
    }
}

  

  