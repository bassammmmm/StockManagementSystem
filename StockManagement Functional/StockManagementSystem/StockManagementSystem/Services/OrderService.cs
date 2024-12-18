using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;

namespace StockManagementSystem.Services
{
    public static class OrderService
    {
        // Place an order immutably
        public static (List<Order> orders, List<Product> products) PlaceOrder(
            List<Order> orders, List<Product> products, int productId, int quantity)
        {
            var (foundProduct, updatedProducts) = FindProductById(products, productId, new List<Product>(), 0);

            if (foundProduct == null || foundProduct.Quantity < quantity)
                throw new InvalidOperationException("Not enough stock or product not found.");

            var updatedProduct = foundProduct with { Quantity = foundProduct.Quantity - quantity };

            // Update products immutably
            updatedProducts = UpdateProductInList(updatedProducts, updatedProduct, new List<Product>(), 0);

            // Add new order immutably
            var newOrder = new Order
            {
                ProductId = productId,
                QuantityOrdered = quantity,
                TotalCost = foundProduct.Price * quantity,
                OrderDate = DateTime.UtcNow
            };

            var updatedOrders = AddOrderToList(orders, newOrder, new List<Order>(), 0);

            return (updatedOrders, updatedProducts);
        }

        // Get all orders (returns the list as-is)
        public static List<Order> GetAllOrders(List<Order> orders) => orders ?? new List<Order>();

        // Find a product immutably using recursion
        private static (Product? product, List<Product>) FindProductById(
            List<Product> products, int productId, List<Product> accumulator, int index)
        {
            if (index == products.Count)
                return (null, accumulator);

            if (products[index].Id == productId)
                return (products[index], AppendToList(accumulator, products, index));

            return FindProductById(products, productId, AppendToList(accumulator, products[index]), index + 1);
        }

        // Update a product in the list immutably using recursion
        private static List<Product> UpdateProductInList(
            List<Product> products, Product updatedProduct, List<Product> accumulator, int index)
        {
            if (index == products.Count)
                return accumulator;

            var currentProduct = products[index];
            var nextAccumulator = AppendToList(accumulator, currentProduct.Id == updatedProduct.Id ? updatedProduct : currentProduct);

            return UpdateProductInList(products, updatedProduct, nextAccumulator, index + 1);
        }

        // Add a new order immutably using recursion
        private static List<Order> AddOrderToList(
            List<Order> orders, Order newOrder, List<Order> accumulator, int index)
        {
            if (index == orders.Count)
                return AppendToList(accumulator, newOrder);

            return AddOrderToList(orders, newOrder, AppendToList(accumulator, orders[index]), index + 1);
        }

        // Helper method to append an item to the list immutably
        private static List<T> AppendToList<T>(List<T> list, T item)
        {
            var newList = new List<T>(list) { item };
            return newList;
        }

        // Helper method to append remaining items immutably
        private static List<T> AppendToList<T>(List<T> list, List<T> items, int startIndex)
        {
            var newList = new List<T>(list);
            for (int i = startIndex; i < items.Count; i++)
            {
                newList.Add(items[i]);
            }
            return newList;
        }
    }
}
