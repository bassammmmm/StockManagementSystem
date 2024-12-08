using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;

namespace StockManagementSystem.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(int productId);
        decimal GetTotalInventoryValue();
    }
}
