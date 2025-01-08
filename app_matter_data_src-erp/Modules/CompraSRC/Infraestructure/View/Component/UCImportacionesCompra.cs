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
using System.Windows.Media.Media3D;

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
                    compra.TotalPagar - compra.TotalIGV,
                    compra.TotalIGV,
                    compra.TotalPagar,
                    compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                    compra.RazonSocial,
                    compra.Errores
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
                var columnName = dataTable.Columns[e.ColumnIndex].Name;
                if (columnName == "Column3" || columnName == "Column4" || columnName == "Column9" || columnName == "Column10")
                {
                    e.Value = "Pendiente";
                    e.CellStyle.ForeColor = Color.Chocolate;
                    e.CellStyle.SelectionForeColor = Color.Chocolate;
                    if(columnName == "Column9" && DataStaticDto.data[dataTable.Rows[e.RowIndex].Index].FechaLlegada != null)
                    {
                        e.Value = DataStaticDto.data[dataTable.Rows[e.RowIndex].Index].FechaLlegada;
                        e.CellStyle.ForeColor = Color.Black;
                        e.CellStyle.SelectionForeColor = Color.Black;
                    }
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
                var errores = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                e.Value = "No Listo";
                e.CellStyle.ForeColor = Color.Chocolate;
                e.CellStyle.SelectionForeColor = Color.Chocolate;

                if (!string.IsNullOrEmpty(errores))
                {
                    e.Value = errores.Length < 0 ? "Listo" : "Error";
                    e.CellStyle.ForeColor = errores.Length < 0 ? Color.Green : Color.Red;
                    e.CellStyle.SelectionForeColor = Color.Red;
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

                            List<List<object>> tablaDatosCombustible = detallesCompra.Select(detalle => new List<object>
                            {
                                detalle.Codigo,
                                detalle.Cantidad,
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

                            List<List<object>> tablaDatos = detallesCompra.Select(detalle => new List<object>
                            {
                                detalle.Codigo,
                                detalle.Descripcion,
                                detalle.Cantidad,
                                detalle.Cantidad,
                                detalle.PrecioUnitario,
                                detalle.Cantidad,
                                detalle.PrecioUnitario,
                                detalle.Total,
                            }).ToList();

                            bool validacion = detallesCompra.All(detalle => detalle.Api == 0.00m || detalle.Temp == 0.00m);

                            if (validacion)
                            {
                                ModalDetalleCompra modal = new ModalDetalleCompra(codigoCompra, tablaDatos);
                                overlayForm.ShowOverlayWithModal(modal);
                            }
                            else
                            {
                                ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible(codigoCompra, tablaDatosCombustible);
                                overlayForm.ShowOverlayWithModal(modal);
                            }
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
                    string direccion = DataStaticDto.data[dataTable.Rows[e.RowIndex].Index].Sucursal;

                    Sucursal modal = new Sucursal((MainComprasSrc)this.ParentForm, direccion, dataTable.Rows[e.RowIndex].Index);
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

                    var cellValue = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    if (DateTime.TryParse(cellValue, out DateTime parsedDate))
                    {
                        dateTimePicker.Value = parsedDate;
                    }
                    else
                    {
                        dateTimePicker.Value = DateTime.Now;
                    }

                    Rectangle rect = dataTable.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dateTimePicker.Size = new Size(rect.Width, rect.Height);
                    dateTimePicker.Location = new Point(rect.Left, rect.Top + 6);

                    dataTable.Controls.Add(dateTimePicker);

                    dateTimePicker.ValueChanged += (s, args) =>
                    {
                        dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dateTimePicker.Value.ToString("dd/MM/yyyy");
                        DataStaticDto.data[dataTable.Rows[e.RowIndex].Index].FechaLlegada = dateTimePicker.Value.ToString("dd/MM/yyyy");
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
                    string codigoCompra = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    CompraDto compra = await _compraSrc.ObtenerCompraPorCodigo(codigoCompra);

                    CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm, compra.NumCompra, compra.Compras);
                    overlayForm.ShowOverlayWithModal(modal);
                }


                if (dataTable.Columns[e.ColumnIndex].Name == "Column11")
                {
                    string error = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                    if (!(error != null && error.Length < 0))
                    {
                        ErrorImportacion modal = new ErrorImportacion(error);
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

        private void btnImportar_Click(object sender, EventArgs e)
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
            await Task.Delay(500);
            LoadData();
            mainForm.HideOverlay();
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            string dateInicio = lblDateInicio.Text;
            string dateFin = lblDateFin.Text;

            if (string.IsNullOrEmpty(dateInicio) || string.IsNullOrEmpty(dateFin))
            {
                MessageBox.Show("Por favor seleccione una fecha de inicio y una fecha de fin.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataTable.Rows.Clear();
            currentPage = 1;

            var data = DataStaticDto.data;

            if (data == null || data.Count == 0)
            {
                MessageBox.Show("No se encontraron datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            totalRows = data.Count;

            foreach (var compra in data)
            {
                bool withinDateRange = true;

                if (!string.IsNullOrEmpty(dateInicio) && !string.IsNullOrEmpty(dateFin))
                {
                    DateTime fechaInicioParsed;
                    DateTime fechaFinParsed;

                    if (DateTime.TryParse(dateInicio, out fechaInicioParsed) && DateTime.TryParse(dateFin, out fechaFinParsed))
                    {
                        withinDateRange = compra.FechaEmision >= fechaInicioParsed && compra.FechaEmision <= fechaFinParsed;
                    }
                }

                if (withinDateRange)
                {
                    dataTable.Rows.Add(
                        compra.NumCompra,
                        compra.FechaEmision.ToString("dd/MM/yyyy"),
                        compra.Sucursal,
                        compra.RazonSocial,
                        compra.Observacion,
                        compra.TotalPagar - compra.TotalIGV,
                        compra.TotalIGV,
                        compra.TotalPagar,
                        compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                        compra.RazonSocial
                    );
                }
            }

            UpdatePagination();
        }
    }
}


    