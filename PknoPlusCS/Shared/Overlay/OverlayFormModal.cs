using System.Drawing;
using System.Windows.Forms;

namespace PknoPlusCS.Forms.Overlay
{
    public partial class OverlayFormModal : Form
    {
        private Form parentForm;

        public OverlayFormModal(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.FormBorderStyle = FormBorderStyle.None; 
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.FromArgb(0, 7, 22);  
            this.ShowInTaskbar = false;  

            AdjustToParent();
            parentForm.SizeChanged += (s, e) => AdjustToParent();
            parentForm.LocationChanged += (s, e) => AdjustToParent();
        }

        private void AdjustToParent()
        {
            if (parentForm != null)
            {
                this.Size = parentForm.ClientSize; 
                this.Location = parentForm.PointToScreen(Point.Empty); 
            }
        }

        public void ShowOverlayWithModal(Form modal)
        {
            this.Show();
            modal.TopMost = true; 
            modal.FormClosed += (s, e) => this.Hide(); 
            modal.ShowDialog(); 
        }
    }
}
