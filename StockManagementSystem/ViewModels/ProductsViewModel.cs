using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;
using StockManagementSystem.Services;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;

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
            
            AddProductCommand = new Command(AddProduct);
            RemoveProductCommand = new Command(RemoveProduct);
            UpdateProductCommand = new Command(UpdateProduct);
        }

        private void AddProduct()
        {
            Debug.WriteLine("AddProduct");
            var product = new Product
            {
                Name = NewProductName,
                Price = NewProductPrice,
                Quantity = NewProductQuantity,
                LowStockThreshold = NewProductLowThreshold
            };
            _productService.AddProduct(product);

            RefreshProducts();
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
                SelectedProduct.Name = NewProductName;
                SelectedProduct.Price = NewProductPrice;
                SelectedProduct.Quantity = NewProductQuantity;
                SelectedProduct.LowStockThreshold = NewProductLowThreshold;
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
