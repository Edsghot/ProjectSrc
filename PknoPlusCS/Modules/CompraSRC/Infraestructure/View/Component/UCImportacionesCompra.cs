using PknoPlusCS.Forms.DialogView;
using PknoPlusCS.Forms.Overlay;
using PknoPlusCS.Global.Helper;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Constantes;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.View.Modales;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PknoPlusCS.Forms
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

            if (!DataPermisoStaticDto.MigrarCompras)
            {
                btnImportar.BackColor = Color.LightGray;
            }
            if (!DataPermisoStaticDto.Escanear) { 
                btnEscanear.BackColor = Color.LightGray;
            }
            btnImportar.Enabled = DataPermisoStaticDto.MigrarCompras;
            btnEscanear.Enabled = DataPermisoStaticDto.Escanear;

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
        private void AgregarColumnaCheckBox()
        {
            if (dataTable.Columns["Column13"] is DataGridViewCheckBoxColumn)
                return;

            int colIndex = dataTable.Columns["Column13"].Index;
            dataTable.Columns.Remove("Column13");

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "✓",
                Name = "Column13",
                Width = 50,
                TrueValue = true,
                FalseValue = false
            };

            dataTable.Columns.Insert(colIndex, checkBoxColumn);
        }


        //----------------------------------------------------------------------------------- lOAD DATA
        private async void UCImportacionesCompra_Load(object sender, EventArgs e)
        {
            await InitializeData();
            AgregarColumnaCheckBox();
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
                MessageBox.Show($"Ocurrió un error al cargar los datos:\n{ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No se encontraron datos.", @"Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
            {
                var compra = compraData[i];
                if(compra.Estado == StatusConstant.Migrado)
                {
                    continue;
                }
                var simbolo = compra.Moneda == "N" ? "S/. " : "$. ";

                var subTotal = compra.TotalGravadas == 0 ? compra.TotalExoneradas : compra.TotalGravadas;


                dataTable.Rows.Add(
                    compra.Seleccionado,
                    compra.idCompraSerie,
                    compra.FechaEmision.ToString("dd/MM/yyyy"),
                    compra.Sucursal,
                    compra.RazonSocial,
                    compra.DocumentoProveedor,
                    compra.RazonSocial,
                    simbolo + subTotal,
                    compra.TotalIGV,
                    simbolo + (compra.TotalPagar),
                    compra.FechaVencimiento.ToString("dd/MM/yyyy"),
                    compra.RazonSocial,
                    compra.Estado
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

            var dataCompra = compraData.Where(x => x.Estado != StatusConstant.Migrado).ToList();
            if(dataCompra.Count == 0)
            {
                return;
            }
            var dataItem = dataCompra[realIndex];
            var columnName = dataTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Column1")
            {
                e.CellStyle.ForeColor = Color.Green;
                e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);
            }

            if (columnName == "Column3" || columnName == "Column4" || columnName == "Column10" || columnName == "Column11")
            {
                e.Value = StatusConstant.Pendiente;
                e.CellStyle.ForeColor = Color.Chocolate;
                e.CellStyle.SelectionForeColor = Color.Chocolate;
                        
               
                if (columnName == "Column11" && dataItem.EstadoProductos != null)
                {
                    if (dataItem.EstadoProductos == true)
                    {
                        e.Value = "Revisado";
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.SelectionForeColor = Color.Black;
                    }
                    else
                    {
                        e.Value = "Pendiente";
                        e.CellStyle.ForeColor = Color.Chocolate;
                        e.CellStyle.SelectionForeColor = Color.Black;
                    }
                   
                }
                else if (columnName == "Column10")
                {
                    e.Value = dataItem.FechaLlegada;
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.SelectionForeColor = Color.Black;
                }
                else if (columnName == "Column3" && dataItem.Sucursal != null)
                {
                    e.Value = dataItem.Sucursal;
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

                if (dataItem.Estado == StatusConstant.Error)
                {
                    e.Value = dataItem.Estado;
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.SelectionForeColor = Color.Red;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);
                    return;
                }
                if (dataItem.Estado == StatusConstant.NoListo)
                {
                    e.Value = dataItem.Estado;
                    e.CellStyle.ForeColor = Color.Chocolate;
                    e.CellStyle.SelectionForeColor = Color.Chocolate;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                    return;
                }
                if (dataItem.Estado == StatusConstant.Listo)
                {
                    e.Value = dataItem.Estado;
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.SelectionForeColor = Color.Green;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                    return;
                }
                if (dataItem.Estado == StatusConstant.Migrado)
                {
                    e.Value = dataItem.Estado;
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.SelectionForeColor = Color.Blue;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                    return;
                }
                if (dataItem.Estado == StatusConstant.Pendiente)
                {
                    e.Value = dataItem.Estado;
                    e.CellStyle.ForeColor = Color.Orange;
                    e.CellStyle.SelectionForeColor = Color.Orange;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold);
                    return;
                }
                if (dataItem.Estado == StatusConstant.EnProceso)
                {
                    e.Value = dataItem.Estado;
                    e.CellStyle.ForeColor = Color.Goldenrod;
                    e.CellStyle.SelectionForeColor = Color.Goldenrod;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);
                    return;
                }

            }
        }

        //----------------------------------------------------------------------------------- TABLE CLICK
        private async void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (!DataPermisoStaticDto.EditarDetalle)
            {
                MessageBox.Show(@"No tiene permisos para editar los detalles de la compra");
                return;
            }
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {

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
                            CompraDto compra =  _compraSrc.ObtenerCompraPorIdRecepcion(idRecepcion);
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
                                ///------------------------------------------------------------------------
                                ExtraStatic.idRecepcionScop = idRecepcion;
                                ModalDetalleCompraCombustible modal = new ModalDetalleCompraCombustible((MainComprasSrc)this.ParentForm, codigo, empresa, tablaDatosCombustible);
                                overlayForm.ShowOverlayWithModal(modal);
                            }
                            else
                            {
                                MessageBox.Show(@"No se encontraron las compras!.");
                            }
                        }
                        else
                        {
                            MessageBox.Show(@"NO se encontró el id de recepción!.");
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"El código del proveedor y de compra no es válido!.");
                    }
                }

                var codigoProveedorG = dataTable.Rows[e.RowIndex].Cells["Column5"].Value?.ToString();
                var codigoG = dataTable.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();

                var idRecepcionG = await _compraSrc.GetIdRecepcion(codigoProveedorG, codigoG);
                var CompraEscogido = _compraSrc.ObtenerCompraPorIdRecepcion(idRecepcionG);

                    if (columnName == "Column3")
                {
                    string direccion = CompraEscogido.Sucursal;
                    Sucursal modal = new Sucursal((MainComprasSrc)this.ParentForm, direccion, idRecepcionG);
                    overlayForm.ShowOverlayWithModal(modal);
                     _compraSrc.createBackup();
                }

                if (columnName == "Column4")
                {
                    AsientoTipo modal = new AsientoTipo((MainComprasSrc)this.ParentForm, idRecepcionG);
                    overlayForm.ShowOverlayWithModal(modal);
                }

                if (columnName == "Column10")
                {
                    DateTimePicker dateTimePicker = new DateTimePicker { Format = DateTimePickerFormat.Short };
                    var cellValue = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    dateTimePicker.Value = DateTime.TryParse(cellValue, out DateTime parsedDate) ? parsedDate : DateTime.Now;

                    Rectangle rect = dataTable.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dateTimePicker.Size = new System.Drawing.Size(rect.Width, rect.Height);
                    dateTimePicker.Location = new System.Drawing.Point(rect.Left, rect.Top + 6);
                    dataTable.Controls.Add(dateTimePicker);

                    dateTimePicker.ValueChanged += (s, args) =>
                    {
                        dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dateTimePicker.Value.ToString("dd/MM/yyyy");
                        CompraEscogido.FechaLlegada = dateTimePicker.Value.ToString("dd/MM/yyyy");
                        CompraEscogido.EstadoFechaLlegada = true;
                        HFunciones.ActualizarEstados();
                        dataTable.Controls.Remove(dateTimePicker);
                    };

                    dateTimePicker.Leave += (s, args) => dataTable.Controls.Remove(dateTimePicker);
                    dateTimePicker.Focus();
                }

                if (columnName == "Column11")
                {
                    string codigoProveedor = dataTable.Rows[e.RowIndex].Cells[5].Value.ToString();
                    string codigo = dataTable.Rows[e.RowIndex].Cells[1].Value.ToString();
                    if (!string.IsNullOrEmpty(codigoProveedor) && !string.IsNullOrEmpty(codigo))
                    {
                        var idRecepcion = await _compraSrc.GetIdRecepcion(codigoProveedor, codigo);
                        if (!string.IsNullOrEmpty(idRecepcion))
                        {
                            CompraDto compra =  _compraSrc.ObtenerCompraPorIdRecepcion(idRecepcion);
                            if (compra != null)
                            {
                                CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm, compra.idCompraSerie, compra.Compras, CompraEscogido.DocumentoProveedor, CompraEscogido.IdRecepcion);
                                overlayForm.ShowOverlayWithModal(modal);
                                ActualizarTabla();
                                _compraSrc.createBackup();
                            }
                            else
                            {
                                MessageBox.Show(@"No tiene compras!.");
                            }
                        }
                        else
                        {
                            MessageBox.Show(@"NO se encontró el id de recepción!.");
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"El código del proveedor y de compra no es válido!.");
                    }
                }

                if (columnName == "Column12")
                {
                    string estado = dataTable.Rows[e.RowIndex].Cells[12].Value.ToString();
                    string codigoCompra = dataTable.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string ruc = dataTable.Rows[e.RowIndex].Cells[5].Value.ToString();
                    var idRecepcion = await _compraSrc.GetIdRecepcion(ruc, codigoCompra);

                    var errores = _compraSrc.GetErrorsDetail(idRecepcion);

                    if (estado == "Error")
                    {
                        ErrorImportacion modal = new ErrorImportacion(errores, codigoCompra);
                        overlayForm.ShowOverlayWithModal(modal);
                    }
                }

                if (columnName == "Column13")
                {
                    CompraEscogido.Seleccionado = !CompraEscogido.Seleccionado;
                    dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CompraEscogido.Seleccionado;
                    dataTable.Refresh(); 
                }

                 _compraSrc.createBackup();

            }
        }
        private void ActualizarTabla()
        {
            dataTable.Refresh();
        }


        //----------------------------------------------------------------------------------- BUTTON IMPORT
        private async void btnImportar_Click(object sender, EventArgs e)
        {
            OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);
            List<Tuple<string, string>> codigosYIdRecepcion = new List<Tuple<string, string>>();

            foreach (DataGridViewRow row in dataTable.Rows) 
            {

                bool seleccionado = row.Cells["Column13"].Value != null && (bool)row.Cells["Column13"].Value;
                if (!seleccionado)
                {
                    continue; 
                }

                int realIndex = (currentPage - 1) * rowsPerPage + row.Index;

                if (realIndex < 0 || realIndex >= DataStaticDto.data.Count)
                {
                    MessageBox.Show(@"Error al obtener el índice real de la fila seleccionada.");
                    return;
                }



                var codigoCompra = row.Cells["Column1"].Value?.ToString();

                var documentoProveedor = row.Cells["Column5"].Value?.ToString();

                var idRecepcion = await _compraSrc.GetIdRecepcion(documentoProveedor, codigoCompra);

                var dataSeleccionado =  _compraSrc.ObtenerCompraPorIdRecepcion(idRecepcion);
                var validarAreaCerrado = _compraSrc.validarCierreArea(dataSeleccionado.FechaEmision, int.Parse(dataSeleccionado.SucursalId));
                if (validarAreaCerrado.situacion != true)
                {
                    MessageBox.Show("No se puede migrar la compra porque el área está cerrado", @"Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string estado = dataSeleccionado.Estado;

                if (estado == "No Listo" || estado == "Error" || estado == "En Proceso")
                {
                    MessageBox.Show($@"No se puede seleccionar el nro de comprobante {row.Cells["Column1"].Value} porque está en estado '{estado}'.");
                    return;
                }

                if (!string.IsNullOrEmpty(codigoCompra) && !string.IsNullOrEmpty(documentoProveedor))
                {
                    codigosYIdRecepcion.Add(new Tuple<string, string>(codigoCompra, idRecepcion));
                }
            }

            if (codigosYIdRecepcion.Count > 0)
            {

                var modal = new ResumenImportacion((MainComprasSrc)this.ParentForm, codigosYIdRecepcion);
                overlayForm.ShowOverlayWithModal(modal);
            }
            else
            {
                MessageBox.Show(@"debe seleccionar algun comprobante para migrar al ERP");
            }
        }



        //----------------------------------------------------------------------------------- BUTTON SCANNER
        private async void btnEscanear_Click(object sender, EventArgs e)
        {
                var confirmResult = MessageBox.Show(
                    "Si procedes con la ejecución, la tabla se refrescará y cualquier cambio hecho se perderá. ¿Deseas continuar?",
                    @"Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.No)
                    return;

                btnEscanear.Enabled = false;

                if (!(this.FindForm() is MainComprasSrc mainForm))
                {
                    MessageBox.Show("No se pudo encontrar el formulario principal.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnEscanear.Enabled = true;
                    return;
                }

                mainForm.ShowOverlay();

                try
                {
                    // 1. Obtener datos del origen
                    var nuevosDatos = await _compraSrc.obtenerDataDelSrc();

                    // 2. Filtrar registros (excluyendo los que están en estado 'Migrado')
                    var datosFiltrados = nuevosDatos
                        .Where(c => c.Estado != StatusConstant.Migrado)
                        .ToList();

                    // 3. Actualizar la fuente de datos estática
                    DataStaticDto.data = datosFiltrados;

                    // 4. Limpiar DataGridView y establecer nueva fuente
                    dataTable.DataSource = null;
                    dataTable.Rows.Clear(); // solo por si acaso
                    compraData = datosFiltrados; // Asumiendo que `compraData` se usa como cache en paginación

                    totalRows = datosFiltrados.Count;
                    currentPage = 1;
                    LoadPageData(); // para que la paginación y formato funcionen
                    UpdatePagination();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al escanear:\n{ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string dateInicio = lblDateInicio.Text.Trim();
            string dateFin = lblDateFin.Text.Trim();
            string selectC = cbEstadoImportacion.SelectedItem?.ToString()?.Trim().ToLower();

            if (string.IsNullOrEmpty(dateInicio) && string.IsNullOrEmpty(dateFin) && string.IsNullOrEmpty(selectC))
            {
                MessageBox.Show("Por favor seleccione una fecha de inicio, una fecha de fin o un estado.", @"Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataTable.Rows.Clear();
            currentPage = 1;

            var data = DataStaticDto.data;
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("No se encontraron datos.", @"Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<CompraDto> filteredData = data.Where(compra =>
            {
                bool withinDateRange = true;
                bool matchesEstado = true;

                if (!string.IsNullOrEmpty(dateInicio) && !string.IsNullOrEmpty(dateFin))
                {
                    if (DateTime.TryParse(dateInicio, out DateTime fechaInicioParsed) &&
                        DateTime.TryParse(dateFin, out DateTime fechaFinParsed))
                    {
                        withinDateRange = compra.FechaEmision.Date >= fechaInicioParsed.Date &&
                                          compra.FechaEmision.Date <= fechaFinParsed.Date;
                    }
                }


                if (!string.IsNullOrEmpty(selectC))
                {
                    string estadoCompra = compra.Estado?.Trim().ToLower();
                    matchesEstado = estadoCompra == selectC;
                }

                return withinDateRange && matchesEstado;
            }).ToList(); 

            compraData = filteredData;

            totalRows = compraData.Count;
            LoadPageData();
        }

    }
}
