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
        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            private set
            {
                _products = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalInventoryValue));
            }
        }

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

        public ProductsViewModel(List<Product> initialProducts)
        {
            Products = initialProducts ?? new List<Product>();

            AddProductCommand = new Command(AddProduct);
            RemoveProductCommand = new Command(RemoveProduct);
            UpdateProductCommand = new Command(UpdateProduct);
        }

        private void AddProduct()
        {
            var newProduct = new Product
            {
                Name = NewProductName,
                Price = NewProductPrice,
                Quantity = NewProductQuantity,
                LowStockThreshold = NewProductLowThreshold
            };

            Products = ProductService.AddProduct(Products, newProduct);
        }

        private void RemoveProduct()
        {
            if (SelectedProduct != null)
            {
                Products = ProductService.RemoveProduct(Products, SelectedProduct.Id);
            }
        }

        private void UpdateProduct()
        {
            if (SelectedProduct != null)
            {
                var updatedProduct = new Product
                {
                    Id = SelectedProduct.Id,
                    Name = NewProductName,
                    Price = NewProductPrice,
                    Quantity = NewProductQuantity,
                    LowStockThreshold = NewProductLowThreshold
                };

                Products = ProductService.UpdateProduct(Products, updatedProduct);
            }
        }

        public decimal TotalInventoryValue => ProductService.GetTotalInventoryValue(Products);

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
