using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Reports;
using ErpSystemBeniSouef.Service.RepresentativeService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.ReportsRegion.ReportsPages;
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

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion
{
    /// <summary>
    /// Interaction logic for HomeReportsChooseTab.xaml
    /// </summary>
    public partial class HomeReportsChooseTab : Page
    {
        private readonly System.DateTime dateTime11;
        private readonly System.DateTime dateTime22;
        private readonly int repId;

        public HomeReportsChooseTab( DateTime dateTime1 , DateTime dateTime2 , int repId)
        {
            InitializeComponent();
            this.dateTime11 = dateTime1;
            this.dateTime22 = dateTime2;
            this.repId = repId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaleButton_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<ICollectionService>();
            var cash_SalesPage = new Installment_SalesPage(representativeService, dateTime11 , dateTime22, repId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(cash_SalesPage);
        }
         
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        { 
            var Dashboard = new Pages.Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        private void Cash_SalesButton_Click(object sender, RoutedEventArgs e)
        {
            var collectionService = App.AppHost.Services.GetRequiredService<ICollectionService>();
            var cash_SalesPage = new Cash_SalesPage(collectionService , dateTime11 , dateTime22 , repId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(cash_SalesPage);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnRateButton_Click(object sender, RoutedEventArgs e)
        {
            var cash_SalesPage = new Return_RatesPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(cash_SalesPage);

        }

        private void New_lamentation_RatiosBtn_Click(object sender, RoutedEventArgs e)
        {
            var cash_SalesPage = new New_lamentation_RatiosPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(cash_SalesPage);
        }
    }
}
