using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StockManagementSystem.StockManagementSystem.DataLayer
{
    // This class tells EF how to create your DbContext at design time (for migrations)
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
            var connectionString = "Host=host.docker.internal;Port=5432;Database=stockdb;Username=postgres;Password=secret";
            optionsBuilder.UseNpgsql(connectionString);

            return new ProductDbContext(optionsBuilder.Options);
        }
    }
}
