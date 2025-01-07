using app_matter_data_src_erp.Forms.DialogView;
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class UCImportacionesCompra : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 14;
        private int totalRows;
        private readonly ICompraSrcInputPort _compraSrc;
        public UCImportacionesCompra()
        {
            InitializeComponent();
            dataTable.CellFormatting += dataTable_CellFormatting;
            dataTable.CellClick += dataTable_CellClick;

            dateInicio.ValueChanged += (sender, args) =>
            {
                lblDateInicio.Text = dateInicio.Value.ToString("dd/MM/yyyy");
            };
            dateFin.ValueChanged += (sender, args) =>
            {
                lblDateFin.Text = dateFin.Value.ToString("dd/MM/yyyy");
            };

            _compraSrc = new CompraSrcAdapter();
            LoadData();
        }

        private async void LoadData()
        {
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataTable.Rows.Clear();

            var data = await _compraSrc.ObtenerDataSrc();

            if (data == null || data.Count == 0)
            {
                MessageBox.Show("No se encontraron datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            totalRows = data.Count;

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                var compra = data[i];

                dataTable.Rows.Add(
                    compra.NumCompra,
                    compra.FechaEmision.ToString("dd/MM/yyyy"),
                    compra.Sucursal,
                    compra.RazonSocial,
                    compra.Observacion,
                    compra.TotalGravadas + compra.TotalExoneradas + compra.TotalOtrosTributos + compra.TotalPercepcion,
                    compra.TotalIGV,
                    compra.TotalPagar,
                    compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                    compra.Scop
                );
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

        // TABLA FORMATO
        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataTable.Columns[e.ColumnIndex].Name == "Column3" ||
                dataTable.Columns[e.ColumnIndex].Name == "Column4" ||
                dataTable.Columns[e.ColumnIndex].Name == "Column9" ||
                dataTable.Columns[e.ColumnIndex].Name == "Column10")
            {
                if (e.Value?.ToString() == "Pendiente")
                {
                    e.CellStyle.ForeColor = Color.Chocolate;
                    e.CellStyle.SelectionForeColor = Color.Chocolate;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                }

                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }

            if (dataTable.Columns[e.ColumnIndex].Name == "Column1")
            {
                e.CellStyle.ForeColor = Color.Chocolate;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }

            if (dataTable.Columns[e.ColumnIndex].Name == "Column11")
            {
                var row = dataTable.Rows[e.RowIndex];

                var firstColumnValue = row.Cells["Column1"].Value?.ToString();

                var validationErrors = _compraSrc.ValidateColumn(1, firstColumnValue);

                if (validationErrors.Any())
                {
                    row.Cells["Column11"].Value = "No Listo";
                    e.CellStyle.ForeColor = Color.Red;
                }
                else
                {
                    row.Cells["Column11"].Value = "Listo";
                    e.CellStyle.ForeColor = Color.Green;
                }

                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }
        }


        // TABLA CLICK
        private async void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;
                OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);
                if (columnName == "Column1")
                {
                    var codigoCompra = dataTable.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();

                    if (!string.IsNullOrEmpty(codigoCompra))
                    {
                        CompraDto compra = await _compraSrc.ObtenerCompraPorCodigo(codigoCompra);

                        if (compra != null)
                        {
                            List<CompraDetalleDto> detallesCompra = compra.Compras;

                            List<List<object>> tablaDatos = detallesCompra.Select(detalle => new List<object>
                            {
                                detalle.Codigo,
                                1,
                                detalle.Cantidad,
                                detalle.PrecioUnitario,
                                detalle.SubTotal,
                                detalle.Descripcion,
                                detalle.Api,
                                detalle.Temp,
                                detalle.Fise,
                                detalle.Dscto,
                                detalle.Isc,
                                detalle.Fise,
                                detalle.Total,
                                detalle.Igv,
                                detalle.SubTotal

                            }).ToList();

                            //ModalDetalleCompra modal = new ModalDetalleCompra(codigoCompra, tablaDatos);
                            ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible(codigoCompra, tablaDatos);

                            overlayForm.ShowOverlayWithModal(modal);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró la compra con el código especificado.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El código de compra no es válido.");
                    }
                }

                if (columnName == "Column3")
                {
                    Sucursal modal = new Sucursal((MainComprasSrc)this.ParentForm);
                    overlayForm.ShowOverlayWithModal(modal);
                }

                if (columnName == "Column4")
                {
                    AsientoTipo modal = new AsientoTipo((MainComprasSrc)this.ParentForm);
                    overlayForm.ShowOverlayWithModal(modal);
                }

                if (columnName == "Column9")
                {
                    DateTimePicker dateTimePicker = new DateTimePicker();
                    dateTimePicker.Format = DateTimePickerFormat.Short;

                    Rectangle rect = dataTable.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dateTimePicker.Size = new Size(rect.Width, rect.Height);
                    dateTimePicker.Location = new Point(rect.Left, rect.Top + 6);

                    dataTable.Controls.Add(dateTimePicker);

                    dateTimePicker.ValueChanged += (s, args) =>
                    {
                        dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dateTimePicker.Value.ToString("dd/MM/yyyy");
                        dataTable.Controls.Remove(dateTimePicker);
                    };

                    dateTimePicker.Leave += (s, args) =>
                    {
                        dataTable.Controls.Remove(dateTimePicker);
                    };

                    dateTimePicker.Focus();
                }

                if (columnName == "Column10")
                {
                    CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm);
                    overlayForm.ShowOverlayWithModal(modal);
                }
                if (columnName == "Column11")
                {
                    var row = dataTable.Rows[e.RowIndex];
                    var validationErrors = _compraSrc.ValidateColumn(1, row.Cells["Column1"].Value?.ToString());

                    if (row.Cells["Column11"].Value?.ToString() == "No Listo" && validationErrors.Any())
                    {
                        var errorMessages = string.Join(Environment.NewLine, validationErrors.Select(err => err.Message));
                        ErrorImportacion modal = new ErrorImportacion(errorMessages);
                        overlayForm.ShowOverlayWithModal(modal);
                       
                    }
                }
            }
        }

        // DATA LOADING
        private void UCImportacionesCompra_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void btnImportar_Click(object sender, EventArgs e)
        {
            OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);
            Importar modal = new Importar((MainComprasSrc)this.ParentForm);
            overlayForm.ShowOverlayWithModal(modal);
        }

        private async void btnEscanear_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            currentPage = 1;
            var mainForm = (MainComprasSrc)this.FindForm();
            mainForm.ShowOverlay();
            await Task.Delay(5000);
            LoadData();
            mainForm.HideOverlay();
        }
    }
}


