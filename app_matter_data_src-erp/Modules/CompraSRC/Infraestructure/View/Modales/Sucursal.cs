using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class Sucursal : Form
    {
        private readonly MainComprasSrc mainForm;
        private readonly ICompraSrcRepository _repo;
        private readonly int _index;

        private List<SucursalDto> sucursales;

        public Sucursal(MainComprasSrc mainForm, string direccion, int index)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = mainForm;
            txtDireccion.Text = string.IsNullOrEmpty(direccion) ? "Pendiente" : direccion;
            _index = index;
            _repo = new CompraSrcRepository();
        }

        private async void Sucursal_Load(object sender, EventArgs e)
        {
            await LoadSucursalData();
        }

        private async Task LoadSucursalData()
        {
            try
            {
                //sucursales = await GetSimulatedSucursales();
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
                    txtDireccion.Text = sucursalSeleccionada.LocalFisico;

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

        private Task<List<SucursalDto>> GetSimulatedSucursales()
        {
            var sucursales = new List<SucursalDto>
            {
                new SucursalDto
                {
                    IdPuntoVenta = "1",
                    NomPuntoVenta = "Sucursal por defecto",
                    LocalFisico = "Av. Principal 123",
                    SucursalSRC = "True",
                    NomAlmacen = "Almacén por defecto",
                    IdAlmacen = "AL001",
                    AlmacenSrc = "True"
                },
                new SucursalDto
                {
                    IdPuntoVenta = "2",
                    NomPuntoVenta = "Sucursal 2",
                    LocalFisico = "Calle Norte 456",
                    SucursalSRC = "False",
                    NomAlmacen = "Almacén 1",
                    IdAlmacen = "AL002",
                    AlmacenSrc = "False"
                },
                new SucursalDto
                {
                    IdPuntoVenta = "2",
                    NomPuntoVenta = "Sucursal 2",
                    LocalFisico = "Plaza Sur 789",
                    SucursalSRC = "False",
                    NomAlmacen = "Almacén 2",
                    IdAlmacen = "AL003",
                    AlmacenSrc = "True"
                },
                new SucursalDto
                {
                    IdPuntoVenta = "3",
                    NomPuntoVenta = "Sucursal 3",
                    LocalFisico = "Av. Este 101",
                    SucursalSRC = "False",
                    NomAlmacen = "Almacén 11",
                    IdAlmacen = "AL004",
                    AlmacenSrc = "False"
                },
                 new SucursalDto
                {
                    IdPuntoVenta = "3",
                    NomPuntoVenta = "Sucursal 3",
                    LocalFisico = "Av. Este 500",
                    SucursalSRC = "False",
                    NomAlmacen = "Almacén 22",
                    IdAlmacen = "AL004",
                    AlmacenSrc = "False"
                },
                     new SucursalDto
                {
                    IdPuntoVenta = "3",
                    NomPuntoVenta = "Sucursal 3",
                    LocalFisico = "Av. Este 800",
                    SucursalSRC = "False",
                    NomAlmacen = "Almacén 33",
                    IdAlmacen = "AL004",
                    AlmacenSrc = "False"
                }
            };

            return Task.FromResult(sucursales);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if (cbAlmacen.SelectedItem is SucursalDto almacenSeleccionado)
            {
                var response = _repo.ActualizarPuntoVentaYAlmacen(Convert.ToInt32(almacenSeleccionado.IdPuntoVenta), Convert.ToInt32(almacenSeleccionado.IdAlmacen)).GetAwaiter();
                if (response.IsCompleted == true)
                {
                    DataStaticDto.data[_index].Sucursal = txtDireccion.Text;
                    mainForm.ShowToast("Datos de la sucursal añadidos con éxito.", "success");
                    this.Close();
                }
                else
                {
                    mainForm.ShowToast("Oppss Alg salio mal.", "error");
                }
            }
        }

        private void cbSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSucursal.SelectedItem is SucursalDto sucursalSeleccionada)
            {
                txtDireccion.Text = sucursalSeleccionada.LocalFisico;

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
                txtDireccion.Text = almacenSeleccionado.LocalFisico;
            }
        }
    }
}