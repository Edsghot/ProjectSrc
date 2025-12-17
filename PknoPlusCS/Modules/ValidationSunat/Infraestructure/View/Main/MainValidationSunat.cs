using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using PknoPlusCS.Configuration.Constants;
using PknoPlusCS.Forms;
using PknoPlusCS.Forms.Overlay;
using PknoPlusCS.Modules.ValidationSunat.Application.Adapter;
using PknoPlusCS.Modules.ValidationSunat.Application.Port;
using PknoPlusCS.Modules.ValidationSunat.Domain.Dto;
using PknoPlusCS.Modules.ValidationSunat.Domain.Repository;
using PknoPlusCS.Modules.ValidationSunat.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PknoPlusCS.Modules
{
    public partial class MainValidationSunat : Form
    {
        private UserControl activeControl;
        private IconButton activeButton;
        private OverlayForm overlay;

        private readonly IRepositoryValidationSunat _repo;
        private readonly IValidationSunatInputPort _adapter;
        private List<ListCpesDto> _datosOriginales;

        private readonly Color COLOR_SIN_VALIDAR = Color.FromArgb(158, 158, 158);      
        private readonly Color COLOR_ACEPTADO = Color.FromArgb(76, 175, 80);           
        private readonly Color COLOR_NO_EXISTE = Color.FromArgb(244, 67, 54);          
        private readonly Color COLOR_ANULADO = Color.FromArgb(198, 40, 40);            

        public MainValidationSunat()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _repo = new ValidationSunatRepository();
            _adapter = new ValidationSunatAdapter();

            HabilitarDoubleBuffering(gunaDataGrid);

            ConfigurarComboBoxMeses();
            ConfigurarComboBoxAnios();
            ConfigurarDatePickers();
            ConfigurarFiltrosIniciales();
            ConfigurarDataGrid();
        }

        #region Configuración Inicial

        private void ConfigurarComboBoxMeses()
        {
            cbMes.Items.Clear();
            cbMes.Items.AddRange(new object[]
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            });
            cbMes.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void ConfigurarComboBoxAnios()
        {
            cbAnio.Items.Clear();
            int anioActual = DateTime.Now.Year;
            for (int i = anioActual - 5; i <= anioActual + 5; i++)
            {
                cbAnio.Items.Add(i.ToString());
            }

            int indexAnio = cbAnio.Items.IndexOf(anioActual.ToString());
            if (indexAnio >= 0) cbAnio.SelectedIndex = indexAnio;
        }

        private void ConfigurarDatePickers()
        {
            gunaDateIni.Value = DateTime.Now;
            gunaDateIni.CustomFormat = "'Fecha inicio'";
            gunaDateIni.Format = DateTimePickerFormat.Custom;

            gunaDateFin.Value = DateTime.Now;
            gunaDateFin.CustomFormat = "'Fecha fin'";
            gunaDateFin.Format = DateTimePickerFormat.Custom;

            gunaDateIni.ValueChanged += (s, e) => gunaDateIni.CustomFormat = "dd/MM/yyyy";
            gunaDateFin.ValueChanged += (s, e) => gunaDateFin.CustomFormat = "dd/MM/yyyy";
        }

        private void ConfigurarFiltrosIniciales()
        {
            gunaTipoCompro.Items.Clear();
            gunaTipoCompro.Items.Add("Tipo compro.");
            gunaTipoCompro.SelectedIndex = 0;

            gunaEstadoCompro.Items.Clear();
            gunaEstadoCompro.Items.Add("Estado compro.");
            gunaEstadoCompro.SelectedIndex = 0;

            gunaEstadoSunat.Items.Clear();
            gunaEstadoSunat.Items.Add("Estado Sunat");
            gunaEstadoSunat.SelectedIndex = 0;

            guna2TextBox1.PlaceholderText = "RUC";
            guna2TextBox2.PlaceholderText = "Razón Social";

            gunaTipoCompro.SelectedIndexChanged += Filtros_Changed;
            gunaEstadoCompro.SelectedIndexChanged += Filtros_Changed;
            gunaEstadoSunat.SelectedIndexChanged += Filtros_Changed;
            guna2TextBox1.TextChanged += Filtros_Changed;
            guna2TextBox2.TextChanged += Filtros_Changed;
            gunaDateIni.ValueChanged += Filtros_Changed;
            gunaDateFin.ValueChanged += Filtros_Changed;

            BtnResetearFiltro.Click += BtnResetearFiltro_Click;
        }

        private void CargarFiltrosDinamicos()
        {
            if (_datosOriginales == null || _datosOriginales.Count == 0)
                return;

            string tipoActual = gunaTipoCompro.SelectedIndex > 0 ? gunaTipoCompro.SelectedItem?.ToString() : null;
            string estadoComproActual = gunaEstadoCompro.SelectedIndex > 0 ? gunaEstadoCompro.SelectedItem?.ToString() : null;
            string estadoSunatActual = gunaEstadoSunat.SelectedIndex > 0 ? gunaEstadoSunat.SelectedItem?.ToString() : null;

            gunaTipoCompro.Items.Clear();
            gunaTipoCompro.Items.Add("Tipo compro.");
            var tiposUnicos = _datosOriginales
                .Where(x => !string.IsNullOrEmpty(x.TipoComprobante))
                .Select(x => x.TipoComprobante)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            foreach (var tipo in tiposUnicos)
            {
                gunaTipoCompro.Items.Add(tipo);
            }
            gunaTipoCompro.SelectedIndex = tipoActual != null && gunaTipoCompro.Items.Contains(tipoActual)
                ? gunaTipoCompro.Items.IndexOf(tipoActual) : 0;

            gunaEstadoCompro.Items.Clear();
            gunaEstadoCompro.Items.Add("Estado compro.");
            var estadosComproUnicos = _datosOriginales
                .Where(x => !string.IsNullOrEmpty(x.EstadoCompro))
                .Select(x => x.EstadoCompro)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            foreach (var estado in estadosComproUnicos)
            {
                gunaEstadoCompro.Items.Add(estado);
            }
            gunaEstadoCompro.SelectedIndex = estadoComproActual != null && gunaEstadoCompro.Items.Contains(estadoComproActual)
                ? gunaEstadoCompro.Items.IndexOf(estadoComproActual) : 0;

      
            gunaEstadoSunat.Items.Clear();
            gunaEstadoSunat.Items.Add("Estado Sunat");
            var estadosSunatUnicos = _datosOriginales
                .Where(x => !string.IsNullOrEmpty(x.EstadoSunat))
                .Select(x => x.EstadoSunat)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            foreach (var estado in estadosSunatUnicos)
            {
                gunaEstadoSunat.Items.Add(estado);
            }
            gunaEstadoSunat.SelectedIndex = estadoSunatActual != null && gunaEstadoSunat.Items.Contains(estadoSunatActual)
                ? gunaEstadoSunat.Items.IndexOf(estadoSunatActual) : 0;
        }

        private void ConfigurarDataGrid()
        {
            gunaDataGrid.AutoGenerateColumns = false;
            gunaDataGrid.Columns.Clear();

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "N° Item",
                Name = "colId",
                Width = 30,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoComprobante",
                HeaderText = "Tipo de comprobante",
                Name = "colTipoComprobante",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NroComprobante",
                HeaderText = "N° compro.",
                Name = "colNroComprobante",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EstadoCompro",
                HeaderText = "Estado compro.",
                Name = "colEstadoCompro",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ruc",
                HeaderText = "RUC",
                Name = "colRuc",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RazonSocial",
                HeaderText = "Razón social",
                Name = "colRazonSocial",
                Width = 250,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaEmision",
                HeaderText = "Fecha de emisión",
                Name = "colFechaEmision",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Moneda",
                HeaderText = "Moneda",
                Name = "colMoneda",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ImporteSoles",
                HeaderText = "Importe Soles (S/)",
                Name = "colImporteSoles",
                Width = 115,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ImporteDolares",
                HeaderText = "Importe Dólares ($)",
                Name = "colImporteDolares",
                Width = 115,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            gunaDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EstadoSunat",
                HeaderText = "Estado SUNAT",
                Name = "colEstadoSunat",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            ConfigurarEstilosDataGrid();

            gunaDataGrid.CellFormatting += GunaDataGrid_CellFormatting;
            gunaDataGrid.CellPainting += GunaDataGrid_CellPainting;
            gunaDataGrid.RowPostPaint += GunaDataGrid_RowPostPaint;
        }

        private void GunaDataGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top,
                grid.Columns["colId"].Width, e.RowBounds.Height);

            e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText,
                headerBounds, centerFormat);
        }

        private void ConfigurarEstilosDataGrid()
        {
            gunaDataGrid.BorderStyle = BorderStyle.None;
            gunaDataGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gunaDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gunaDataGrid.EnableHeadersVisualStyles = false;
            gunaDataGrid.GridColor = Color.White; 

            gunaDataGrid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = Color.FromArgb(100, 100, 100),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(5)
            };
            gunaDataGrid.ColumnHeadersHeight = 45;

            gunaDataGrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(235, 245, 255),
                SelectionForeColor = Color.FromArgb(70, 70, 70),
                Padding = new Padding(3)
            };

            gunaDataGrid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(252, 252, 252),
                ForeColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(235, 245, 255),
                SelectionForeColor = Color.FromArgb(70, 70, 70)
            };

            gunaDataGrid.RowTemplate.Height = 42;

            gunaDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gunaDataGrid.ReadOnly = true;
            gunaDataGrid.AllowUserToAddRows = false;
            gunaDataGrid.AllowUserToDeleteRows = false;
            gunaDataGrid.AllowUserToResizeRows = false;
            gunaDataGrid.RowHeadersVisible = false;
            gunaDataGrid.MultiSelect = false;
            gunaDataGrid.ScrollBars = ScrollBars.Both;
            gunaDataGrid.BackgroundColor = Color.White;
        }

        private void GunaDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if ((gunaDataGrid.Columns[e.ColumnIndex].Name == "colImporteSoles" ||
                 gunaDataGrid.Columns[e.ColumnIndex].Name == "colImporteDolares") && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal importe) && importe == 0)
                {
                    e.Value = "-";
                    e.FormattingApplied = true;
                    gunaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void GunaDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
 
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 &&
                gunaDataGrid.Columns[e.ColumnIndex].Name == "colEstadoSunat")
            {
                e.PaintBackground(e.ClipBounds, true);

                if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
                {
                    string estado = e.Value.ToString();
                    Color bgColor = GetColorEstado(estado);

                    int badgeWidth = Math.Min(e.CellBounds.Width - 16, 90);
                    int badgeHeight = 24;
                    int x = e.CellBounds.X + (e.CellBounds.Width - badgeWidth) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - badgeHeight) / 2;

                    Rectangle rect = new Rectangle(x, y, badgeWidth, badgeHeight);

                    using (var brush = new SolidBrush(bgColor))
                    using (var path = CrearRectanguloRedondeado(rect, 12))
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.FillPath(brush, path);
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        estado,
                        new Font("Segoe UI", 8F, FontStyle.Bold),
                        rect,
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                }

                e.Handled = true;
            }
        }
        private Color GetColorEstado(string estado)
        {
            switch (estado?.ToUpper())
            {
                case "SIN VALIDAR":
                    return COLOR_SIN_VALIDAR;
                case "ACEPTADO":
                case "VÁLIDO":
                    return COLOR_ACEPTADO;
                case "NO EXISTE":
                    return COLOR_NO_EXISTE;
                case "ANULADO":
                    return COLOR_ANULADO;
                default:
                    return Color.Gray;
            }
        }

        private System.Drawing.Drawing2D.GraphicsPath CrearRectanguloRedondeado(Rectangle rect, int radio)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
            path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
            path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region Búsqueda Principal

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMes.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar un mes", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbMes.Focus();
                    return;
                }

                if (cbAnio.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar un año", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbAnio.Focus();
                    return;
                }

                int mes = cbMes.SelectedIndex + 1;
                int anio = int.Parse(cbAnio.SelectedItem.ToString());

                this.Cursor = Cursors.WaitCursor;

                var resultado = _repo.UpsertCPESComprasValidados(4, mes, anio);
                Console.WriteLine($"Registros procesados: {resultado.RegistrosProcesados}");

                _datosOriginales = _repo.ListarCPESComprasValidados(mes, anio);
                gunaDataGrid.DataSource = _datosOriginales;

                panel7.Enabled = true;

                CargarFiltrosDinamicos();


                ResetearFiltros();

                if (_datosOriginales.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros para el período seleccionado",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                var sumaSoles = _datosOriginales.Sum(x => x.ImporteSoles);
                var sumaDolares = _datosOriginales.Sum(x => x.ImporteDolares);
                lblTotalSoles.Text = sumaSoles.ToString("N2");
                lblTotalDolares.Text = sumaDolares.ToString("N2");

                btnExportar.Enabled = true;
                BtnValidarSunat.Enabled = true;
                lblTotalRegistro.Text = _datosOriginales.Count.ToString();
                iconButton1.BackColor = Color.FromArgb(202, 0, 57);
                iconSunat.BackColor = Color.Blue;
                iconExportar.BackColor = Color.FromArgb(203, 98, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Filtros

        private void Filtros_Changed(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (_datosOriginales == null || _datosOriginales.Count == 0)
                return;

            var datosFiltrados = _datosOriginales.AsEnumerable();

            if (gunaTipoCompro.SelectedIndex > 0)
            {
                string tipoSeleccionado = gunaTipoCompro.SelectedItem.ToString();
                datosFiltrados = datosFiltrados.Where(x => x.TipoComprobante == tipoSeleccionado);
            }

            if (gunaEstadoCompro.SelectedIndex > 0)
            {
                string estadoSeleccionado = gunaEstadoCompro.SelectedItem.ToString();
                datosFiltrados = datosFiltrados.Where(x => x.EstadoCompro == estadoSeleccionado);
            }

            if (gunaEstadoSunat.SelectedIndex > 0)
            {
                string estadoSunat = gunaEstadoSunat.SelectedItem.ToString();
                datosFiltrados = datosFiltrados.Where(x => x.EstadoSunat == estadoSunat);
            }

            if (!string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                string ruc = guna2TextBox1.Text.Trim();
                datosFiltrados = datosFiltrados.Where(x => x.Ruc.Contains(ruc));
            }

            if (!string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                string razonSocial = guna2TextBox2.Text.Trim().ToUpper();
                datosFiltrados = datosFiltrados.Where(x => x.RazonSocial.ToUpper().Contains(razonSocial));
            }

            if (gunaDateIni.CustomFormat == "dd/MM/yyyy")
            {
                DateTime fechaInicio = gunaDateIni.Value.Date;
                datosFiltrados = datosFiltrados.Where(x => x.FechaEmision.Date >= fechaInicio);
            }

            if (gunaDateFin.CustomFormat == "dd/MM/yyyy")
            {
                DateTime fechaFin = gunaDateFin.Value.Date;
                datosFiltrados = datosFiltrados.Where(x => x.FechaEmision.Date <= fechaFin);
            }

            gunaDataGrid.DataSource = datosFiltrados.ToList();
        }

        private void ResetearFiltros()
        {
            gunaTipoCompro.SelectedIndex = 0;
            gunaEstadoCompro.SelectedIndex = 0;
            gunaEstadoSunat.SelectedIndex = 0;

            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";

            gunaDateIni.Value = DateTime.Now;
            gunaDateIni.CustomFormat = "'Fecha inicio'";

            gunaDateFin.Value = DateTime.Now;
            gunaDateFin.CustomFormat = "'Fecha fin'";
        }

        private void BtnResetearFiltro_Click(object sender, EventArgs e)
        {
            ResetearFiltros();

            if (_datosOriginales != null)
            {
                gunaDataGrid.DataSource = _datosOriginales;
            }
        }

        #endregion

        #region Toast y Overlay

        public void ShowToast(string message, string type)
        {
            var toast = new Toast(message, type)
            {
                Location = new Point(this.Width - 630, this.Height - 105),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            this.Controls.Add(toast);
            toast.BringToFront();
        }

        public void ShowOverlay()
        {
            if (overlay == null || overlay.IsDisposed)
            {
                overlay = new OverlayForm(this);
            }
            overlay.Show();
            overlay.BringToFront();
        }

        public void HideOverlay()
        {
            overlay?.Hide();
        }

        #endregion

        #region Eventos UI

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            btnBuscar_Click(sender, e);
        }

        #endregion

        private void BtnResetearFiltro_Click_1(object sender, EventArgs e)
        {
            ResetearFiltros();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ResetearFiltros();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
        private void HabilitarDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true }
            );
        }

        private  void BtnValidarSunat_Click(object sender, EventArgs e)
        {

            if (_datosOriginales == null || _datosOriginales.Count == 0)
            {
                MessageBox.Show("No hay datos para validar. Primero realice una búsqueda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pendientes = _datosOriginales.Count(x => x.EstadoSunat == "SIN VALIDAR");

            if (pendientes == 0)
            {
                MessageBox.Show("No hay comprobantes pendientes de validar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"Se validarán {pendientes} comprobantes con SUNAT.\n\n¿Desea continuar?",
                "Confirmar validación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                BtnValidarSunat.Enabled = false;
                BtnValidarSunat.Text = "Validando...";

                var resultado =  _adapter.ValidarSunatAsync(_datosOriginales, Credentials.Ruc);

                int mes = cbMes.SelectedIndex + 1;
                int anio = int.Parse(cbAnio.SelectedItem.ToString());

                _datosOriginales = _repo.ListarCPESComprasValidados(mes, anio);

                gunaDataGrid.DataSource = null;
                gunaDataGrid.DataSource = _datosOriginales;

                CargarFiltrosDinamicos();

                MessageBox.Show(
                    $"Validación completada.\n\n" +
                    $"✓ Exitosos: {resultado.Exitosos}\n" +
                    $"✗ Errores: {resultado.Errores}\n" +
                    $"Total procesados: {resultado.TotalProcesados}",
                    "Resultado",
                    MessageBoxButtons.OK,
                    resultado.Exitosos > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la validación: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                BtnValidarSunat.Enabled = true;
                BtnValidarSunat.Text = "Validar con SUNAT";
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            var datosExportar = gunaDataGrid.DataSource as List<ListCpesDto>;

            if (datosExportar == null || datosExportar.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnExportar.Enabled = false;

                int mes = cbMes.SelectedIndex + 1;
                int anio = int.Parse(cbAnio.SelectedItem.ToString());

                string rutaArchivo = _adapter.ExportarExcel(datosExportar, Credentials.Ruc, mes, anio);

                if (!string.IsNullOrEmpty(rutaArchivo))
                {
                    var resultado = MessageBox.Show(
                        $"Archivo exportado exitosamente.\n\n" +
                        $"Ubicación: {rutaArchivo}\n\n" +
                        $"¿Desea abrir el archivo?",
                        "Exportación completada",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (resultado == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(rutaArchivo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnExportar.Enabled = true;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}