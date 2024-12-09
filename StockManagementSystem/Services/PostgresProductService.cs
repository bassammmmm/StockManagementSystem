using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;
using StockManagementSystem.StockManagementSystem.DataLayer;
using System.Diagnostics;

namespace StockManagementSystem.Services
{
    public class PostgresProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        public PostgresProductService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }
        public List<Product> GetAllProducts()
        {
            // Fetch all products from the database
            return _dbContext.Products.ToList();
        }
        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            var existing = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                existing.LowStockThreshold = product.LowStockThreshold;
                _dbContext.SaveChanges();
            }
        }
        public void RemoveProduct(int productId)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
        }
        public decimal GetTotalInventoryValue()
        {
            return _dbContext.Products
                .AsNoTracking()
                .Sum(p => p.Price * p.Quantity);
        }
    }
}
