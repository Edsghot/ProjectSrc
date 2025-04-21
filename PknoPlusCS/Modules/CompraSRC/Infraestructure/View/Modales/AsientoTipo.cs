using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using System;
using System.Reflection;
using System.Windows.Forms;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using System.Collections.Generic;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using System.Linq;
using System.Threading.Tasks;
using PknoPlusCS.Global.Helper;
using System.Windows.Media.Media3D;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;

namespace PknoPlusCS.Forms.DialogView
{
    public partial class AsientoTipo : Form
    {
        private readonly MainComprasSrc mainForm;
        private readonly ICompraSrcRepository _repo;
        private readonly ICompraSrcInputPort _compraSrc;
        private readonly int _index;
        private List<PlantillasDto> planillas;
        public AsientoTipo(MainComprasSrc main, int index)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = main;
            _index = index;
            _repo = new CompraSrcRepository();
            _compraSrc = new CompraSrcAdapter();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async Task loadAsiento()
        {
            try
            {
                planillas = (await _repo.spListarEspecificasCompras()).ToList();

                var planillasUnicas = planillas
                    .GroupBy(p => p.NomPlantilla)
                    .Select(g => g.First())
                    .ToList();

                cbAsientoTipo.DataSource = planillasUnicas;
                cbAsientoTipo.DisplayMember = "NomPlantilla";
                cbAsientoTipo.ValueMember = "IdPlantilla";

              

                if (planillasUnicas.Any())
                {
                    var planillaSeleccionada = planillasUnicas.First();
                    cbAsientoTipo.SelectedValue = planillaSeleccionada.IdPlantilla;

                    // Aquí puedes agregar cualquier lógica adicional que necesites para las planillas seleccionadas
                }
                else
                {
                    MessageBox.Show("No se encontró ninguna planilla activa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las planillas: {ex.Message}");
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {

            if (cbAsientoTipo.SelectedItem is PlantillasDto planillaSeleccionada)
            {
                var idPlantilla = (planillaSeleccionada.IdPlantilla).ToString();
                var nomPlantilla = (planillaSeleccionada.NomPlantilla).ToString();
                try
                {
                    DataStaticDto.data[_index].IdPlantilla = idPlantilla;
                    DataStaticDto.data[_index].NomPlantilla = nomPlantilla;
                    DataStaticDto.data[_index].EstadoAsiento = true;
                    HFunciones.ActualizarEstados();
                    mainForm.ShowToast("Datos de la plantilla añadidos con éxito.", "success");
                    _compraSrc.createBackup().GetAwaiter().GetResult();
                    this.Close();
                }
                catch (Exception ex)
                {
                    mainForm.ShowToast($"Error al actualizar los datos: {ex.Message}", "error");
                }
            }
        }

        private async void AsientoTipo_Load(object sender, EventArgs e)
        {
            await loadAsiento();
        }

        private void cbAsientoTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
