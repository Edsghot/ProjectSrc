using System.Drawing;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.Overlay
{
    public partial class OverlayForm : Form
    {
        private Form parentForm;

        public OverlayForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;

            this.FormBorderStyle = FormBorderStyle.None;  
            this.StartPosition = FormStartPosition.Manual; 

            this.ShowInTaskbar = false; 
            this.TopMost = true; 

            AdjustToParent();

          
            parentForm.SizeChanged += (s, e) => AdjustToParent();
            parentForm.LocationChanged += (s, e) => AdjustToParent();

            parentForm.Resize += (s, e) =>
            {
                if (parentForm.WindowState == FormWindowState.Minimized)
                {
                    this.Hide(); 
                }
                else
                {
                    this.Show(); 
                }
            };
        }

        private void AdjustToParent()
        {
            if (parentForm != null)
            {
                this.Size = parentForm.ClientSize;
                this.Location = parentForm.PointToScreen(Point.Empty); 
                this.Invalidate();  
            }
        }
    }
}
