using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.Receipt;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.ViewModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ErpSystemBeniSouef.Views.Pages.ReceiptsRegion.ReceiptsPages
{
    /// <summary>
    /// Interaction logic for PrintReceiptsInFullPage.xaml
    /// </summary>
    public partial class PrintReceiptsInFullPage : Page
    {
        private readonly IReceiptService _receiptService;
        private readonly IMainAreaService _mainAreaService;
        bool loadPage = true;
        List<GetAllReceiptsDto> getAllReceiptsDtos = new List<GetAllReceiptsDto>();

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
        }

        #region load products to Grid Region

        private async Task LoadReceipts(int mainId = 4, int fr = 1, int to = 100000)
        {
            List<GetAllReceiptsDto> receiptsData4 = new List<GetAllReceiptsDto>();
            if (!loadPage)
            {
                (receiptsData4, var fileData4) = await _receiptService.GetAllReceiptsAsync(mainId, fr, to);
            }
            else
            {
                (receiptsData4, var fileData4) = await _receiptService.GetAllReceiptsAsync();
                var mainAreas = await _mainAreaService.GetAllAsync();
                MainAreaCombo.ItemsSource = mainAreas;
                MainAreaCombo.SelectedIndex = 0;

            }
            int index = 1;
            foreach (var item in receiptsData4)
            {
                item.DisplayUIId = index++;
            }
            getAllReceiptsDtos = receiptsData4;
            ReceiptsDataGrid.ItemsSource = getAllReceiptsDtos;


        }


        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new HomeReceiptsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private async void GetReceiptDataBtn_Click(object sender, RoutedEventArgs e)
        {
            loadPage = false;

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




            #region MyRegion


            LoadingBar.Visibility = Visibility.Visible;
            LoadingText.Visibility = Visibility.Visible;
            LoadingText.Text = "جاري تحميل البيانات...";
            await Task.Delay(2000);

            await LoadReceipts(selectedCategory.Id, fromclient, toclient);

            LoadingBar.Visibility = Visibility.Collapsed;
            LoadingText.Text = "تم تحميل البيانات بنجاح";
            await Task.Delay(12000);
            LoadingText.Visibility = Visibility.Collapsed;

            #endregion
        }

        private void PrintDataReceiptBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedGridItems = ReceiptsDataGrid.SelectedItems.Count;

            if (selectedGridItems == 0)
            {
                // رسالة تأكيد
                var result = MessageBox.Show(
                "هل أنت متأكد أنك تريد طباعه الكل ؟",
                "تأكيد الطباعه",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

                if (result == MessageBoxResult.Yes)
                {
                    List<int> allReceiptsIds = getAllReceiptsDtos.Select(e => e.DisplayUIId).ToList();
                    return;
                }
                else
                {
                    MessageBox.Show(" تم الالغاء ");
                    return;

                }
            }

            else
            {
                // رسالة تأكيد
                var result = MessageBox.Show(
                "هل أنت متأكد أنك تريد طباعه الايصالات المحدده ؟",
                "تأكيد الطباعه",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    //if (ReceiptsDataGrid.SelectedItems is List<GetAllReceiptsDto> selected)

                    if (ReceiptsDataGrid.SelectedItems.Count > 0)
                    {
                        List<GetAllReceiptsDto> selectedItemsDto = ReceiptsDataGrid.SelectedItems.Cast<GetAllReceiptsDto>().ToList();

                        List<int> allReceiptsIds = selectedItemsDto.Select(e => e.DisplayUIId).ToList();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(" تم الالغاء ");
                    return;
                }
            }




            

        }
   
    }
}
