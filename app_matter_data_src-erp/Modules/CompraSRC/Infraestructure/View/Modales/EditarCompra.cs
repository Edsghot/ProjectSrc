using app_matter_data_src_erp.Forms.DialogView;
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace app_matter_data_src_erp.Forms
{
    public partial class EditarCompra : Form
    {
        private readonly MainComprasSrc mainForm;
        private int fila;
        private string code;
        private readonly ICompraSrcInputPort _compraSrc;
        private readonly ICompraSrcRepository _repo;
        private List<SucursalDto> sucursales;
        public EditarCompra(string codigo,string documento,int row, MainComprasSrc mainForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text = codigo;
            fila = row;
            code = codigo;
            _compraSrc = new CompraSrcAdapter();
            this.mainForm = mainForm;
            _repo = new CompraSrcRepository();
        }

        private async void lblCoincidencia_Click(object sender, EventArgs e)
        {

            int rowIndex = this.fila;
            string codigoCompra = this.code;
            CompraDto compra = await _compraSrc.ObtenerCompraPorCodigo(codigoCompra);

            CoincidenciaProductos modal = new CoincidenciaProductos((MainComprasSrc)this.ParentForm, compra.NumCompra, compra.Compras, DataStaticDto.data[rowIndex].DocumentoProveedor);
            modal.TopMost = true;
            modal.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
           // var modal = new DialogModal("¿Estás seguro?", "Para realizar una nueva modificación tendrás que hacerla directamente desde el ERP de forma manual.", "question", "", this);
           // modal.ShowDialog();
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
