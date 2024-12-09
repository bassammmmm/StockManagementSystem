using Microsoft.EntityFrameworkCore;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;

namespace StockManagementSystem.StockManagementSystem.DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}