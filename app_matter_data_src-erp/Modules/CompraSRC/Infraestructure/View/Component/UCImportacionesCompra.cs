using app_matter_data_src_erp.Forms.DialogView;
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
        private List<CompraDto> compraData;

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

        //----------------------------------------------------------------------------------- lOAD DATA
        private async void UCImportacionesCompra_Load(object sender, EventArgs e)
        {
            await InitializeData();
        }

        private async Task InitializeData()
        {
            var mainForm = (MainComprasSrc)this.FindForm();
            if (mainForm == null) return;

            mainForm.ShowOverlay();
            try
            {
                if (DataStaticDto.data == null || DataStaticDto.data.Count == 0)
                {
                    DataStaticDto.data = await _compraSrc.ObtenerDataSrc();
                }

                compraData = DataStaticDto.data;
                totalRows = compraData.Count;

                LoadPageData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los datos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mainForm.HideOverlay();
            }
        }

        private void LoadPageData()
        {
            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataTable.Rows.Clear();

            if (compraData == null || compraData.Count == 0)
            {
                MessageBox.Show("No se encontraron datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                var compra = compraData[i];
                dataTable.Rows.Add(
                    compra.idCompraSerie,
                    compra.FechaEmision.ToString("dd/MM/yyyy"),
                    compra.Sucursal,
                    compra.RazonSocial,
                    compra.DocumentoProveedor,
                    compra.RazonSocial,
                    "s/." + (compra.TotalPagar - compra.TotalIGV),
                    compra.TotalIGV,
                    "s/." + (compra.TotalPagar),
                    compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                    compra.RazonSocial,
                    compra.Errores
                );
            }
            UpdatePagination();
        }

        //----------------------------------------------------------------------------------- TABLE BUTTONS
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
                LoadPageData();
            }
        }

        private void nextPageButton_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPageData();
            }
        }

        //----------------------------------------------------------------------------------- TABLE FORMAT
        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || DataStaticDto.data == null || DataStaticDto.data.Count == 0)
            {
                return;
            }

            int realIndex = (currentPage - 1) * rowsPerPage + e.RowIndex;
            if (realIndex >= DataStaticDto.data.Count) return; 

            var dataItem = DataStaticDto.data[realIndex]; 
            var columnName = dataTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Column1")
            {
                e.CellStyle.ForeColor = Color.Green;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }
            else if (columnName == "Column3" || columnName == "Column4" || columnName == "Column10" || columnName == "Column11")
            {
                e.Value = "Pendiente";
                e.CellStyle.ForeColor = Color.Chocolate;
                e.CellStyle.SelectionForeColor = Color.Chocolate;

                if (columnName == "Column11" && dataItem.Coicidencia != null)
                {
                    e.Value = dataItem.Coicidencia;
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.SelectionForeColor = Color.Black;
                }
                else if (columnName == "Column10" && dataItem.FechaLlegada != null)
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
            else if (columnName == "Column12")
            {
                var cell = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (dataItem.Estado == "En proceso")
                {
                    e.Value = "En proceso";
                    e.CellStyle.ForeColor = Color.Goldenrod;
                    e.CellStyle.SelectionForeColor = Color.Goldenrod;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                    return;
                }

                bool allColumnsHaveData =
                    dataItem.NomPlantilla != null &&
                    dataItem.NewSucursal != null &&
                    dataItem.FechaLlegada != null &&
                    dataItem.Coicidencia != null;

                if (allColumnsHaveData)
                {
                    DataStaticDto.data[realIndex].Estado = "Listo";
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
                            DataStaticDto.data[realIndex].Estado = "Error";
                            e.Value = "Error";
                            e.CellStyle.ForeColor = Color.Red;
                            e.CellStyle.SelectionForeColor = Color.Red;
                            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                        }
                        else if (cellValue.StartsWith("No listo"))
                        {
                            DataStaticDto.data[realIndex].Estado = "No listo";
                            e.Value = "No listo";
                            e.CellStyle.ForeColor = Color.Chocolate;
                            e.CellStyle.SelectionForeColor = Color.Chocolate;
                            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                        }
                    }
                }
            }
        }


        //----------------------------------------------------------------------------------- TABLE CLICK
        private async void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {

                int realIndex = (currentPage - 1) * rowsPerPage + e.RowIndex;
                var columnName = dataTable.Columns[e.ColumnIndex].Name;
                OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);

                if (columnName == "Column1")
                {
                    var codigoProveedor = dataTable.Rows[e.RowIndex].Cells["Column5"].Value?.ToString();
                    var codigo = dataTable.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();
                    var empresa = dataTable.Rows[e.RowIndex].Cells["Column6"].Value?.ToString();

                    if (!string.IsNullOrEmpty(codigoProveedor) && !string.IsNullOrEmpty(codigo))
                    {
                        var idRecepcion = await _compraSrc.GetIdRecepcion(codigoProveedor, codigo);
                        if (!string.IsNullOrEmpty(idRecepcion))
                        {
                            CompraDto compra = await _compraSrc.ObtenerCompraPorIdRecepcion(idRecepcion);
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

                                ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible(codigo, empresa, tablaDatosCombustible);
                                overlayForm.ShowOverlayWithModal(modal);
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron las compras!.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("NO se encontró el id de recepción!.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El código del proveedor y de compra no es válido!.");
                    }
                }

                if (columnName == "Column3")
                {
                    string direccion = DataStaticDto.data[realIndex].Sucursal;
                    Sucursal modal = new Sucursal((MainComprasSrc)this.ParentForm, direccion, realIndex);
                    overlayForm.ShowOverlayWithModal(modal);
                }

                if (columnName == "Column4")
                {
                    AsientoTipo modal = new AsientoTipo((MainComprasSrc)this.ParentForm, realIndex);
                    overlayForm.ShowOverlayWithModal(modal);
                }

                if (columnName == "Column10")
                {
                    DateTimePicker dateTimePicker = new DateTimePicker { Format = DateTimePickerFormat.Short };
                    var cellValue = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    dateTimePicker.Value = DateTime.TryParse(cellValue, out DateTime parsedDate) ? parsedDate : DateTime.Now;

                    Rectangle rect = dataTable.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dateTimePicker.Size = new Size(rect.Width, rect.Height);
                    dateTimePicker.Location = new Point(rect.Left, rect.Top + 6);
                    dataTable.Controls.Add(dateTimePicker);

                    dateTimePicker.ValueChanged += (s, args) =>
                    {
                        dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dateTimePicker.Value.ToString("dd/MM/yyyy");
                        DataStaticDto.data[realIndex].FechaLlegada = dateTimePicker.Value.ToString("dd/MM/yyyy");
                        dataTable.Controls.Remove(dateTimePicker);
                    };

                    dateTimePicker.Leave += (s, args) => dataTable.Controls.Remove(dateTimePicker);
                    dateTimePicker.Focus();
                }

                if (columnName == "Column11")
                {
                    string codigoProveedor = dataTable.Rows[e.RowIndex].Cells[4].Value.ToString();
                    string codigo = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(codigoProveedor) && !string.IsNullOrEmpty(codigo))
                    {
                        var idRecepcion = await _compraSrc.GetIdRecepcion(codigoProveedor, codigo);
                        if (!string.IsNullOrEmpty(idRecepcion))
                        {
                            CompraDto compra = await _compraSrc.ObtenerCompraPorIdRecepcion(idRecepcion);
                            if (compra != null)
                            {
                                CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm, compra.idCompraSerie, compra.Compras, DataStaticDto.data[realIndex].DocumentoProveedor, realIndex);
                                overlayForm.ShowOverlayWithModal(modal);
                            }
                            else
                            {
                                MessageBox.Show("No tiene compras!.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("NO se encontró el id de recepción!.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El código del proveedor y de compra no es válido!.");
                    }
                }

                if (columnName == "Column12")
                {
                    string codigoCompra = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string error = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    if (error.StartsWith("Error"))
                    {
                        ErrorImportacion modal = new ErrorImportacion(error, codigoCompra);
                        overlayForm.ShowOverlayWithModal(modal);
                    }
                }
            }
        }

        private void dataTable_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                string estado = DataStaticDto.data[e.RowIndex].Estado;

                if (estado == "No listo" || estado == "Error")
                {
                    dataTable.ClearSelection(); 

                }
            }
        }

        //----------------------------------------------------------------------------------- BUTTON IMPORT
        private void btnImportar_Click(object sender, EventArgs e)
        {
            OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);

            List<Tuple<string, string>> codigosYDocumentos = new List<Tuple<string, string>>(); 

            foreach (DataGridViewRow selectedRow in dataTable.SelectedRows)
            {
                string estado = DataStaticDto.data[selectedRow.Index].Estado;

                if (estado == "No listo" || estado == "Error")
                {
                    MessageBox.Show($"No se puede seleccionar la fila con código de compra {selectedRow.Cells["Column1"].Value} porque está en estado '{estado}'.");
                    return;
                }

                var codigoCompra = selectedRow.Cells["Column1"].Value?.ToString();
                var documentoProveedor = selectedRow.Cells["Column5"].Value?.ToString();

                if (!string.IsNullOrEmpty(codigoCompra) && !string.IsNullOrEmpty(documentoProveedor))
                {
                    codigosYDocumentos.Add(new Tuple<string, string>(codigoCompra, documentoProveedor)); 
                }
            }

            if (codigosYDocumentos.Count > 0)
            {
                var modal = new Importar((MainComprasSrc)this.ParentForm, codigosYDocumentos); 
                overlayForm.ShowOverlayWithModal(modal);
            }
            else
            {
                MessageBox.Show("No se encontraron códigos de compra o documentos del proveedor en las filas seleccionadas.");
            }
        }

        //----------------------------------------------------------------------------------- BUTTON SCANNER
        private async void btnEscanear_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Si procedes con la ejecución, la tabla se refrescará y cualquier cambio hecho se perderá. ¿Deseas continuar?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
            {
                return; 
            }

            btnEscanear.Enabled = false;
            var mainForm = (MainComprasSrc)this.FindForm();
            mainForm.ShowOverlay();
            await Task.Delay(500);

            try
            {
                var nuevosDatos = await _compraSrc.ObtenerDataSrc();

                if (nuevosDatos == null || nuevosDatos.Count == 0)
                {
                    MessageBox.Show("No se encontraron nuevos datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int datosActuales = DataStaticDto.data.Count;
                int nuevosRegistros = nuevosDatos.Count;

                if (nuevosRegistros > datosActuales)
                {
                    var datosParaAgregar = nuevosDatos.Skip(datosActuales).ToList();
                    DataStaticDto.data.AddRange(datosParaAgregar);
                    compraData.AddRange(datosParaAgregar);
                    totalRows = compraData.Count;

                    foreach (var compra in datosParaAgregar)
                    {
                        dataTable.Rows.Add(
                        compra.idCompraSerie,
                        compra.FechaEmision.ToString("dd/MM/yyyy"),
                        compra.Sucursal,
                        compra.RazonSocial,
                        compra.DocumentoProveedor,
                        compra.RazonSocial,
                        "s/." + (compra.TotalPagar - compra.TotalIGV),
                        compra.TotalIGV,
                        "s/." + (compra.TotalPagar),
                        compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                        compra.RazonSocial,
                        compra.Errores
                        );
                    }

                    UpdatePagination();
                }
                else
                {
                    MessageBox.Show("No hay nuevos registros para agregar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al escanear:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mainForm.HideOverlay();
                btnEscanear.Enabled = true;
            }
        }


        //----------------------------------------------------------------------------------- FILTER DATA
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
                        compra.idCompraSerie,
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
