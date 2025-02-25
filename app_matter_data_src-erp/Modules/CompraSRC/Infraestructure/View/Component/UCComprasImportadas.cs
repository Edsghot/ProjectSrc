
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;

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

            cbMes.SelectedItem = SeleccionesGlobalesDto.MesSeleccionado;
            cbAño.SelectedItem = SeleccionesGlobalesDto.AñoSeleccionado;
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
            if (dataTable.Columns[e.ColumnIndex].Name == "Column9")
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

                if (columnName == "Column9")
                {
                    ControlStatic.CierreDIalogvIew = false;
                    int rowIndex = e.RowIndex;
                    string code = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string ruc = dataTable.Rows[e.RowIndex].Cells[1].Value.ToString();

                    if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(ruc))
                    {
                        var idRecepcion = await _compraSrc.GetIdRecepcion(code, ruc);
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
            LoadData();
        }

        private void cbAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionesGlobalesDto.AñoSeleccionado = cbAño.SelectedItem.ToString();
            LoadData();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var mainForm = (MainComprasSrc)this.FindForm();
            dataTable.Rows.Clear();
            currentPage = 1;

            if (cbMes.Text != "Seleccione el mes" && cbAño.Text != "Seleccione el mes")
            {
                pictureNone.Visible = false;
                mainForm.ShowOverlay();
                await Task.Delay(3000);
                LoadData();
                mainForm.HideOverlay();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un mes y un año.",
                                "Campos vacíos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }

        }
        //----------------------------------------------------------------------------------- BUTTON LIMPIAR
        private async void btnLimpiar_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            DatosImportadosStatic.Data.Clear();
            SeleccionesGlobalesDto.MesSeleccionado = null; 
            SeleccionesGlobalesDto.AñoSeleccionado = null;
            cbMes.Text = "Seleccione el mes";
            cbAño.Text = "Seleccione el año";

            pictureNone.Visible = true;
            currentPage = 1;  
            UpdatePagination(); 
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
