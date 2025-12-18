using System.Drawing;
using System.Windows.Forms;

namespace PknoPlusCS.Forms.Overlay
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
            this.Owner = parentForm;

            // Habilitar animación del GIF
            if (pictureBox1.Image != null)
            {
                ImageAnimator.Animate(pictureBox1.Image, OnFrameChanged);
            }

            AdjustToParent();
            parentForm.SizeChanged += (s, e) => AdjustToParent();
            parentForm.LocationChanged += (s, e) => AdjustToParent();
        }

        private void OnFrameChanged(object sender, System.EventArgs e)
        {
            // Forzar actualización del frame del GIF
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new System.Action(() => pictureBox1.Invalidate()));
            }
            else
            {
                pictureBox1.Invalidate();
            }
        }

        public void SetMessage(string message)
        {
            if (label1 != null)
            {
                label1.Text = message;
            }
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Detener animación al cerrar
            if (pictureBox1.Image != null)
            {
                ImageAnimator.StopAnimate(pictureBox1.Image, OnFrameChanged);
            }
            base.OnFormClosing(e);
        }
    }
}