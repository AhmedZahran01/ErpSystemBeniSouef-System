using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Windows;
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

namespace ErpSystemBeniSouef.Views.Pages.Products
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private readonly int _comanyNo;

        public Dashboard(int CompanyNo)
        {
            InitializeComponent();
            _comanyNo = CompanyNo;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { 
            var regionsPage = new ErpSystemBeniSouef.Views.Pages.Products.RegionsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);
        }
          
        private void Products_Click_1(object sender, RoutedEventArgs e)
        {
            var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            var mapper = App.AppHost.Services.GetRequiredService<IMapper>();

            var productsPage = new Views.Pages.Products.AllProductsPage(_comanyNo, productService, mapper);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(productsPage);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RepresentativeCollector_Click(object sender, RoutedEventArgs e)
        { 
        }

        private void SignOutButton_Click_2(object sender, RoutedEventArgs e)
        {
            var Dashboard = new LoginPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        } 

        private void InvoiceMainPage_Click_2(object sender, RoutedEventArgs e)
        {
            var invoicePage = new InvoiceAndsupplierRegion.InvoiceAndsupplierRegion(_comanyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicePage);

        }
    }
}
