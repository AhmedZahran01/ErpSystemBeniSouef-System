using DocumentFormat.OpenXml.Bibliography;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for PrintReceiptsInFullPage.xaml
    /// </summary>
    public partial class PrintReceiptsInFullPage : Page
    {
        private readonly IReceiptService _receiptService;
        private readonly IMainAreaService _mainAreaService;

        public PrintReceiptsInFullPage(IReceiptService receiptService, IMainAreaService mainAreaService)
        {
            InitializeComponent();
            _receiptService = receiptService;
            _mainAreaService = mainAreaService;
            Loaded += async (s, e) =>
            {
                try
                {
                    LoadingBar.Visibility = Visibility.Visible;
                    LoadingText.Visibility = Visibility.Visible;
                    LoadingText.Text = "جاري تحميل البيانات...";

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

        }

        #region load products to Grid Region

        private async Task LoadReceipts(int mainId = 4, int fr = 1, int to = 100000)
        {
            //(var receiptsData, var fileData) = await _receiptService.GetAllReceiptsAsync( 1,1, 1000000000);
            //(var receiptsData2, var fileData2) = await _receiptService.GetAllReceiptsAsync( 2,1, 1000000000);
            //(var receiptsData3, var fileData3) = await _receiptService.GetAllReceiptsAsync( 3,1, 1000000000);
            (var receiptsData4, var fileData4) = await _receiptService.GetAllReceiptsAsync(mainId, fr, to);
            //(var receiptsData5, var fileData5) = await _receiptService.GetAllReceiptsAsync( 5,1, 1000000000);
            //(var receiptsData6, var fileData6) = await _receiptService.GetAllReceiptsAsync( 6,1, 1000000000);
            ReceiptsDataGrid.ItemsSource = receiptsData4;

            var f = await _mainAreaService.GetAllAsync();
            MainAreaCombo.ItemsSource = f;

        }


        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new HomeReceiptsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private async void GetReceiptDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(FromClient.Text, out int fromclient) ||
                !int.TryParse(ToClient.Text, out int toclient) ||
                fromclient <= 0 || toclient <= 0 || fromclient > toclient)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            if (MainAreaCombo.SelectedItem is not MainAreaDto selectedCategory)
            {
                MessageBox.Show("من فضلك اختر المنطقة");
                return;
            }

            await LoadReceipts(selectedCategory.Id, fromclient, toclient);

        }

    }
}
