using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Service.MainAreaServices;
using ErpSystemBeniSouef.Service.ReceiptServices;
using ErpSystemBeniSouef.Service.SubAreaServices;
using ErpSystemBeniSouef.ViewModel;
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

namespace ErpSystemBeniSouef.Views.Pages.ReceiptsRegion.ReceiptsPages
{
    /// <summary>
    /// Interaction logic for Monthly_Receipts_AccordingToTheCollector.xaml
    /// </summary>
    public partial class Monthly_Receipts_AccordingToTheCollector : Page
    {
        private readonly IReceiptService _receiptService; 

        public Monthly_Receipts_AccordingToTheCollector(IReceiptService receiptService)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                await LoadReceipts();
            };
            _receiptService = receiptService; 
        }

        #region load products to Grid Region

        private async Task LoadReceipts(int mainId = 4, int fr = 1, int to = 100000)
        {
            DateTime monthDate = DateTime.UtcNow;
            //(var receiptsData4, var fileData4) = await _receiptService.GetCollectorReceiptsAsync(monthDate, 2);
            //(var receiptsData42, var fileData24) = await _receiptService.GetCollectorReceiptsAsync(monthDate, 3);
            (var receiptsData14, var fileData41) = await _receiptService.GetCollectorReceiptsAsync(monthDate, 4);
            (var receiptsData422, var fileData434) = await _receiptService.GetCollectorReceiptsAsync(monthDate,5);
            //(var receiptsData4221, var fileData44) = await _receiptService.GetCollectorReceiptsAsync(monthDate, 6);
            //(var receiptsData4222, var fileData45) = await _receiptService.GetCollectorReceiptsAsync(monthDate, 7);
            
            RepresentativeDataGridReceipts.ItemsSource = receiptsData14;
             

        }

        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new HomeReceiptsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }
    }
}
