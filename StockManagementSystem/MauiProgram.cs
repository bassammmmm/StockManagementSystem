using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.Services;
using StockManagementSystem.ViewModels;
using StockManagementSystem.Views;
using StockManagementSystem.StockManagementSystem.DataLayer;

namespace StockManagementSystem
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            var connectionString = "Host=localhost;Port=5432;Database=stockdb;Username=postgres;Password=secret";

            builder.Services.AddDbContext<ProductDbContext>(options=>options.UseNpgsql(connectionString));

            // Replace InMemoryProductService with PostgresProductService
            builder.Services.AddScoped<IProductService, PostgresProductService>();
            builder.Services.AddTransient<ProductsViewModel>(); // Transient means a new instance each time
            builder.Services.AddTransient<ProductsPage>();

            return builder.Build();
        }
    }
}
