using PknoPlusCS.Forms.DialogView;
using PknoPlusCS.Forms.Overlay;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Constantes;
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

namespace PknoPlusCS.Modules.CompraSRC.Infraestructure.View.Modales
{
    public partial class ResumenImportacion : Form
    {

        MainComprasSrc _main;
        List<Tuple<string, string>> Codigos;
        List<int> listIdTipoAuxiliar = new List<int>();
        public ResumenImportacion(MainComprasSrc main, List<Tuple<string, string>> CodigosYDocumentos)
        {
            InitializeComponent();
            _main = main;
            Codigos = CodigosYDocumentos;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
     
        private void ConfigurarEstilos()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            if (panel1 != null)
            {
                panel1.BackColor = Color.FromArgb(70, 130, 200);
            }

            if (label1 != null)
            {
                label1.ForeColor = Color.White;
                label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            }

            if (btnSalir != null)
            {
                btnSalir.Click += (s, e) =>
                {
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                };
            }


            if (lblTotal != null)
            {
                lblTotal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblTotal.ForeColor = Color.FromArgb(70, 130, 200);
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if (listIdTipoAuxiliar.Count > 0)
            {
                var modal = new Importar((MainComprasSrc)this.ParentForm, Codigos);
                modal.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("No hay tipos de productos para importar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ResumenImportacion_Load(object sender, EventArgs e)
        {

            try
            {
                var idsRecepcion = new HashSet<string>(Codigos.Select(x => x.Item2));
                var compras = DataStaticDto.data.Where(x => idsRecepcion.Contains(x.IdRecepcion)).ToList();
                var comprasDetalle = compras.SelectMany(x => x.Compras).ToList();

                var tiposProductosUnicos = comprasDetalle
                    .GroupBy(x => x.IdTipoAuxiliar)
                    .Select(grupo => new
                    {
                        IdTipoAuxiliar = grupo.Key,
                        NombreProducto = grupo.First().NombreTipoProducto
                    })
                    .OrderBy(x => x.IdTipoAuxiliar)
                    .ToList();

                listIdTipoAuxiliar = tiposProductosUnicos.Select(x => x.IdTipoAuxiliar).ToList();

                var datosParaMostrar = tiposProductosUnicos.Select((item, index) => new
                {
                    TipoAuxiliar = item.IdTipoAuxiliar,
                    NombreTipo = item.NombreProducto,
                    Cantidad = comprasDetalle.Count(x => x.IdTipoAuxiliar == item.IdTipoAuxiliar)
                }).ToList();


                MostrarDatosEnGrid(datosParaMostrar);

                ConfigurarEstilos();

                lblTotal.Text = $"Total de tipos Productos: {tiposProductosUnicos.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error en Resumen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void MostrarDatosEnGrid(dynamic datosParaMostrar)
        {
            if (panel3 == null)
                return;

            panel3.Controls.Clear();

            DataGridView dgv = new DataGridView();
            dgv.DataSource = datosParaMostrar;
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.LightGray;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.EnableHeadersVisualStyles = false;

            // Estilos de encabezado
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 30;

            // Estilos de celdas
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.RowTemplate.Height = 25;

            panel3.Controls.Add(dgv);
        }
    }
}
