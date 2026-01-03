using DocumentFormat.OpenXml.Bibliography;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.Receipt;
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
            List<GetAllReceiptsDto> receiptsData4 = new List<GetAllReceiptsDto>();
            if (mainId != 4)
            {
                (receiptsData4, var fileData4) = await _receiptService.GetAllReceiptsAsync(mainId, fr, to);
            }
            else
            {
                (receiptsData4, var fileData4) = await _receiptService.GetAllReceiptsAsync();
                var f = await _mainAreaService.GetAllAsync();
                MainAreaCombo.ItemsSource = f;
                MainAreaCombo.SelectedIndex = 0;

            }
            int index = 1;
            foreach (var item in receiptsData4)
            {
                item.DisplayUIId = index++;
            }

            ReceiptsDataGrid.ItemsSource = receiptsData4;


        }


        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new HomeReceiptsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private async void GetReceiptDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var fromClientVariable = int.TryParse(FromClient.Text, out int fromclient);
            var ToClientVariable = int.TryParse(ToClient.Text, out int toclient);
            if (!fromClientVariable || !ToClientVariable ||
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
