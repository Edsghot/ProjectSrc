using PknoPlusCS.Global.Helper;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PknoPlusCS.Forms.DialogView
{
    public partial class Sucursal : Form
    {
        private readonly MainComprasSrc mainForm;
        private readonly ICompraSrcRepository _repo;
        private readonly ICompraSrcInputPort _compraSrc;
        private readonly int _index;

        private List<SucursalDto> sucursales;

        public Sucursal(MainComprasSrc main, string direccion, int index)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = main;

            txtDireccion.Text = string.IsNullOrEmpty(direccion) ? "Pendiente" : direccion;

            _index = index;
            _repo = new CompraSrcRepository();
            _compraSrc = new CompraSrcAdapter();
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
                        DataStaticDto.data[_index].Sucursal = txtDireccion.Text;
                        DataStaticDto.data[_index].NewSucursal = txtDireccion.Text;
                        DataStaticDto.data[_index].IdAlmacen = almacen;
                        DataStaticDto.data[_index].EstadoAlmacen = true;
                        DataStaticDto.data[_index].EstadoSucursal = true;
                        HFunciones.ActualizarEstados();
                        mainForm.ShowToast("Datos de la sucursal añadidos con éxito.", "success");
                        await _compraSrc.createBackup();
                        this.Close();
                   
                }
                catch (Exception ex)
                {
                    mainForm.ShowToast($"Error al actualizar los datos: {ex.Message}", "error");
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