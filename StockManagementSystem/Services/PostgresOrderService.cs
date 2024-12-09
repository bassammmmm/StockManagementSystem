using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementSystem.StockManagementSystem.DataLayer;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;

namespace StockManagementSystem.Services
{
    public class PostgresOrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductService _productService;

        public PostgresOrderService(AppDbContext dbContext, IProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }

        public Order PlaceOrder(int productId, int quantity)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null || product.Quantity < quantity)
            {
                throw new InvalidOperationException("Not enough stock or product not found.");
            }
            var totalCost = product.Price * quantity;

            product.Quantity -= quantity;
            _dbContext.Products.Update(product);

            var order = new Order
            {
                ProductId = productId,
                QuantityOrdered = quantity,
                TotalCost = totalCost,
                OrderDate = DateTime.UtcNow
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _dbContext.Orders.ToList();
        }
    }
}
