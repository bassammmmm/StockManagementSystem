using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using StockManagementSystem.Services;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;

namespace StockManagementSystem.ViewModels
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        private Product _selectedProduct;
        private int _orderQuantity;
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();

        public event PropertyChangedEventHandler PropertyChanged;

        public OrdersViewModel()
        {
            // Initialize orders and products safely
            Orders = new ObservableCollection<Order>(
                OrderService.GetAllOrders(new List<Order>())
            );

            RefreshProducts();

            PlaceOrderCommand = new Command(PlaceOrder);
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set { _orders = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        public int OrderQuantity
        {
            get => _orderQuantity;
            set { _orderQuantity = value; OnPropertyChanged(); }
        }

        public ICommand PlaceOrderCommand { get; }

        private async void PlaceOrder()
        {
            try
            {
                if (SelectedProduct == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Problem", "Please select a product", "OK");
                    return;
                }

                if (OrderQuantity <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Problem", "Quantity must be greater than 0", "OK");
                    return;
                }

                var totalCost = SelectedProduct.Price * OrderQuantity;

                var isConfirmed = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Order",
                    $"Are you sure you want to place an order for {OrderQuantity} of {SelectedProduct.Name} for ${totalCost}?",
                    "Yes", "No");

                if (!isConfirmed) return;

                // Place the order immutably
                var (updatedOrders, updatedProducts) = OrderService.PlaceOrder(
                    _orders.ToList(), _products.ToList(), SelectedProduct.Id, OrderQuantity);

                Orders = new ObservableCollection<Order>(updatedOrders);
                Products = new ObservableCollection<Product>(updatedProducts);

                SelectedProduct = null;
                OrderQuantity = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error placing order: {ex.Message}");
            }
        }

        public void RefreshProducts()
        {
            Products = new ObservableCollection<Product>(
                ProductService.GetAllProducts(new List<Product>())
            );
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
