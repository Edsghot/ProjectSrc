using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.general;

namespace PknoPlusCS.Forms.DialogView
{
    public partial class ErrorImportacion : Form
    {
        public ErrorImportacion(GenericErrorsDto errores, string codigoCompra)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            lblCodigo.Text = codigoCompra;

            ConfigurarListView(listViewErroresCompra);
            ConfigurarListView(listViewErroresDetalle);
            LlenarListViewErrores(errores);
        }

        private void ConfigurarListView(ListView listView)
        {
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.MultiSelect = false;
            listView.Scrollable = true;

            listView.Columns.Add("Campo", 100);
            listView.Columns.Add("Mensaje", 250);
            listView.Columns.Add("Detalle", 250);
        }

        private void LlenarListViewErrores(GenericErrorsDto errores)
        {
            listViewErroresCompra.Items.Clear();
            listViewErroresDetalle.Items.Clear();

            if (errores.HeaderError.Any())
            {
                foreach (var error in errores.HeaderError)
                {
                    ListViewItem item = new ListViewItem(error.Field.ToString() ?? "-");
                    item.SubItems.Add(error.Message ?? "-");
                    item.SubItems.Add(error.Detail ?? "-");
                    item.BackColor = Color.LightGoldenrodYellow; 
                    listViewErroresCompra.Items.Add(item);
                }
            }

            if (errores.ErrorDetail.Any())
            {
                foreach (var error in errores.ErrorDetail)
                {
                    ListViewItem item = new ListViewItem(error.Field.ToString() ?? "-");
                    item.SubItems.Add(error.Message ?? "-");
                    item.SubItems.Add(error.Detail ?? "-");
                    item.BackColor = Color.LightGray; 
                    listViewErroresDetalle.Items.Add(item);
                }
            }

            AjustarListView(listViewErroresCompra);
            AjustarListView(listViewErroresDetalle);
        }

        private void AjustarListView(ListView listView)
        {
            if (listView.Items.Count > 0)
            {
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
