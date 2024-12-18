using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementSystem.ViewModels;
namespace StockManagementSystem.Views
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage(ReportsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}

