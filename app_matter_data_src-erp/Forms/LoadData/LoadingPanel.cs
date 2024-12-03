using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.LoadData
{
    public partial class LoadingPanel : UserControl
    {
        public LoadingPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }
        public void ShowLoading(bool show)
        {
            this.Visible = show;  // Muestra u oculta el panel de carga según el parámetro

            if (show)
            {
                this.BringToFront();  // Asegura que el panel esté por encima de todo
            }
        }
    }
}
