
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using System;
using System.Drawing;
using System.Windows.Forms;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using System.Collections.Generic;
using System.Linq;

namespace app_matter_data_src_erp.Forms
{
    public partial class UCComprasImportadas : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 15;
        private int totalRows;
        private readonly ICompraSrcImportadosInputPort _compraSrc;

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

            _compraSrc = new CompraSrcImportadosAdapter();
            LoadData();
        }

        //----------------------------------------------------------------------------------- lOAD DATA
        private async void LoadData()
        {
            ControlStatic.actualizarData = false;
            try
            {
                var data = await _compraSrc.ListarImportados(3);

                if (data == null || data.Count == 0)
                {
                    pictureNone.Visible = true;
                    dataTable.Rows.Clear();
                    return;
                }

                pictureNone.Visible = false;
                dataTable.Rows.Clear(); 

                foreach (var compra in data)
                {
                    dataTable.Rows.Add(
                        compra.SerieCompra + "-" + compra.NumCompra, 
                        compra.RucPersona,
                        compra.NomPersona,
                        compra.Fecha.ToString("dd/MM/yyyy"), 
                        compra.Igv,
                        compra.SubTotal,
                        compra.Total,
                        compra.Estado == 3 ? "Importado" : "Procesando",
                        "ok",
                        "Editar"
                    );
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
                e.CellStyle.ForeColor = Color.Chocolate;

                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }
        }

        //----------------------------------------------------------------------------------- TABLE CLICK
        private async void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;
                OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);

                if (columnName == "Column10")
                {
                    ControlStatic.CierreDIalogvIew = false;
                    int rowIndex = e.RowIndex;
                    string code = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string ruc = dataTable.Rows[e.RowIndex].Cells[1].Value.ToString();

                    if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(ruc))
                    {
                        var idRecepcion = await _compraSrc.GetIdRecepcion(code, ruc);
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
            LoadData();
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

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var mainForm = (MainComprasSrc)this.FindForm();
            dataTable.Rows.Clear();
            currentPage = 1;

            if (cbMes.SelectedItem == null || cbAño.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un mes y un año.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mesTexto = cbMes.SelectedItem.ToString();
            string añoTexto = cbAño.SelectedItem.ToString();

            int mesNumero = ObtenerNumeroMes(mesTexto);
            if (mesNumero == 0)
            {
                MessageBox.Show("Seleccione un mes válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int año;
            if (!int.TryParse(añoTexto, out año))
            {
                MessageBox.Show("Seleccione un año válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pictureNone.Visible = false;
            mainForm.ShowOverlay();

            try
            {
                var data = await _compraSrc.ListarImportados(3);

                var datosFiltrados = data.Where(c => c.Fecha.Year == año && c.Fecha.Month == mesNumero).ToList();

                dataTable.Rows.Clear();

                if (datosFiltrados.Count == 0)
                {
                    pictureNone.Visible = true;
                }
                else
                {
                    pictureNone.Visible = false;
                    foreach (var compra in datosFiltrados)
                    {
                        dataTable.Rows.Add(
                            compra.SerieCompra + "-" + compra.NumCompra,
                            compra.RucPersona,
                            compra.NomPersona,
                            compra.Fecha.ToString("dd/MM/yyyy"),
                            compra.Igv,
                            compra.SubTotal,
                            compra.Total,
                            compra.Estado == 3 ? "Importado" : "Procesando",
                            "ok",
                            "Editar"
                        );
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

    }
}
