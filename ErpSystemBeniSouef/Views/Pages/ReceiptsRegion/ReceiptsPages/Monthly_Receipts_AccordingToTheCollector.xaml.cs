using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
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
        private readonly IRepresentativeService _representativeService;

        public Monthly_Receipts_AccordingToTheCollector(IReceiptService receiptService, IRepresentativeService representativeService)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                try
                {
                    LoadingBar.Visibility = Visibility.Visible;
                    LoadingText.Visibility = Visibility.Visible;
                    LoadingText.Text = "جاري تحميل البيانات...";
                    await Task.Delay(2000);

                    await LoadReceipts();

                    LoadingBar.Visibility = Visibility.Collapsed;
                    LoadingText.Text = "تم تحميل البيانات بنجاح";
                    await Task.Delay(3000);
                    LoadingText.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    LoadingText.Text = "حدث خطأ أثناء تحميل البيانات";
                }
            };
            _receiptService = receiptService;
            _representativeService = representativeService;
        }

        #region load products to Grid Region

        private async Task LoadReceipts(int mainId = 4, int fr = 1, int to = 100000)
        {
            DateTime monthDate = DateTime.UtcNow;
            (var receiptsData14, var fileData41) = await _receiptService.GetCollectorReceiptsAsync(monthDate);
            var rep = await _representativeService.GetAllAsync(); 
            RepresentativeDataGridReceipts.ItemsSource = receiptsData14;
            repComboBox.ItemsSource = rep;
            repComboBox.SelectedIndex = 0;
             
        }

        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new HomeReceiptsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime SearchDate = SearchMonthDate.SelectedDate ?? DateTime.UtcNow;
            RepresentativeDto rep = (RepresentativeDto)repComboBox.SelectedItem;
            if (rep is not null)
            {
                int repId = rep.Id;

                try
                {
                    LoadingBar.Visibility = Visibility.Visible;
                    LoadingText.Visibility = Visibility.Visible;
                    LoadingText.Text = "جاري تحميل البيانات...";
                    await Task.Delay(2000);

                    (var SearchreceiptsData, var SearchfileData) = await _receiptService.GetCollectorReceiptsAsync(SearchDate, repId);
                    RepresentativeDataGridReceipts.ItemsSource = SearchreceiptsData;

                    LoadingBar.Visibility = Visibility.Collapsed;
                    LoadingText.Text = "تم تحميل البيانات بنجاح";
                    await Task.Delay(3000);
                    LoadingText.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    LoadingText.Text = "حدث خطأ أثناء تحميل البيانات";
                }


            }


        }
    }
}
