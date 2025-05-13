using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PknoPlusCS.Forms.DialogView.ProductMatch
{
    public partial class BuscarProducto : Form
    {
        private readonly ICompraSrcRepository _repo;
        private string _productId;
        private string _productName;

        public string SelectedProductId => _productId;
        public string SelectedProductName => _productName;

        public BuscarProducto(string initialProductId = "", string initialProductName = "")
        {
            InitializeComponent();
            cmbSearchOption.SelectedItem = "Nombre";
            this.StartPosition = FormStartPosition.CenterScreen;
            SetPlaceholder(txtSearch, "¿Qué deseas buscar?");
            _repo = new CompraSrcRepository();

            txtSearch.Text = _productName;

            lvResults.View = View.Details;
            lvResults.FullRowSelect = true;
            lvResults.Columns.Clear();
            lvResults.Columns.Add("Código", 80);
            lvResults.Columns.Add("Nombre", 480);

            lvResults.SelectedIndexChanged += LvResults_SelectedIndexChanged;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchQuery) || searchQuery == "¿Qué deseas buscar?")
            {
                MessageBox.Show(@"Por favor, ingrese un término de búsqueda.");
                return;
            }

            lvResults.Items.Clear();

            try
            {
                if (cmbSearchOption.SelectedItem.ToString() == "Nombre")
                {
                    var response =  _repo.searchProducts(searchQuery);

                    if (response != null && response.Any())
                    {
                        foreach (var product in response)
                        {
                            ListViewItem item = new ListViewItem(product.ProductId);
                            item.SubItems.Add(product.ProductName);
                            lvResults.Items.Add(item);
                        }
                    }
                    else
                    {
                        lvResults.Items.Add(new ListViewItem(new[] { "N/A", "No se encontraron productos" }));
                    }
                }
                else if (cmbSearchOption.SelectedItem.ToString() == "ID de producto")
                {
                    var response =  _repo.BuscarProductoPorId(searchQuery);

                    if (response != null && response.Any())
                    {
                        foreach (var product in response)
                        {
                            ListViewItem item = new ListViewItem(product.ProductId);
                            item.SubItems.Add(product.ProductName);
                            lvResults.Items.Add(item);
                        }
                    }
                    else
                    {
                        lvResults.Items.Add(new ListViewItem(new[] { "N/A", "No se encontró producto con ese ID" }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al buscar el producto: {ex.Message}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvResults.SelectedItems.Count > 0)
            {
                var selectedItem = lvResults.SelectedItems[0];
                _productId = selectedItem.Text;
                _productName = selectedItem.SubItems[1].Text;

               
                this.Close();
            }
        }

        public string GetSelectedProduct()
        {
            return _productName;
        }
    }
}
