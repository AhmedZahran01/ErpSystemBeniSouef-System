using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.HelperFunctions;
using ErpSystemBeniSouef.Service.CustomerInvoiceServices;
using ErpSystemBeniSouef.Service.MainAreaServices;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.Service.RepresentativeService;
using ErpSystemBeniSouef.Service.SubAreaServices;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.ReportsRegion;
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
        private readonly int _comanyNo = AppGlobalCompanyId.CompanyId;

        public Dashboard( )
        {
            InitializeComponent(); 
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

            var productsPage = new Views.Pages.Products.AllProductsPage( productService, mapper);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(productsPage);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RepresentativeCollector_Click(object sender, RoutedEventArgs e)
        {
            var representativeCollectorPage = new RepresentativeAndCollector.MainRepresentativeAndCollectorPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(representativeCollectorPage);

        }

        private void SignOutButton_Click_2(object sender, RoutedEventArgs e)
        {
            var Dashboard = new LoginPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
            App.Current.Properties["CompanyId"] = 0;


        }

        private void InvoiceMainPage_Click_2(object sender, RoutedEventArgs e)
        {
            var invoicePage = new InvoiceAndsupplierRegion.InvoiceAndsupplierRegion(_comanyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicePage);

        }
         

        private void CustomersPage_Click(object sender, RoutedEventArgs e)
        {
            var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            var customerInvoiceService = App.AppHost.Services.GetRequiredService<ICustomerInvoiceService>();
            var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var subAreaService = App.AppHost.Services.GetRequiredService<ISubAreaService>();
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();

            var customersPage = new  CustomersRegion.CustomersPage(customerInvoiceService, productService , mainAreaService,
                                             subAreaService , representativeService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);
        }

        private void RepresentativeReports_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new ChooseRepresentative(representativeService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
     