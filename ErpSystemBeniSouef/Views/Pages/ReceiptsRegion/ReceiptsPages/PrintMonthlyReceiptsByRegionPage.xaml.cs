using ErpSystemBeniSouef.Core.Contract;
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
    /// Interaction logic for PrintMonthlyReceiptsByRegionPage.xaml
    /// </summary>
    public partial class PrintMonthlyReceiptsByRegionPage : Page
    {
        private readonly IReceiptService _receiptService;
        private readonly IMainAreaService _mainAreaService;
        private readonly ISubAreaService _subAreaService;

        public PrintMonthlyReceiptsByRegionPage(IReceiptService receiptService, IMainAreaService mainAreaService, ISubAreaService subAreaService)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                await LoadReceipts();
            };
            _receiptService = receiptService;
            _mainAreaService = mainAreaService;
            _subAreaService = subAreaService;
        }

        #region load products to Grid Region

        private async Task LoadReceipts(int mainId = 4, int fr = 1, int to = 100000)
        {
            DateTime monthDate = DateTime.UtcNow;
            (var receiptsData4, var fileData4) = await _receiptService.GetMonthlyReceiptsAsync(monthDate, fr, to);
            DataGridRegion.ItemsSource = receiptsData4;

            var MainAreaDtos = await _mainAreaService.GetAllAsync();
            MainAreaCombo.ItemsSource = MainAreaDtos;

            var subAreaDtos = await _subAreaService.GetAllAsync();
            SubAreaCombo.ItemsSource = subAreaDtos;

        }

        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new HomeReceiptsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #region Radio Btn comment Region

        //private void MainRadio_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (sender is RadioButton rb)
        //    {
        //        switch (rb.Tag)
        //        {
        //            case "AllMain":
        //                LoadAllBtnVisibitity(1);
        //                break;

        //            case "SelectMain":
        //                LoadAllBtnVisibitity(2);
        //                break;
        //            case "AllSub":
        //                LoadAllBtnVisibitity(3);
        //                break;

        //            case "SelectSub":
        //                LoadAllBtnVisibitity(4);
        //                break;
        //        }
        //    }
        //}

        //public void LoadAllBtnVisibitity(int typeData)
        //{
        //    if (typeData == 1)
        //    {
        //        MainAreaCombo.Visibility = Visibility.Collapsed;
        //    }
        //    else if (typeData == 2)
        //    {
        //        MainAreaCombo.Visibility = Visibility.Visible;
        //    }
        //    else if (typeData == 3)
        //    {
        //        SubAreaCombo.Visibility = Visibility.Collapsed;
        //    }
        //    else if (typeData == 4)
        //    {
        //        SubAreaCombo.Visibility = Visibility.Visible;
        //    }

        //}


        #endregion


    }
}
