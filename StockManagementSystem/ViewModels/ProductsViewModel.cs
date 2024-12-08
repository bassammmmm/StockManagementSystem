using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;
using StockManagementSystem.Services;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace StockManagementSystem.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private readonly IProductService _productService;

        public ObservableCollection<Product> Products { get; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        // Properties for adding/updating a product
        public int NewProductId { get; set; }
        public string NewProductName { get; set; } = string.Empty;
        public decimal NewProductPrice { get; set; }
        public int NewProductQuantity { get; set; }
        public int NewProductLowThreshold { get; set; }

        public ICommand AddProductCommand { get; }
        public ICommand RemoveProductCommand { get; }
        public ICommand UpdateProductCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
            Products = new ObservableCollection<Product>(_productService.GetAllProducts());
            
            // Commands: OOP encapsulating actions as object methods.
            AddProductCommand = new Command(AddProduct);
            RemoveProductCommand = new Command(RemoveProduct);
            UpdateProductCommand = new Command(UpdateProduct);
        }

        private void AddProduct()
        {
            var product = new Product
            {
                Id = NewProductId,
                Name = NewProductName,
                Price = NewProductPrice,
                Quantity = NewProductQuantity,
                LowStockThreshold = NewProductLowThreshold
            };
            _productService.AddProduct(product);

            // Refresh the UI by reloading products.
            RefreshProducts();
            ClearNewProductFields();
        }

        private void RemoveProduct()
        {
            if (SelectedProduct != null)
            {
                _productService.RemoveProduct(SelectedProduct.Id);
                RefreshProducts();
            }
        }

        private void UpdateProduct()
        {
            if (SelectedProduct != null)
            {
                // Maybe we use the SelectedProduct fields directly.
                _productService.UpdateProduct(SelectedProduct);
                RefreshProducts();
            }
        }

        public decimal TotalInventoryValue => _productService.GetTotalInventoryValue();

        private void RefreshProducts()
        {
            Products.Clear();
            foreach (var p in _productService.GetAllProducts())
                Products.Add(p);

            OnPropertyChanged(nameof(TotalInventoryValue));
        }

        private void ClearNewProductFields()
        {
            NewProductId = 0;
            NewProductName = string.Empty;
            NewProductPrice = 0m;
            NewProductQuantity = 0;
            NewProductLowThreshold = 0;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
