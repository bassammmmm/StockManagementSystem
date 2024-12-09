using StockManagementSystem.Views;

namespace StockManagementSystem
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }

}
