using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.Services;
using StockManagementSystem.ViewModels;
using StockManagementSystem.Views;
using StockManagementSystem.StockManagementSystem.DataLayer;
using OfficeOpenXml;

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

            builder.Services.AddDbContext<AppDbContext>(options=>options.UseNpgsql(connectionString));

            // Replace InMemoryProductService with PostgresProductService

            builder.Services.AddTransient<ProductsViewModel>(); // Transient means a new instance each time
            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<OrdersViewModel>();
            builder.Services.AddTransient<OrdersPage>();
            builder.Services.AddTransient<ReportsViewModel>();
            builder.Services.AddTransient<ReportsPage>();

            // Build the MauiApp
            var app = builder.Build();

            // Apply pending migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate(); // Apply EF Core migrations to the database
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            return app;
        }
    }
}
