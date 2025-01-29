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

        }

        private async void LoadData()
        {
            var mainForm = (MainComprasSrc)this.FindForm();
            mainForm.ShowOverlay();
            try
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
                            compra.RazonSocial,
                            "s/."+(compra.TotalPagar - compra.TotalIGV),
                            compra.TotalIGV,
                             "s/." + (compra.TotalPagar),
                            compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                            compra.RazonSocial,
                            compra.Errores
                        );
                    }

                UpdatePagination();
            }
            finally
            {
                mainForm.HideOverlay();
            }

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
            var columnName = dataTable.Columns[e.ColumnIndex].Name;



            if (columnName == "Column3" || columnName == "Column4" || columnName == "Column9" || columnName == "Column10")
            {
                e.Value = "Pendiente";
                e.CellStyle.ForeColor = Color.Chocolate;
                e.CellStyle.SelectionForeColor = Color.Chocolate;

                if (e.RowIndex >= 0 && e.RowIndex < DataStaticDto.data.Count)
                {
                    var dataItem = DataStaticDto.data[e.RowIndex];

                    if (columnName == "Column10" && dataItem.Coicidencia != null)
                    {
                        e.Value = dataItem.Coicidencia;
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Black;

                    }
                    if (columnName == "Column9" && dataItem.FechaLlegada != null)
                    {
                        e.Value = dataItem.FechaLlegada;
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Black;
                    }
                    else if (columnName == "Column3" && dataItem.NewSucursal != null)
                    {
                        e.Value = dataItem.NewSucursal;
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Black;
                    }
                    else if (columnName == "Column4" && dataItem.IdPlantilla != null)
                    {
                        e.Value = dataItem.NomPlantilla;
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Black;
                    }
                }
            }
            else if (columnName == "Column1")
            {
                e.CellStyle.ForeColor = Color.Green;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }
            else if (columnName == "Column11")
            {
                var cell = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var dataItem = DataStaticDto.data[e.RowIndex];
                bool allColumnsHaveData =
                dataItem.NomPlantilla != null &&
                dataItem.NewSucursal != null &&
                dataItem.FechaLlegada != null &&
                dataItem.Coicidencia != null;

                if (allColumnsHaveData)
                {
                    DataStaticDto.data[e.RowIndex].Estado = "Listo";
                    e.Value = "Listo";
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.SelectionForeColor = Color.Green;
                }
                else
                {
                    if (cell.Value != null)
                    {
                        string cellValue = cell.Value.ToString();
                        if (cellValue.StartsWith("Error"))
                        {

                            DataStaticDto.data[e.RowIndex].Estado = "Error";
                            e.Value = "Error";
                            e.CellStyle.ForeColor = Color.Red;
                            e.CellStyle.SelectionForeColor = Color.Red;
                            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                        }
                        else if (cellValue.StartsWith("No listo"))
                        {
                            DataStaticDto.data[e.RowIndex].Estado = "No listo";
                            e.Value = "No listo";
                            e.CellStyle.ForeColor = Color.Chocolate;
                            e.CellStyle.SelectionForeColor = Color.Chocolate;
                            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                        }
                    }
                }


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
                    var empresa = dataTable.Rows[e.RowIndex].Cells["Column5"].Value?.ToString();

                    if (!string.IsNullOrEmpty(codigoCompra))
                    {
                        CompraDto compra = await _compraSrc.ObtenerCompraPorCodigo(codigoCompra);

                        if (compra != null)
                        {
                            List<CompraDetalleDto> detallesCompra = compra.Compras;

                            List<List<object>> tablaDatosCombustible = detallesCompra.Select(detalle => new List<object>
                            {
                                detalle.Descripcion,
                                detalle.Cantidad,
                                detalle.PrecioUnitarioSinIgv,
                                detalle.PrecioUnitarioConIgv,
                                detalle.Api,
                                detalle.Temp,
                                detalle.SubTotalSinIgv,
                                detalle.SubTotalConIgv

                            }).ToList();

                            List<List<object>> tablaDatos = detallesCompra.Select(detalle => new List<object>
                            {
                                detalle.Codigo,
                                detalle.Descripcion,
                                detalle.Cantidad,
                                detalle.Cantidad,
                                detalle.PrecioUnitarioConIgv,
                                detalle.Cantidad,
                                detalle.PrecioUnitarioConIgv,
                                detalle.Total,
                            }).ToList();

                            bool validacion = detallesCompra.All(detalle => detalle.Api == "" || detalle.Temp == "");

                            if (validacion)
                            {
                                //ModalDetalleCompra modal = new ModalDetalleCompra(codigoCompra, tablaDatos);
                                ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible(codigoCompra, empresa, tablaDatosCombustible);
                                overlayForm.ShowOverlayWithModal(modal);
                            }
                            else
                            {
                                ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible(codigoCompra, empresa, tablaDatosCombustible);
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
                    AsientoTipo modal = new AsientoTipo((MainComprasSrc)this.ParentForm, dataTable.Rows[e.RowIndex].Index);
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
                    int rowIndex = e.RowIndex;
                    string codigoCompra = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    CompraDto compra = await _compraSrc.ObtenerCompraPorCodigo(codigoCompra);

                    CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm, compra.NumCompra, compra.Compras, DataStaticDto.data[rowIndex].DocumentoProveedor, rowIndex);
                    overlayForm.ShowOverlayWithModal(modal);

                }

                if (dataTable.Columns[e.ColumnIndex].Name == "Column11")
                {
                    string codigoCompra = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string error = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    string cellValue = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (cellValue.StartsWith("Error"))
                    {
                        ErrorImportacion modal = new ErrorImportacion(error, codigoCompra);
                        overlayForm.ShowOverlayWithModal(modal);
                    }
                }
            }
        }
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // Evitar encabezados
            {
                string estado = DataStaticDto.data[e.RowIndex].Estado;

                if (estado == "No listo" || estado == "Error")
                {
                    dataTable.ClearSelection(); // Desseleccionar fila

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

            List<string> codigosCompras = new List<string>();

            foreach (DataGridViewRow selectedRow in dataTable.SelectedRows)
            {
                string estado = DataStaticDto.data[selectedRow.Index].Estado;

                if (estado == "No listo" || estado == "Error")
                {
                    MessageBox.Show($"No se puede seleccionar la fila con código de compra {selectedRow.Cells["Column1"].Value} porque está en estado '{estado}'.");
                    return;
                }
                var codigoCompra = selectedRow.Cells["Column1"].Value?.ToString();
                if (!string.IsNullOrEmpty(codigoCompra))
                {
                    codigosCompras.Add(codigoCompra);
                }
            }

            if (codigosCompras.Count > 0)
            {
                var modal = new Importar((MainComprasSrc)this.ParentForm, codigosCompras[0]);
                overlayForm.ShowOverlayWithModal(modal);

                string mensaje = "Códigos de compra seleccionados:\n" + string.Join("\n", codigosCompras);
                MessageBox.Show(mensaje);
            }
            else
            {
                MessageBox.Show("No se encontraron códigos de compra en las filas seleccionadas.");
            }
        }



        private async void btnEscanear_Click(object sender, EventArgs e)
        {
            btnEscanear.Enabled = false;
            dataTable.Rows.Clear();
            currentPage = 1;
            var mainForm = (MainComprasSrc)this.FindForm();
            mainForm.ShowOverlay();
            await Task.Delay(500);
            LoadData();
            mainForm.HideOverlay();
            btnEscanear.Enabled = true;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string dateInicio = lblDateInicio.Text;
            string dateFin = lblDateFin.Text;
            string selectC = cbEstadoImportacion.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(dateInicio) && string.IsNullOrEmpty(dateFin) && string.IsNullOrEmpty(selectC))
            {
                MessageBox.Show("Por favor seleccione una fecha de inicio, una fecha de fin o un estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                bool matchesEstado = true;

                if (!string.IsNullOrEmpty(dateInicio) && !string.IsNullOrEmpty(dateFin))
                {
                    DateTime fechaInicioParsed;
                    DateTime fechaFinParsed;

                    if (DateTime.TryParse(dateInicio, out fechaInicioParsed) && DateTime.TryParse(dateFin, out fechaFinParsed))
                    {
                        withinDateRange = compra.FechaEmision >= fechaInicioParsed && compra.FechaEmision <= fechaFinParsed;
                    }
                }

                if (!string.IsNullOrEmpty(selectC))
                {
                    matchesEstado = compra.Estado == selectC;
                }

                if (withinDateRange && matchesEstado)
                {
                    dataTable.Rows.Add(
                        compra.NumCompra,
                        compra.FechaEmision.ToString("dd/MM/yyyy"),
                        compra.Sucursal,
                        compra.RazonSocial,
                        compra.RazonSocial,
                        "s/." + (compra.TotalPagar - compra.TotalIGV),
                        compra.TotalIGV,
                        "s/." + (compra.TotalPagar),
                        compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                        compra.RazonSocial,
                        compra.Errores
                    );
                }
            }

            UpdatePagination();
        }

    }
}