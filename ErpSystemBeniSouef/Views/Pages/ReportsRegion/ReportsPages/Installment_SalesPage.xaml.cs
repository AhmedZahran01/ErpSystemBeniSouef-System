using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Reports;
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

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion.ReportsPages
{
    /// <summary>
    /// Interaction logic for Installment_SalesPage.xaml
    /// </summary>
    public partial class Installment_SalesPage : Page
    {
        private readonly ICollectorsReports _collectorsReports;
        private DateTime _dateTime1;
        private DateTime _dateTime2;
        private int _repId;

        public Installment_SalesPage(ICollectorsReports collectorsReports, DateTime dateTime1, DateTime dateTime2, int repId)
        {
            InitializeComponent();
            _collectorsReports = collectorsReports;
            _dateTime1 = dateTime1;
            _dateTime2 = dateTime2;
            _repId = repId;
            //Loaded += async (s, d) =>
            //{
            Loaded += async (s, e) =>
            {
                try
                {
                    await LoadData();
                }
                catch
                {

                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //GetAllItemsInstallmentSalesReportAsync
        }

        private async Task LoadData()
        {
            _dateTime1 = new DateTime(1025, 1, 1, 10, 30, 0);
            _dateTime2 = new DateTime(3025, 1, 1, 10, 30, 0);
            _repId = 3;

            //var s2 = await _collectionService.GetInstallmentSalesReportAsync(_dateTime1, _dateTime2, 4);
            var installmentReportDtos = await _collectorsReports.GetInstallmentSalesReportAsync(_dateTime1, _dateTime2, 5);
            GridDataForInstallmwnt.ItemsSource = installmentReportDtos;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new HomeReportsChooseTab(_dateTime1, _dateTime2, _repId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);

        }
    }
}
