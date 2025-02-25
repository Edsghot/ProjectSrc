using System;
using System.Drawing;
using System.Windows.Forms;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using FontAwesome.Sharp;

namespace app_matter_data_src_erp.Forms.DialogView.DialogModal
{
    public partial class DialogModal : Form
    {
        private string modalType;
        private string idRecepcion;
        private string codigo; 
        private string rucRecuperado;
        private EditarCompra parentForm;
        public DialogModal(string title, string subtitle, string type,string code,string id,string ruc, EditarCompra parent = null)
        {
                  
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.parentForm= parent;
            lblTitle.Text = title;
            lblSubtitle.Text = subtitle;    
            modalType = type.ToLower();
            this.idRecepcion = id;
            this.codigo = code;
            this.rucRecuperado = ruc;
            switch (modalType)
            {
                case "warning":
                    iconPicture.IconChar = IconChar.CircleInfo;
                    iconPicture.IconColor = Color.Red;
                    lblTitle.ForeColor = Color.Red;
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
                    ControlStatic.CierreModalEditar = false;
                    EditarCompra warningModal = new EditarCompra(codigo,idRecepcion, rucRecuperado, (MainComprasSrc)this.ParentForm);
                    warningModal.TopMost = true;
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

        private void DialogVIew_Activated(object sender, EventArgs e)
        {
            if (ControlStatic.CierreDIalogvIew)
            {
                this.Dispose();
            }
        }
    }
}
