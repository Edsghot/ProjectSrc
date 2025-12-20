using System;
using System.Drawing;
using System.Windows.Forms;

namespace PknoPlusCS.Forms.Overlay
{
    public partial class OverlayForm : Form
    {
        private Form parentForm;
        private bool isAnimating;

        public OverlayForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.Owner = parentForm;

            AdjustToParent();

            // Guardar referencias para poder desuscribir
            parentForm.SizeChanged += ParentForm_SizeChanged;
            parentForm.LocationChanged += ParentForm_LocationChanged;
        }

        private void ParentForm_SizeChanged(object sender, EventArgs e) => AdjustToParent();
        private void ParentForm_LocationChanged(object sender, EventArgs e) => AdjustToParent();

        private void OnFrameChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1 != null && !pictureBox1.IsDisposed)
                {
                    if (pictureBox1.InvokeRequired)
                    {
                        pictureBox1.BeginInvoke(new Action(() => pictureBox1.Invalidate()));
                    }
                    else
                    {
                        pictureBox1.Invalidate();
                    }
                }
            }
            catch { }
        }

        public new void Show()
        {
            StartAnimation();
            base.Show();
        }

        public new void Hide()
        {
            StopAnimation();
            base.Hide();
        }

        private void StartAnimation()
        {
            if (!isAnimating && pictureBox1?.Image != null)
            {
                ImageAnimator.Animate(pictureBox1.Image, OnFrameChanged);
                isAnimating = true;
            }
        }

        private void StopAnimation()
        {
            if (isAnimating && pictureBox1?.Image != null)
            {
                ImageAnimator.StopAnimate(pictureBox1.Image, OnFrameChanged);
                isAnimating = false;
            }
        }

        public void SetMessage(string message)
        {
            if (label1 != null && !label1.IsDisposed)
            {
                label1.Text = message;
            }
        }

        private void AdjustToParent()
        {
            try
            {
                if (parentForm != null && !parentForm.IsDisposed)
                {
                    this.Size = parentForm.ClientSize;
                    this.Location = parentForm.PointToScreen(Point.Empty);
                }
            }
            catch { }
        }

        public void ForceClose()
        {
            try
            {
                StopAnimation();

                // Desuscribir eventos
                if (parentForm != null)
                {
                    parentForm.SizeChanged -= ParentForm_SizeChanged;
                    parentForm.LocationChanged -= ParentForm_LocationChanged;
                    parentForm = null;
                }

                this.Close();
                this.Dispose();
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopAnimation();

            if (parentForm != null)
            {
                parentForm.SizeChanged -= ParentForm_SizeChanged;
                parentForm.LocationChanged -= ParentForm_LocationChanged;
                parentForm = null;
            }

            base.OnFormClosing(e);
        }

       
    }
}