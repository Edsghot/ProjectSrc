using System.Drawing;
using System.Windows.Forms;

public partial class LoadingForm : Form
{
    public LoadingForm()
    {
   
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;

        Label lblLoading = new Label
        {
            Text = "Cargando...",
            Font = new Font("Arial", 14, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(50, 30)
        };

        ProgressBar progressBar = new ProgressBar
        {
            Style = ProgressBarStyle.Marquee,
            Size = new Size(200, 20),
            Location = new Point(50, 60)
        };

        this.Controls.Add(lblLoading);
        this.Controls.Add(progressBar);
        this.Size = new Size(700, 500);
    }
}
