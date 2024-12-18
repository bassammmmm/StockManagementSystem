using System.Windows.Input;
using StockManagementSystem.Services;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;
using OfficeOpenXml;
using System.Diagnostics;

namespace StockManagementSystem.ViewModels
{
    public class ReportsViewModel
    {
        public ICommand GenerateLowStockReportCommand { get; }
        public ICommand GenerateTotalSalesReportCommand { get; }
        public ICommand GenerateInventoryValueReportCommand { get; }

        public ReportsViewModel()
        {
            GenerateLowStockReportCommand = new Command(GenerateLowStockReport);
            GenerateTotalSalesReportCommand = new Command(GenerateTotalSalesReport);
            GenerateInventoryValueReportCommand = new Command(GenerateInventoryValueReport);
        }

        private async void GenerateLowStockReport()
        {
            try
            {
                var products = ProductService.GetAllProducts(new List<Product>());
                var lowStockItems = FilterLowStockRecursively(products, new List<Product>(), 0);

                if (lowStockItems.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Info", "No Low Stock", "OK");
                    return;
                }

                var filePath = await GenerateExcelReport(lowStockItems);
                await Application.Current.MainPage.DisplayAlert("Report Generated", $"The report has been saved to:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void GenerateTotalSalesReport()
        {
            try
            {
                var orders = OrderService.GetAllOrders(new List<Order>());
                var filePath = await GenerateTotalSalesExcelReport(orders, 0, 0);

                await Application.Current.MainPage.DisplayAlert("Report Generated", $"The report has been saved to:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void GenerateInventoryValueReport()
        {
            try
            {
                var products = ProductService.GetAllProducts(new List<Product>());
                var filePath = await GenerateInventoryValueExcelReport(products, 0, 0);

                await Application.Current.MainPage.DisplayAlert("Report Generated", $"The report has been saved to:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        // Recursive filter for low stock
        private List<Product> FilterLowStockRecursively(List<Product> products, List<Product> result, int index)
        {
            if (index == products.Count) return result;

            var currentProduct = products[index];
            var newResult = currentProduct.IsLowStock ? AppendToList(result, currentProduct) : result;

            return FilterLowStockRecursively(products, newResult, index + 1);
        }

        // Generate Excel Report for Low Stock
        private async Task<string> GenerateExcelReport(List<Product> lowStockItems)
        {
            var fileName = Path.Combine("C:\\Users\\Ayaas\\OneDrive\\Desktop", "LowStockReport.xlsx");
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Low Stock Items");

            WriteProductRowsRecursively(worksheet, lowStockItems, 0);
            await package.SaveAsAsync(new FileInfo(fileName));

            return fileName;
        }

        private void WriteProductRowsRecursively(ExcelWorksheet worksheet, List<Product> products, int index)
        {
            if (index == products.Count) return;

            var product = products[index];
            worksheet.Cells[index + 2, 2].Value = product.Name;
            worksheet.Cells[index + 2, 3].Value = product.Quantity;

            WriteProductRowsRecursively(worksheet, products, index + 1);
        }

        // Generate Total Sales Report
        private async Task<string> GenerateTotalSalesExcelReport(List<Order> orders, int index, decimal totalSales)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var fileName = Path.Combine("C:\\Users\\Ayaas\\OneDrive\\Desktop", $"TotalSalesReport_{today}.xlsx");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Total Sales");

            WriteOrderRowsRecursively(worksheet, orders, index, totalSales);
            await package.SaveAsAsync(new FileInfo(fileName));

            return fileName;
        }

        private void WriteOrderRowsRecursively(ExcelWorksheet worksheet, List<Order> orders, int index, decimal totalSales)
        {
            if (index == orders.Count)
            {
                var totalRow = orders.Count + 2;
                worksheet.Cells[totalRow, 1].Value = "Total Sales";
                worksheet.Cells[totalRow, 1].Style.Font.Bold = true;
                worksheet.Cells[totalRow, 2].Value = "$" + totalSales;
                return;
            }

            var order = orders[index];
            worksheet.Cells[index + 2, 1].Value = order.Id;
            worksheet.Cells[index + 2, 2].Value = order.ProductId;
            worksheet.Cells[index + 2, 3].Value = order.QuantityOrdered;
            worksheet.Cells[index + 2, 4].Value = "$" + order.TotalCost;
            worksheet.Cells[index + 2, 5].Value = order.OrderDate.ToString("yyyy-MM-dd");

            WriteOrderRowsRecursively(worksheet, orders, index + 1, totalSales + order.TotalCost);
        }

        // Generate Inventory Value Report
        private async Task<string> GenerateInventoryValueExcelReport(List<Product> products, int index, decimal totalValue)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var fileName = Path.Combine("C:\\Users\\Ayaas\\OneDrive\\Desktop", $"InventoryValueReport_{today}.xlsx");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Inventory Value");

            WriteInventoryRowsRecursively(worksheet, products, index, totalValue);
            await package.SaveAsAsync(new FileInfo(fileName));

            return fileName;
        }

        private void WriteInventoryRowsRecursively(ExcelWorksheet worksheet, List<Product> products, int index, decimal totalValue)
        {
            if (index == products.Count)
            {
                var totalRow = products.Count + 2;
                worksheet.Cells[totalRow, 1].Value = "Total Inventory Value";
                worksheet.Cells[totalRow, 1].Style.Font.Bold = true;
                worksheet.Cells[totalRow, 2].Value = "$" + totalValue;
                return;
            }

            var product = products[index];
            var inventoryValue = product.Price * product.Quantity;

            worksheet.Cells[index + 2, 1].Value = product.Name;
            worksheet.Cells[index + 2, 2].Value = "$" + product.Price;
            worksheet.Cells[index + 2, 3].Value = product.Quantity;
            worksheet.Cells[index + 2, 4].Value = inventoryValue;

            WriteInventoryRowsRecursively(worksheet, products, index + 1, totalValue + inventoryValue);
        }

        // Helper to append an item immutably
        private List<T> AppendToList<T>(List<T> list, T item)
        {
            var newList = new List<T>(list) { item };
            return newList;
        }
    }
}
