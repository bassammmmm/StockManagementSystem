using StockManagementSystem.ViewModels;

namespace StockManagementSystem.Views
{
    public partial class OrdersPage : ContentPage
    {
        private readonly OrdersViewModel _viewModel;

        public OrdersPage(OrdersViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.RefreshProducts();
        }
    }
}