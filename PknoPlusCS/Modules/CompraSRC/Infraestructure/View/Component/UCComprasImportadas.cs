
using PknoPlusCS.Forms.DialogView.DialogModal;
using PknoPlusCS.Forms.Overlay;
using System;
using System.Drawing;
using System.Windows.Forms;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Static;
using System.Collections.Generic;
using System.Linq;
using PknoPlusCS.Forms.DialogView;
using System.Threading.Tasks;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using ExpressMapper.Extensions;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion;
using System.Windows.Media.Media3D;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;

namespace PknoPlusCS.Forms
{
    public partial class UCComprasImportadas : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 15;
        private int totalRows;
        private readonly ICompraSrcImportadosInputPort _compraSrc;
        private readonly ICompraSrcRepository compraSrcRepository;
        private List<SucursalDto> sucursales;
        private ValidarCierreDto validarAreaCerrado;

        public UCComprasImportadas()
        {
            InitializeComponent();
            this.VisibleChanged += UCComprasImportadas_VisibleChanged;
            dataTable.CellClick += dataTable_CellClick;
            dataTable.CellFormatting += dataTable_CellFormatting;

            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataTable.ColumnHeadersHeight = 45;
            this.sucursales = new List<SucursalDto>();
            compraSrcRepository = new CompraSrcRepository();

            _compraSrc = new CompraSrcImportadosAdapter();
            compraSrcRepository.UpdateConfiguracionInicial(2);
            LoadData();
        }

        //----------------------------------------------------------------------------------- lOAD DATA
        private async void LoadData()
        {
            ControlStatic.actualizarData = false;
            label5.Text = "Resultados compras sucursal: Todos";
            try
            {
                var data = await _compraSrc.ListarImportados(3);

                data = data.OrderByDescending(x => x.FechaImportacion).Take(10).ToList();
                await LoadSucursalData();

                if (data == null || data.Count == 0)
                {
                    pictureNone.Visible = true;
                    dataTable.Rows.Clear();
                    return;
                }


                pictureNone.Visible = false;
                dataTable.Rows.Clear();
                int cont = 0;
                foreach (var compra in data)
                {
            

                    validarAreaCerrado = _compraSrc.validarCierreArea(compra.FechaImportacion, int.Parse(compra.Sucursal));
                    var mensajeee = compra.Actualizar ?  "Actualizado": "Ok";
                    dataTable.Rows.Add(
                        cont+1,
                        compra.SerieCompra + "-" + compra.NumCompra, 
                        compra.RucPersona,
                        compra.NomPersona,
                       compra.FechaImportacion,
                        compra.SubTotal,
                        compra.Igv,
                        compra.Total,
                        compra.Estado == 3 ? "Migrado" : "Procesando",
                        compra.Actualizar == true ? "Cerrado" : "Editar"
                    );
                    if (compra.Actualizar || validarAreaCerrado.situacion != true)
                    {
                        DataGridViewCell cell = dataTable.Rows[cont].Cells["Column10"];
                        cell.ReadOnly = true;
                        cell.Style.ForeColor = Color.Gray;
                        cell.Style.BackColor = Color.LightGray;
                      
                    }
                    cont++;
                }

                totalRows = data.Count;
                UpdatePagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //----------------------------------------------------------------------------------- TABLE FORMTATING
        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataTable.Columns[e.ColumnIndex].Name == "Column10")
            {
                DataGridViewCell cell = dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell.ReadOnly)
                {
                    e.CellStyle.ForeColor = Color.Chocolate;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.BackColor = Color.LightGray;
                }

            }
        }

        //----------------------------------------------------------------------------------- TABLE CLICK
        private async void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;
                OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);

                string valorColumna9 = dataTable.Rows[e.RowIndex].Cells[9].Value?.ToString();
               
                if (!DataPermisoStaticDto.EditarMigracion)
                {
                    MessageBox.Show("No tienes permisos para editar compras", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
                    return;
                }
                if (valorColumna9 == "Cerrado")
                {
                    MessageBox.Show("Esta compra ya ha sido actualizada y no puede actualizarse nuevamente, ya que solo se permite una actualización", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; 
                }
                
                if (columnName == "Column10")
                {
                    ControlStatic.CierreDIalogvIew = false;
                    int rowIndex = e.RowIndex;
                    string code = dataTable.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string ruc = dataTable.Rows[e.RowIndex].Cells[2].Value.ToString();

                    if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(ruc))
                    {
                        var idRecepcion = await _compraSrc.GetIdRecepcion(code, ruc);
                        var data = await _compraSrc.GetAllByIdRecepcion(idRecepcion);

                        validarAreaCerrado = _compraSrc.validarCierreArea(data.FechaImportacion, int.Parse(data.Sucursal));
                        if(validarAreaCerrado.situacion != true)
                        {
                            MessageBox.Show("No se puede editar la compra porque el área está cerrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        ExtraStatic.idRecepcion = idRecepcion;
                        var modal = new DialogModal("¡Importante!", "Al editar esta compra, se eliminará la importación registrada en el ERP y se creara un nuevo registro de importación.", "warning", code, idRecepcion, ruc);
                        overlayForm.ShowOverlayWithModal(modal);
                    }

                }
            }
        }

        //----------------------------------------------------------------------------------- TABLE BUTTONS
        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            label3.Text = currentPage.ToString();
            iconButton4.Enabled = currentPage > 1;
            iconButton1.Enabled = currentPage < totalPages;
        }

        //----------------------------------------------------------------------------------- FILTER OPTIONS
        private void cbMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionesGlobalesDto.MesSeleccionado = cbMes.SelectedItem.ToString();

        }


        private void cbAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionesGlobalesDto.AñoSeleccionado = cbAño.SelectedItem.ToString();
 
        }
        private int ObtenerNumeroMes(string mes)
        {
            Dictionary<string, int> meses = new Dictionary<string, int>
    {
        { "Enero", 1 }, { "Febrero", 2 }, { "Marzo", 3 }, { "Abril", 4 },
        { "Mayo", 5 }, { "Junio", 6 }, { "Julio", 7 }, { "Agosto", 8 },
        { "Septiembre", 9 }, { "Octubre", 10 }, { "Noviembre", 11 }, { "Diciembre", 12 }
    };

            return meses.ContainsKey(mes) ? meses[mes] : 0;
        }
        private async Task LoadSucursalData()
        {
            try
            {
                //sucursales = await GetSimulatedSucursales();
                 sucursales = _compraSrc.GetAllSucursales();

                var sucursalesUnicas = sucursales
                    .GroupBy(s => s.NomPuntoVenta)
                    .Select(g => g.First())
                    .ToList();

                cbSucursal.DataSource = sucursalesUnicas;
                cbSucursal.DisplayMember = "NomPuntoVenta";
                cbSucursal.ValueMember = "IdPuntoVenta";
                var sucursalesSeleccionadas = sucursalesUnicas
                    .Where(s => s.SucursalSRC == "True" && sucursales.Any(a => a.IdPuntoVenta == s.IdPuntoVenta && a.AlmacenSrc == "True"))
                    .ToList();

                if (sucursalesSeleccionadas.Any())
                {
                    var sucursalSeleccionada = sucursalesSeleccionadas.First();
                    cbSucursal.SelectedValue = sucursalSeleccionada.IdPuntoVenta;

                    label5.Text = "Resultados compras sucursal: " + sucursalSeleccionada.NomPuntoVenta;

                }
                else
                {
                    MessageBox.Show("No se encontró ninguna sucursal con SucursalSRC == 'True' y AlmacenSrc == 'True'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las sucursales: {ex.Message}");
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var mainForm = (MainComprasSrc)this.FindForm();
            dataTable.Rows.Clear();
            currentPage = 1;

            string sucursalSeleccionada = cbSucursal.Text;
            label5.Text = "Resultados compras sucursal: "+ sucursalSeleccionada;
            string mesTexto = cbMes.SelectedItem?.ToString();
            string añoTexto = cbAño.SelectedItem?.ToString();

            int? mesNumero = null;
            int? año = null;

            // Validar el mes si está seleccionado
            if (!string.IsNullOrEmpty(mesTexto))
            {
                mesNumero = ObtenerNumeroMes(mesTexto);
                if (mesNumero == 0)
                {
                    MessageBox.Show("Seleccione un mes válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Validar el año si está seleccionado
            if (!string.IsNullOrEmpty(añoTexto) && int.TryParse(añoTexto, out int parsedAño))
            {
                año = parsedAño;
            }

            // Obtener código de sucursal si se ha seleccionado alguna
            if (!string.IsNullOrEmpty(sucursalSeleccionada) && sucursalSeleccionada != "Todos")
            {
                sucursalSeleccionada = _compraSrc.GetCodigoSucursal(sucursalSeleccionada);
            }
            else
            {
                sucursalSeleccionada = null; // No aplicar filtro si es "Todos"
            }

            pictureNone.Visible = false;
            mainForm.ShowOverlay();

            try
            {
                var data = await _compraSrc.ListarImportados(3);

                // Aplicar los filtros dinámicamente
                var datosFiltrados = data.Where(c =>
                    (string.IsNullOrEmpty(sucursalSeleccionada) || c.Sucursal == sucursalSeleccionada) &&
                    (!año.HasValue || c.FechaPeriodo.Year == año.Value) &&
                    (!mesNumero.HasValue || c.FechaPeriodo.Month == mesNumero.Value)
                ).ToList();

                dataTable.Rows.Clear();

                if (datosFiltrados.Count == 0)
                {
                    pictureNone.Visible = true;
                }
                else
                {
                    pictureNone.Visible = false;
                    int cont = 0;
                    foreach (var compra in datosFiltrados)
                    {
                        dataTable.Rows.Add(
                            cont + 1,
                            compra.SerieCompra + "-" + compra.NumCompra,
                            compra.RucPersona,
                            compra.NomPersona,
                            compra.FechaImportacion,
                            compra.SubTotal,
                            compra.Igv,
                            compra.Total,
                            compra.Estado == 3 ? "Migrado" : "Procesando",
                            compra.Actualizar ? "Cerrado" : "Editar"
                        );
                        if (compra.Actualizar)
                        {
                            DataGridViewCell cell = dataTable.Rows[cont].Cells["Column10"];
                            cell.ReadOnly = true;
                            cell.Style.ForeColor = Color.Gray;
                            cell.Style.BackColor = Color.LightGray;
                        }
                        cont++;
                    }
                }

                totalRows = datosFiltrados.Count;
                UpdatePagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mainForm.HideOverlay();
        }




        //----------------------------------------------------------------------------------- BUTTON LIMPIAR
        private async void btnLimpiar_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        //----------------------------------------------------------------------------------- MAGIA
        private void UCComprasImportadas_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && ControlStatic.actualizarData)
            {
                LoadData();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
