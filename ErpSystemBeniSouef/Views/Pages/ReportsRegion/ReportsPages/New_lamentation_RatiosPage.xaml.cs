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
//using static ClosedXML.Excel.XLPredefinedFormat;

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion.ReportsPages
{
    /// <summary>
    /// Interaction logic for New_lamentation_RatiosPage.xaml
    /// </summary>
    public partial class New_lamentation_RatiosPage : Page
    {
        private readonly ICollectorsReports _collectorsReports;
        private DateTime _dateTime1;
        private DateTime _dateTime2;
        private int _repId;
        public New_lamentation_RatiosPage(ICollectorsReports collectorsReports)
        {
            InitializeComponent();
            this._collectorsReports = collectorsReports;
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

        private async Task LoadData()
        {
            _dateTime1 = new DateTime(1025, 1, 1, 10, 30, 0);
            _dateTime2 = new DateTime(3025, 1, 1, 10, 30, 0);
            _repId = 3;
            var commissionReportDtos1 = await _collectorsReports.GetAllItemsInstallmentSalesReportAsync(_dateTime1, _dateTime2, 2);
              
            dataGridOfNewReprese.ItemsSource = commissionReportDtos1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new HomeReportsChooseTab(_dateTime1, _dateTime2, _repId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);

        }
    }
}
