using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using app_matter_data_src_erp.Forms.DialogView.ProductMatch;
using FontAwesome.Sharp;

namespace app_matter_data_src_erp.Forms.DialogView.DialogModal
{
    public partial class DialogModal : Form
    {
        private string modalType;
        private string code;
        private EditarCompra parentForm;
        public DialogModal(string title, string subtitle, string type, string optionalCode = null, EditarCompra parent = null)
        {
                  
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.parentForm= parent;
            lblTitle.Text = title;
            lblSubtitle.Text = subtitle;
            code = optionalCode;
            modalType = type.ToLower(); 

            switch (modalType)
            {
                case "warning":
                    iconPicture.IconChar = IconChar.CircleInfo;
                    iconPicture.IconColor = Color.Navy;
                    lblTitle.ForeColor = Color.Navy;
                    break;
                case "question":
                    iconPicture.IconChar = IconChar.CircleQuestion;
                    iconPicture.IconColor = Color.Red;
                    lblTitle.ForeColor = Color.Red;
                    break;
                case "success":
                    iconPicture.IconChar = IconChar.CircleCheck;
                    iconPicture.IconColor = Color.Green;
                    lblTitle.ForeColor = Color.Green;
                    break;
                default:
                    throw new ArgumentException("Tipo desconocido. Usa 'warning', 'question' o 'success'.");
            }

            iconPicture.IconSize = 74;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            switch (modalType)
            {
                case "warning":
                    this.Hide();
                    EditarCompra warningModal = new EditarCompra(code, (Main)this.ParentForm);
                    warningModal.ShowDialog();
                    break;
                case "question":
                    parentForm.Close();
                    this.Close();
                    break;
                default:
                    MessageBox.Show("Tipo no soportado para continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
