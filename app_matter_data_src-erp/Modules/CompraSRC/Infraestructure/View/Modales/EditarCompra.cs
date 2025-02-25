using app_matter_data_src_erp.Forms.DialogView;
using app_matter_data_src_erp.Forms.DialogView.DialogModal;
using app_matter_data_src_erp.Forms.Overlay;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using app_matter_data_src_erp.Shared.DialogModal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    public partial class EditarCompra : Form
    {
        private readonly MainComprasSrc mainForm;
        private string idRecepcion;
        private string codigo;
        private string rucRecuperado;
        private readonly ICompraSrcImportadosInputPort _compraSrc;
        private readonly ICompraSrcRepository _repo;

        private List<SucursalDto> sucursales;
        public EditarCompra(string code, string id, string ruc,MainComprasSrc mainForm1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCode.Text = code;
            this.idRecepcion = id; 
            this.codigo = code;
            this.rucRecuperado = ruc;
            _compraSrc = new CompraSrcImportadosAdapter();
            this.mainForm = mainForm1;
            _repo = new CompraSrcRepository();
        }

        private async void lblCoincidencia_Click(object sender, EventArgs e)
        {

            string id = this.idRecepcion;
            string codigo = this.codigo;
            List<CompraTemporalMonitoreoSrcDto> compras = await _compraSrc.GetComprasPorIdRecepcion(id);

            CoincidenciaProductosImported modal = new CoincidenciaProductosImported((MainComprasSrc)this.ParentForm,compras, codigo, rucRecuperado);
            modal.TopMost = true;
            modal.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            ControlStatic.CierreDIalogvIew = true;
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

                    var modal = new DIalogModalFInal();
                    modal.TopMost = true;
                    modal.ShowDialog();
                    this.Close();

                }
                catch (Exception ex)
                {                   
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
        private void DialogEditor_Activated(object sender, EventArgs e)
        {
            if (ControlStatic.CierreModalEditar)
            {
                this.Dispose();
            }
        }
    }
}
