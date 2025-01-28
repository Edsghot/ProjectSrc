using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView.ProductMatch
{
    public partial class BuscarProducto : Form
    {
        private readonly ICompraSrcRepository _repo;
        private string _selectedProduct;

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
            lstResults.SelectedIndexChanged += LstResults_SelectedIndexChanged;
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
                MessageBox.Show("Por favor, ingrese un término de búsqueda.");
                return;
            }

            lstResults.Items.Clear();

            try
            {
                if (cmbSearchOption.SelectedItem.ToString() == "Nombre")
                {
                    var response = await _repo.searchProducts(searchQuery);

                    if (response != null && response.Any())
                    {
                        foreach (var product in response)
                        {
                            lstResults.Items.Add($"{product.ProductName} ! {product.ProductId}");
                        }
                    }
                    else
                    {
                        lstResults.Items.Add("No se encontraron productos con ese nombre.");
                    }
                }
                else if (cmbSearchOption.SelectedItem.ToString() == "ID de producto")
                {
                    var response = await _repo.BuscarProductoPorId(searchQuery);

                    if (response != null && response.Any())
                    {
                        foreach (var product in response)
                        {
                            lstResults.Items.Add($"{product.ProductName} !  {product.ProductId}");
                        }
                    }
                    else
                    {
                        lstResults.Items.Add("No se encontró producto con ese ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al buscar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstResults.SelectedItem != null)
            {
                var selectedProduct = lstResults.SelectedItem.ToString();
                _productName = selectedProduct;
                _productId = selectedProduct.Split('!')[1].Trim();

                MessageBox.Show($"Producto seleccionado: {_productName}", "Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        public string GetSelectedProduct()
        {
            return _productName;
        }
    }
}
