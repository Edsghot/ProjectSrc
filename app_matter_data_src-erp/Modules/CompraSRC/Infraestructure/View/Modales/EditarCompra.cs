using app_matter_data_src_erp.Forms.DialogView;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class EditarCompra : Form
    {
        private readonly MainComprasSrc mainForm;
        private int fila;
        private string prove;
        private string code;
        private string docu;
        private readonly ICompraSrcInputPort _compraSrc;
        private readonly ICompraSrcRepository _repo;
        private List<SucursalDto> sucursales;
        public EditarCompra(string proveedor,string codigo,string documento,int row, MainComprasSrc mainForm1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text = codigo;
            fila = row; 
            prove = proveedor;
            code = codigo;
            docu = documento;
            _compraSrc = new CompraSrcAdapter();
            this.mainForm = mainForm1;
            _repo = new CompraSrcRepository();
        }

        private async void lblCoincidencia_Click(object sender, EventArgs e)
        {

            int rowIndex = this.fila;
            string codigoCompra = this.code; 
            string codigoProveedor = this.prove;
            string documen = this.docu;


            CompraDto compra = await _compraSrc.ObtenerCompraPorIdRecepcion("");

            string mensaje = $"Índice de fila: {rowIndex}\n" +
                             $"Código de Compra: {codigoCompra}\n" +
                             $"Documento: {documen}\n" +
                             $"Compra Detalle:\n" +
                             $"- Id: {compra.NumCompra}\n" +
                             $"- Fecha: {compra.Compras}\n";

  
            MessageBox.Show(mensaje, "Detalles de la Compra", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm, compra.idCompraSerie, compra.Compras, documen,rowIndex);
            modal.TopMost = true;
            modal.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnContinuar_Click(object sender, EventArgs e)
        {

            if (cbAlmacen.SelectedItem is SucursalDto almacenSeleccionado)
            {
                var idPunto = Convert.ToInt32(almacenSeleccionado.IdPuntoVenta);
                var almacen = Convert.ToInt32(almacenSeleccionado.IdAlmacen);
                try
                {
                    await _repo.ActualizarPuntoVentaYAlmacen(idPunto, almacen);
                    this.Close();

                }
                catch (Exception ex)
                {
                    //mainForm.ShowToast($"Error al actualizar los datos: {ex.Message}", "error");
                }
            }
        }

        private async void Sucursal_Load(object sender, EventArgs e)
        {
            await LoadSucursalData();
        }

        private async Task LoadSucursalData()
        {
            try
            {
                sucursales = (await _repo.getAllSucursal()).ToList();

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
                   
                    var almacenes = sucursales
                        .Where(a => a.IdPuntoVenta == sucursalSeleccionada.IdPuntoVenta)
                        .ToList();

                    cbAlmacen.DataSource = almacenes;
                    cbAlmacen.DisplayMember = "NomAlmacen";
                    cbAlmacen.ValueMember = "IdAlmacen";

                    var almacenesConAlmacenSrcTrue = almacenes
                        .Where(a => a.AlmacenSrc == "True")
                        .ToList();
                    if (almacenesConAlmacenSrcTrue.Any())
                    {
                        cbAlmacen.SelectedValue = almacenesConAlmacenSrcTrue.First().IdAlmacen;
                    }
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
        private void cbSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSucursal.SelectedItem is SucursalDto sucursalSeleccionada)
            {       

                var almacenes = sucursales
                    .Where(a => a.IdPuntoVenta == sucursalSeleccionada.IdPuntoVenta)
                    .ToList();

                cbAlmacen.DataSource = almacenes;
                cbAlmacen.DisplayMember = "NomAlmacen";
                cbAlmacen.ValueMember = "IdAlmacen";

                var almacenesConAlmacenSrcTrue = almacenes
                    .Where(a => a.AlmacenSrc == "True")
                    .ToList();
                if (almacenesConAlmacenSrcTrue.Any())
                {
                    cbAlmacen.SelectedValue = almacenesConAlmacenSrcTrue.First().IdAlmacen;
                }
            }
        }

        private void cbAlmacen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAlmacen.SelectedItem is SucursalDto almacenSeleccionado)
            {
                
            }
        }
    }
}
