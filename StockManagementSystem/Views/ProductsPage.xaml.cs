using StockManagementSystem.ViewModels;

namespace StockManagementSystem.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage(ProductsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
