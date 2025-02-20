
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Forms.DialogView;

namespace app_matter_data_src_erp.Forms
{
    public partial class UCComprasImportadas : UserControl
    {
        private int currentPage = 1;
        private int rowsPerPage = 15;
        private int totalRows;
        private readonly ICompraSrcInputPort _compraSrc;

        public UCComprasImportadas()
        {
            InitializeComponent();
            dataTable.CellClick += dataTable_CellClick; dataTable.CellFormatting += dataTable_CellFormatting;

            this.dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataTable.ColumnHeadersHeight = 45;

            cbMes.SelectedItem = SeleccionesGlobalesDto.MesSeleccionado;
            cbAño.SelectedItem = SeleccionesGlobalesDto.AñoSeleccionado;
            _compraSrc = new CompraSrcAdapter();
            if (DatosGlobales.Compras.Count > 0)
            {
                LoadData();
            }

        }
        private async void LoadData()
        {
            try
            {
                dataTable.Rows.Clear();
                DatosGlobales.Compras.Clear();

                var dataList = await _compraSrc.ListarImportados(3);

                if (dataList == null || dataList.Count == 0)
                {
                    pictureNone.Visible = true;
                    return;
                }

                pictureNone.Visible = false;

                foreach (var compra in dataList)
                {
                    var compraDto = new CompraRDto
                    {
                        Codigo = compra.SerieCompra + "-"+compra.NumCompra,
                        Nombre = compra.NomPersona,
                        Fecha = compra.Fecha.ToString("dd/MM/yyyy"),
                        Igv = compra.Igv,
                        SubTotal = compra.SubTotal,
                        Total = compra.Total,
                        Estado = compra.Estado ==3? "Importado" : "Procesando",
                        Accion = "Editar"
                    };
                    DatosGlobales.Compras.Add(compraDto);
                }

                totalRows = DatosGlobales.Compras.Count;

                for (int i = (currentPage - 1) * rowsPerPage; i < Math.Min(currentPage * rowsPerPage, totalRows); i++)
                {
                    var compra = DatosGlobales.Compras[i];
                    dataTable.Rows.Add(
                        compra.Codigo,
                        compra.Nombre,
                        compra.Fecha,
                        compra.Igv,
                        compra.SubTotal,
                        compra.Total,
                        compra.Estado,
                        compra.Accion
                    );
                }

                UpdatePagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botones filtrado de tabla
        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)totalRows / rowsPerPage);
            label3.Text = currentPage.ToString();

            iconButton4.Enabled = currentPage > 1;
            iconButton1.Enabled = currentPage < totalPages;
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

        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataTable.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataTable.Columns.Count)
            {
                var columnName = dataTable.Columns[e.ColumnIndex].Name;

                OverlayFormModal overlayForm = new OverlayFormModal(this.ParentForm);


                if (columnName == "Column8")
                {
                    int rowIndex = e.RowIndex;
                    string code = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();


                    var modal = new DialogModal("¡Importante!", "Al editar esta compra, se eliminará la importación registrada en el ERP y se creara un nuevo registro de importación.", "warning", code, rowIndex, DataStaticDto.data[rowIndex].DocumentoProveedor);
                    overlayForm.ShowOverlayWithModal(modal);
                }
            }
        }
        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataTable.Columns[e.ColumnIndex].Name == "Column8")
            {
                e.CellStyle.ForeColor = Color.Chocolate;

                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            }
        }

        private async void btnLimpiar_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            DatosGlobales.Compras.Clear();
            SeleccionesGlobalesDto.MesSeleccionado = null; 
            SeleccionesGlobalesDto.AñoSeleccionado = null;
            cbMes.Text = "Seleccione el mes";
            cbAño.Text = "Seleccione el año";

            pictureNone.Visible = true;
            currentPage = 1;  
            UpdatePagination(); 
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

        private void cbMes_SelectedIndexChanged(object sender, EventArgs e)
        {          
            SeleccionesGlobalesDto.MesSeleccionado = cbMes.SelectedItem.ToString();
        }

        private void cbAño_SelectedIndexChanged(object sender, EventArgs e)
        {          
            SeleccionesGlobalesDto.AñoSeleccionado = cbAño.SelectedItem.ToString();
        }
    }
}
