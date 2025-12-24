using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Reports;
using ErpSystemBeniSouef.Core.DTOs.Reports;
using ErpSystemBeniSouef.ViewModel;
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
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion.ReportsPages
{
    /// <summary>
    /// Interaction logic for Cash_SalesPage.xaml
    /// </summary>
    public partial class Cash_SalesPage : Page
    {
        private readonly ICollectionService _collectionService;
        private DateTime _dateTime1;
        private DateTime _dateTime2;
        private int _repId;
        List<CashInvoicesReportDto> cashInvoicesReport = new List<CashInvoicesReportDto>();
        decimal totalCash = 0;

        //public Cash_SalesPage( )
        public Cash_SalesPage(ICollectionService collectionService, DateTime dateTime1, DateTime dateTime2, int repId)
        {
            InitializeComponent();
            _collectionService = collectionService;
            Loaded += async (s, e) =>
            {
                LoadingBar.Visibility = Visibility.Visible;
                LoadingText.Visibility = Visibility.Visible;
                LoadingText.Text = "جاري تحميل البيانات...";

                try
                {
                    _dateTime1 = new DateTime(1025, 1, 1, 10, 30, 0);
                    _dateTime2 = new DateTime(3025, 1, 1, 10, 30, 0);
                    _repId = 2;

                    await LoadData();

                    LoadingText.Text = "تم تحميل البيانات بنجاح";

                    await Task.Delay(2000);
                    LoadingBar.Visibility = Visibility.Collapsed;
                    LoadingText.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    LoadingText.Text = "حدث خطأ أثناء تحميل البيانات";
                }
            };
            //Loaded += async (s, e) =>
            //{
            //    LoadDataFromDB.Content = " جاري تحميل البيانات... ";
            //    try
            //    {
            //        _dateTime1 = new DateTime(1025, 1, 1, 10, 30, 0);
            //        _dateTime2 = new DateTime(3025, 1, 1, 10, 30, 0);
            //        _repId = 2;

            //        //_dateTime1 = dateTime1;
            //        //_dateTime2 = dateTime2;
            //        //_repId = repId;

            //        await LoadData();
            //        LoadDataFromDB.Content = "تم تحميل الداتا بنجاح";
            //    }
            //    catch
            //    {
            //        LoadDataFromDB.Content = "Data Not Loaded , There is an error ";
            //    }

            //};

        }

        private async Task LoadData()
        {
            var s = await _collectionService.GetRepresentativeCashInvoicesAsync(_dateTime1, _dateTime2, _repId);
            totalCash = s.totalCash;
            cashInvoicesReport = s.Item1;
            CashReportsDataGrid.ItemsSource = cashInvoicesReport;
            TotalCashReport.Text = totalCash.ToString();
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new ChooseRepresentative(representativeService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);
        }
    }
}
