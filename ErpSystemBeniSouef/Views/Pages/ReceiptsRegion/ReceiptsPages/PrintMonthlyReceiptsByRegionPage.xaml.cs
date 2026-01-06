using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.Receipt;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
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
                await LoadReceipts(null, null, null);
            };
            _receiptService = receiptService;
            _mainAreaService = mainAreaService;
            _subAreaService = subAreaService;
        }

        #region load products to Grid Region

        private async Task LoadReceipts(DateTime? dateOfRecceipt, int? mainId, int? subId)
        {
            DateTime monthDate = new DateTime(1025, 1, 1, 10, 30, 0);
            List<GetAllReceiptsDto> receiptsData4 = new List<GetAllReceiptsDto>();

            byte[] fileData4 = Array.Empty<byte>();
            if (!dateOfRecceipt.HasValue && !mainId.HasValue && !subId.HasValue)
            {
                (receiptsData4, fileData4) = await _receiptService.GetMonthlyReceiptsAsync();

            }
            else
            {
                (receiptsData4, fileData4) = await _receiptService.GetMonthlyReceiptsAsync(dateOfRecceipt, mainId, subId);

            }
            //(receiptsData4, fileData4) = await _receiptService.GetMonthlyReceiptsAsync();
            //receiptsData5 = receiptsData4;
            DataGridRegion.ItemsSource = receiptsData4;



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

        private async void SearchByRegion_Click(object sender, RoutedEventArgs e)
        {
            MainAreaDto mainAreaDto = (MainAreaDto)MainAreaCombo.SelectedItem;
            int MainIdForSearch = 0;
            int SubIdForSearch = 0;
            if (mainAreaDto != null)
            {
                MainIdForSearch = mainAreaDto.Id;
            }
            SubAreaDto subAreaDto = (SubAreaDto)SubAreaCombo.SelectedItem;
            if (subAreaDto != null)
            {
                SubIdForSearch = subAreaDto.Id;
            }
            DateTime SelectedDate = DateOfReceipt.SelectedDate ?? DateTime.UtcNow;

            var SearableData =  LoadReceipts(SelectedDate , MainIdForSearch  , SubIdForSearch);

        }

        private async void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var selected = sender as RadioButton;
            if (selected?.Name.ToString() == "rbMainAll")
            {
                // الكل main
            }
            else if (selected?.Name.ToString() == "rbMainSelect")
            {
                MainAreaCombo.IsEnabled = true;
                var MainAreaDtos = await _mainAreaService.GetAllAsync();
                MainAreaCombo.ItemsSource = MainAreaDtos;
                MainAreaCombo.SelectedIndex = 0;
                // select main
            }
            else if (selected?.Name.ToString() == "rbSubAll")
            {
                // الكل
            }
            else if (selected?.Name.ToString() == "rbSubSelect")
            {
                MainAreaDto mainAreaDto = (MainAreaDto)MainAreaCombo.SelectedItem;
                SubAreaCombo.IsEnabled = true;
                if (mainAreaDto != null)
                {
                    int subIdSelectedMainArea = mainAreaDto.Id;
                    var subAreaDtos =  _subAreaService.GetSubAreaDtoByMainAreaId(subIdSelectedMainArea);
                    SubAreaCombo.ItemsSource = subAreaDtos;
                    SubAreaCombo.SelectedIndex = 0;
                }
                else
                {
                    var subAreaDtos = await _subAreaService.GetAllAsync();
                    SubAreaCombo.ItemsSource = subAreaDtos;
                    SubAreaCombo.SelectedIndex = 0;
                }
                // الكل
            }

        }
         
    }
}
