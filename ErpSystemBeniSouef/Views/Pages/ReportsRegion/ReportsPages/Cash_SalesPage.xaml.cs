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
        private readonly ICollectorsReports _collectorsReports;
        private DateTime _dateTime1;
        private DateTime _dateTime2;
        private int _repId;
        List<CashInvoicesReportDto> cashInvoicesReport = new List<CashInvoicesReportDto>();
        decimal totalCash = 0;

        //public Cash_SalesPage( )
        public Cash_SalesPage(ICollectorsReports collectorsReports, DateTime dateTime1, DateTime dateTime2, int repId)
        {
            InitializeComponent();
            _collectorsReports = collectorsReports;
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
            var s = await _collectorsReports.GetRepresentativeCashInvoicesAsync(_dateTime1, _dateTime2, _repId);
            totalCash = s.totalCash;
            cashInvoicesReport = s.Item1;
            CashReportsDataGrid.ItemsSource = cashInvoicesReport;
            TotalCashReport.Text = totalCash.ToString();
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new HomeReportsChooseTab(_dateTime1, _dateTime2, _repId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);
        }
    }
}



#region Comment Page Xaml Region



 //< StackPanel Orientation = "Horizontal" HorizontalAlignment = "Right" Margin = "0,0,0,10" >
 //                       < TextBlock Text = "المقدمات"
 //                   Foreground = "{StaticResource TextBrush}"
 //                   VerticalAlignment = "Center"
 //                   Margin = "0,0,10,0"
 //                   Width = "165"
 //                   FontSize = "16" />

 //                       < Grid Width = "200" HorizontalAlignment = "Right" >
 //                           < Grid.ColumnDefinitions >
 //                               < ColumnDefinition Width = "*" />
 //                               < ColumnDefinition Width = "70" />
 //                           </ Grid.ColumnDefinitions >

 //                           < TextBox Grid.Column = "0"
 //                     Style = "{StaticResource SummaryTextBoxStyle}"
 //                     Width = "Auto" Text = "0"
 //                     Margin = "0,0,5,0"
 //                      />
 //                           < Button Content = "طباعة"
 //                    Grid.Column = "1"
 //                    Style = "{StaticResource MainButtonStyle}"
 //                    Height = "36"
 //                    FontSize = "12"
 //                    Margin = "5,0,0,0" />
 //                       </ Grid >
 //                   </ StackPanel >

 //                   < StackPanel Orientation = "Horizontal" HorizontalAlignment = "Right" Margin = "0,0,0,10" >
 //                       < TextBlock Text = "إجمالي الكاش"
 //                   Foreground = "{StaticResource TextBrush}"
 //                   VerticalAlignment = "Center"
 //                   Margin = "0,0,10,0"
 //                   Width = "165"
 //                   FontSize = "16" />

 //                       < Grid Width = "200" HorizontalAlignment = "Right" >
 //                           < Grid.ColumnDefinitions >
 //                               < ColumnDefinition Width = "*" />
 //                               < ColumnDefinition Width = "70" />
 //                           </ Grid.ColumnDefinitions >

 //                           < TextBox Grid.Column = "0"
 //                     Style = "{StaticResource SummaryTextBoxStyle}"
 //                     Width = "Auto" Name = "TotalCashReport"
 //                     Margin = "0,0,5,0"
 //                     Text = "" />
 //                           < Button Content = "طباعة"
 //                    Grid.Column = "1"
 //                    Style = "{StaticResource MainButtonStyle}"
 //                    Height = "36"
 //                    FontSize = "12"
 //                    Margin = "5,0,0,0" />
 //                       </ Grid >
 //                   </ StackPanel >

 //                   < StackPanel Orientation = "Horizontal" HorizontalAlignment = "Right" Margin = "0,0,0,10" >
 //                       < TextBlock Text = "نسبة المندبة الأولية"
 //                   Foreground = "{StaticResource TextBrush}"
 //                   VerticalAlignment = "Center"
 //                   Margin = "0,0,10,0"
 //                   Width = "165"
 //                   FontSize = "16" />

 //                       < Grid Width = "200" HorizontalAlignment = "Right" >
 //                           < Grid.ColumnDefinitions >
 //                               < ColumnDefinition Width = "*" />
 //                               < ColumnDefinition Width = "70" />
 //                           </ Grid.ColumnDefinitions >

 //                           < TextBox Grid.Column = "0"
 //                     Style = "{StaticResource SummaryTextBoxStyle}"
 //                     Width = "Auto"
 //                     Margin = "0,0,5,0"
 //                     Text = "" />
 //                           < Button Content = "طباعة"
 //                    Grid.Column = "1"
 //                    Style = "{StaticResource MainButtonStyle}"
 //                    Height = "36"
 //                    FontSize = "12"
 //                    Margin = "5,0,0,0" />
 //                       </ Grid >
 //                   </ StackPanel >

 //                   < StackPanel Orientation = "Horizontal" HorizontalAlignment = "Right" Margin = "0,0,0,10" >
 //                       < TextBlock Text = "نسبة المرتجعات"
 //                   Foreground = "{StaticResource TextBrush}"
 //                   VerticalAlignment = "Center"
 //                   Margin = "0,0,10,0"
 //                   Width = "165"
 //                   FontSize = "16" />
 //                       < TextBox Style = "{StaticResource SummaryTextBoxStyle}"
 //                 Width = "200"
 //                 Text = ""
 //                 Margin = "0,0,0,0" />
 //                   </ StackPanel >

 //                   < StackPanel Orientation = "Horizontal" HorizontalAlignment = "Right" >
 //                       < TextBlock Text = "صافي النسبة"
 //                   Foreground = "{StaticResource TextBrush}"
 //                   VerticalAlignment = "Center"
 //                   Margin = "0,0,10,0"
 //                   Width = "165"
 //                   FontSize = "16" />
 //                       < TextBox Style = "{StaticResource SummaryTextBoxStyle}"
 //                 Width = "200"
 //                 FontWeight = "ExtraBold"
 //                 Background = "#304151"
 //                 IsEnabled = "False"
 //                 Text = ""
 //                 Margin = "0,0,0,0" />
 //                   </ StackPanel >



#endregion
