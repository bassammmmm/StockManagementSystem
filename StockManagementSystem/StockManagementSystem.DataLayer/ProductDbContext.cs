using Microsoft.EntityFrameworkCore;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;

namespace StockManagementSystem.StockManagementSystem.DataLayer
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}