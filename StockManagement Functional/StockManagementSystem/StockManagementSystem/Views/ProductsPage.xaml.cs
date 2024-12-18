using StockManagementSystem.ViewModels;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;
namespace StockManagementSystem.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage(ProductsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = new ProductsViewModel(new List<Product>()); // Pass an empty list initially
        }
    }
}
