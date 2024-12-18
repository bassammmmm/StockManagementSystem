using StockManagementSystem.StockManagementSystem.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementSystem.Services
{
    public static class ProductService
    {
        // Get all products (returns the same list as-is since it's immutable)
        public static List<Product> GetAllProducts(List<Product> products) => products ?? new List<Product>();

        public static List<Product> AddProduct(List<Product> products, Product product)
        {
            if (products == null) products = new List<Product>();
            if (product == null) throw new ArgumentNullException(nameof(product));

            var newProducts = new List<Product>();
            AddProductRecursively(products, product, newProducts, 0);
            return newProducts;
        }

        private static void AddProductRecursively(
            List<Product> products, Product product, List<Product> newProducts, int index)
        {
            if (index == products.Count)
            {
                newProducts.Add(product);
                return;
            }

            newProducts.Add(products[index]);
            AddProductRecursively(products, product, newProducts, index + 1);
        }

        // Update a product immutably
        public static List<Product> UpdateProduct(List<Product> products, Product updatedProduct)
        {
            var newProducts = new List<Product>();
            UpdateProductRecursively(products, updatedProduct, newProducts, 0);
            return newProducts;
        }

        private static void UpdateProductRecursively(
            List<Product> products, Product updatedProduct, List<Product> newProducts, int index)
        {
            if (index == products.Count)
                return;

            if (products[index].Id == updatedProduct.Id)
                newProducts.Add(updatedProduct);
            else
                newProducts.Add(products[index]);

            UpdateProductRecursively(products, updatedProduct, newProducts, index + 1);
        }

        // Remove a product immutably
        public static List<Product> RemoveProduct(List<Product> products, int productId)
        {
            if (products == null) return new List<Product>();

            var newProducts = new List<Product>();
            RemoveProductRecursively(products, productId, newProducts, 0);
            return newProducts;
        }

        private static void RemoveProductRecursively(
            List<Product> products, int productId, List<Product> newProducts, int index)
        {
            if (index == products.Count)
                return;

            if (products[index].Id != productId)
                newProducts.Add(products[index]);

            RemoveProductRecursively(products, productId, newProducts, index + 1);
        }

        // Calculate total inventory value
        public static decimal GetTotalInventoryValue(List<Product> products)
        {
            if (products == null) return 0;

            return CalculateInventoryValueRecursively(products, 0, 0);
        }

        private static decimal CalculateInventoryValueRecursively(
            List<Product> products, int index, decimal currentTotal)
        {
            if (index == products.Count)
                return currentTotal;

            var product = products[index];
            return CalculateInventoryValueRecursively(
                products, index + 1, currentTotal + (product.Price * product.Quantity));
        }
    }
}
