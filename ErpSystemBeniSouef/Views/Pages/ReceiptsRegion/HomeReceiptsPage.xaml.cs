using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErpSystemBeniSouef.Views.Pages.ReceiptsRegion
{
    /// <summary>
    /// Interaction logic for HomeReceiptsPage.xaml
    /// </summary>
    public partial class HomeReceiptsPage : Page
    {
        public HomeReceiptsPage()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private void Print_Receipts_inFull_Click(object sender, RoutedEventArgs e)
        {
            var receiptService = App.AppHost.Services.GetRequiredService<IReceiptService>();
            var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var Dashboard = new ReceiptsPages.PrintReceiptsInFullPage(receiptService, mainAreaService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private void PrintMonthlyReceiptsByRegion_Click(object sender, RoutedEventArgs e)
        {
            var receiptService = App.AppHost.Services.GetRequiredService<IReceiptService>();
            var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var subAreaService = App.AppHost.Services.GetRequiredService<ISubAreaService>();
            var Dashboard = new ReceiptsPages.PrintMonthlyReceiptsByRegionPage(receiptService , mainAreaService , subAreaService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }
    }
}
