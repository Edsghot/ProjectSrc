using PknoPlusCS.Forms;
using PknoPlusCS.Forms.Overlay;
using PknoPlusCS.Modules.CompraSRC.Application.Adapter;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Static;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using FontAwesome.Sharp; 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PknoPlusCS
{
    public partial class MainComprasSrc : Form
    {
        private IconButton activeButton;
        private readonly ICompraSrcInputPort compra;
        private UserControl activeControl;

        private OverlayForm overlay;
        public MainComprasSrc()
        {
            InitializeComponent();         
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadUserControl(new UCImportacionesCompra(), btnOption1);
            compra = new CompraSrcAdapter();
        }
        // ------------------------------------------------------------- TOAST
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

        // ------------------------------------------------------------- OVERLAY
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

        // ------------------------------------------------------------- SIDEBAR

        private void LoadUserControl(UserControl userControl, IconButton senderButton)
        {

            if (activeControl != null)
            {
                pnlContainer.Controls.Remove(activeControl);
            }

            activeControl = userControl;
            activeControl.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(activeControl);

            HighlightButton(senderButton);
        }

        private void HighlightButton(IconButton button)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.White;
                activeButton.ForeColor = Color.Black;
                activeButton.IconColor = Color.Black;
            }

            button.BackColor = Color.Blue;
            button.ForeColor = Color.White;
            button.IconColor = Color.White;

            activeButton = button; 
        }

        private void btnOption1_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UCImportacionesCompra(), (IconButton)sender);
        }

        private void btnOption2_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UCComprasImportadas(), (IconButton)sender);
        }

        private async void MainComprasSrc_Load(object sender, EventArgs e)
        {
            var data = await compra.GetMenu();

            lblRuc.Text = data.Ruc;
            lblEmpresa.Text = data.NomRuc;
            lblSucursal.Text = data.NomSucursal;

            ICompraSrcRepository repository = new CompraSrcRepository();
            var dataRepo =  repository.GetConfiguracionInicial();

            switch (dataRepo.reiniciar)
            {
                case 0:

                    break;
                case 1:
                    DataStaticDto.data = await compra.ObtenerDataSrc();
                     repository.UpdateConfiguracionInicial(0);
                    LoadUserControl(new UCComprasImportadas(), btnOption2);
                    break;

            }
        }


        private void MainComprasSrc_Activated(object sender, EventArgs e)
        {
            if (ControlStatic.CierreTotal)
            {
                this.Hide();
                this.overlay.Hide();
                this.Dispose();
            }
        }
    }
}
