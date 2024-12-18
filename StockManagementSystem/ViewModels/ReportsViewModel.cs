using System.Windows.Input;
using StockManagementSystem.Services;
using StockManagementSystem.StockManagementSystem.DataLayer.Models;
using OfficeOpenXml;
using System.Diagnostics;

namespace StockManagementSystem.ViewModels
{
    public class ReportsViewModel
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ICommand GenerateLowStockReportCommand { get; }
        public ICommand GenerateTotalSalesReportCommand { get; }
        public ICommand GenerateInventoryValueReportCommand { get; }
        public ReportsViewModel(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
            GenerateLowStockReportCommand = new Command(GenerateLowStockReport);
            GenerateTotalSalesReportCommand = new Command(GenerateTotalSalesReport);
            GenerateInventoryValueReportCommand = new Command(GenerateInventoryValueReport);

        }

        private async void GenerateLowStockReport()
        {
            try
            {
                var lowStockItems = _productService
                    .GetAllProducts()
                    .Where(p => p.IsLowStock)
                    .ToList();
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
                var orders = _orderService.GetAllOrders().ToList();

                if (orders.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No Orders", "There are no orders to generate a report.", "OK");
                    return;
                }

                var filePath = await GenerateTotalSalesExcelReport(orders);

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
                var products = _productService.GetAllProducts().ToList();

                if (products.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No Products", "There are no products to generate a report.", "OK");
                    return;
                }

                var filePath = await GenerateInventoryValueExcelReport(products);

                await Application.Current.MainPage.DisplayAlert("Report Generated", $"The report has been saved to:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        private async Task<string> GenerateExcelReport(List<Product> lowStockItems)
        {
            var fileName = Path.Combine("C:\\Users\\Ayaas\\OneDrive\\Desktop", "LowStockReport.xlsx");
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Low Stock Items");

            worksheet.Cells[1, 2].Value = "Product Name";
            worksheet.Cells[1, 3].Value = "Quantity";

            for (int i = 0; i < lowStockItems.Count; i++)
            {
                var product = lowStockItems[i];
                worksheet.Cells[i + 2, 2].Value = product.Name;
                worksheet.Cells[i + 2, 3].Value = product.Quantity;
            }

            await package.SaveAsAsync(new FileInfo(fileName));

            return fileName;
        }
        private async Task<string> GenerateTotalSalesExcelReport(List<Order> orders)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var fileName = Path.Combine("C:\\Users\\Ayaas\\OneDrive\\Desktop", $"TotalSalesReport_{today}.xlsx");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Total Sales");

            worksheet.Cells[1, 1].Value = "Order ID";
            worksheet.Cells[1, 2].Value = "Product ID";
            worksheet.Cells[1, 3].Value = "Quantity Ordered";
            worksheet.Cells[1, 4].Value = "Total Cost";
            worksheet.Cells[1, 5].Value = "Order Date";

            decimal totalSales = 0;

            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                worksheet.Cells[i + 2, 1].Value = order.Id;
                worksheet.Cells[i + 2, 2].Value = order.ProductId;
                worksheet.Cells[i + 2, 3].Value = order.QuantityOrdered;
                worksheet.Cells[i + 2, 4].Value = "$" + order.TotalCost;
                worksheet.Cells[i + 2, 5].Value = order.OrderDate.ToString("yyyy-MM-dd");

                totalSales += order.TotalCost;
            }

            var totalRow = orders.Count + 2;
            worksheet.Cells[totalRow, 1].Value = "Total Sales";
            worksheet.Cells[totalRow, 1].Style.Font.Bold = true;
            worksheet.Cells[totalRow, 2].Value = "$" + totalSales;

            await package.SaveAsAsync(new FileInfo(fileName));

            return fileName;
        }

        private async Task<string> GenerateInventoryValueExcelReport(List<Product> products)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var fileName = Path.Combine("C:\\Users\\Ayaas\\OneDrive\\Desktop", $"InventoryValueReport_{today}.xlsx");
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Inventory Value");

            worksheet.Cells[1, 1].Value = "Product Name";
            worksheet.Cells[1, 2].Value = "Price";
            worksheet.Cells[1, 3].Value = "Quantity";
            worksheet.Cells[1, 4].Value = "Inventory Value";

            decimal totalInventoryValue = 0;

            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                var inventoryValue = product.Price * product.Quantity;

                worksheet.Cells[i + 2, 1].Value = product.Name;
                worksheet.Cells[i + 2, 2].Value = "$" + product.Price;
                worksheet.Cells[i + 2, 3].Value = product.Quantity;
                worksheet.Cells[i + 2, 4].Value = inventoryValue;

                totalInventoryValue += inventoryValue;
            }

            var totalRow = products.Count + 2;
            worksheet.Cells[totalRow, 1].Value = "Total Inventory Value:";
            worksheet.Cells[totalRow, 1].Style.Font.Bold = true;
            worksheet.Cells[totalRow, 2].Value = "$" + totalInventoryValue;

            await package.SaveAsAsync(new FileInfo(fileName));

            return fileName;
        }
    }
}
