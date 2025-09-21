using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
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

namespace ErpSystemBeniSouef.Views.Pages.RepresentativeAndCollector
{
    /// <summary>
    /// Interaction logic for MainRepresentativeAndCollectorPage.xaml
    /// </summary>
    public partial class MainRepresentativeAndCollectorPage : Page
    {

        public MainRepresentativeAndCollectorPage()
        {
            InitializeComponent();
        }


        private void BtnCollector_Click(object sender, RoutedEventArgs e)
        { 
            var collectorService = App.AppHost.Services.GetRequiredService<ICollectorService>(); 

            var collectorPage = new UsersPaes.CollectorPage(collectorService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(collectorPage);

        }

        private void BtnRepresentative_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService< IRepresentativeService >();
            var mapper = App.AppHost.Services.GetRequiredService< IMapper >();

            var representativePage = new UsersPaes.RepresentativePage(representativeService , mapper);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(representativePage);

        }

        private void BtnStorekeepers_Click(object sender, RoutedEventArgs e)
        {
            //var storekeepersPage = new StorekeepersPage();
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(storekeepersPage);
        }

        private void BtnSuppliers_Click(object sender, RoutedEventArgs e)
        {

            var supplierService = App.AppHost.Services.GetRequiredService<ISupplierService>();
            var suppliersPage = new Views.Pages.RepresentativeAndCollector.UsersPaes.SuppliersPage(supplierService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(suppliersPage);

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Pages.Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }


    }
}
