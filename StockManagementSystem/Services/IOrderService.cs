using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;


namespace StockManagementSystem.Services
{
    public interface IOrderService
    {
        Order PlaceOrder(int productId, int quantity);
        IEnumerable<Order> GetAllOrders();
    }

}
