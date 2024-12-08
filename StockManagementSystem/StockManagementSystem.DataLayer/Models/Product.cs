using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementSystem.StockManagementSystem.DataLayer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public Decimal Price { get; set; }
        public int Quantity { get; set; }
        public int LowStockThreshold { get; set; }
        public bool IsLowStock => Quantity < LowStockThreshold;
    }
}
