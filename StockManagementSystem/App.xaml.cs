using StockManagementSystem.Views;

namespace StockManagementSystem
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Resolve the ProductsPage from the DI container.
            MainPage = new NavigationPage(serviceProvider.GetService<ProductsPage>());
        }
    }

}
