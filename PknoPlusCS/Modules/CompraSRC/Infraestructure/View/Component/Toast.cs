using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

public class Toast : UserControl
{
    private Timer closeTimer;
    private Label messageLabel;
    private PictureBox iconPictureBox;

    public Toast(string message, string type)
    {
        InitializeToast(message, type);
    }

    private void InitializeToast(string message, string type)
    {
        this.BackColor = GetBackgroundColor(type);
        this.Size = new Size(600, 50);
        this.Padding = new Padding(10);

        iconPictureBox = new PictureBox
        {
            Image = GetIcon(type),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Size = new Size(30, 30),
            Location = new Point(10, (this.Height - 30) / 2),
        };

        messageLabel = new Label
        {
            Text = message,
            ForeColor = Color.White,
            Font = new Font("Arial", 10, FontStyle.Regular),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(40, 0, 10, 0),
        };

        this.Controls.Add(iconPictureBox);
        this.Controls.Add(messageLabel);

        closeTimer = new Timer
        {
            Interval = 3000,
        };
        closeTimer.Tick += (s, e) => this.HideToast();
        closeTimer.Start();
    }

    private Color GetBackgroundColor(string type)
    {
        switch (type.ToLower())
        {
            case "success":
                return Color.FromArgb(52, 181, 116);
            case "warning":
                return Color.FromArgb(255, 193, 7);
            case "error":
                return Color.FromArgb(220, 53, 69);
            case "info":
                return Color.FromArgb(23, 162, 184);
            default:
                return Color.Gray;
        }
    }

    private Image GetIcon(string type)
    {
        switch (type.ToLower())
        {
            case "success":
                return IconChar.CheckCircle.ToBitmap(IconFont.Auto, 30, Color.White);
            case "warning":
                return IconChar.ExclamationTriangle.ToBitmap(IconFont.Auto, 30, Color.White);
            case "error":
                return IconChar.TimesCircle.ToBitmap(IconFont.Auto, 30, Color.White);
            case "info":
                return IconChar.InfoCircle.ToBitmap(IconFont.Auto, 30, Color.White);
            default:
                return IconChar.QuestionCircle.ToBitmap(IconFont.Auto, 30, Color.White);
        }
    }

    private void HideToast()
    {
        closeTimer.Stop();
        this.Dispose();
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // Toast
            // 
            this.Name = "Toast";
            this.Load += new System.EventHandler(this.Toast_Load);
            this.ResumeLayout(false);

    }

    private void Toast_Load(object sender, System.EventArgs e)
    {

    }
}

