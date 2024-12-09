using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementSystem.StockManagementSystem.DataLayer.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
