using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Service.SupplierAccountServices;
using ErpSystemBeniSouef.Service.supplierCashService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.SupplierAccounts;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion
{
    /// <summary>
    /// Interaction logic for InvoiceAndsupplierRegion.xaml
    /// </summary>
    public partial class InvoiceAndsupplierRegion : Page
    {
        private readonly int _companyNo;

        public InvoiceAndsupplierRegion(int CompanyNo)
        {
            InitializeComponent();
            _companyNo = CompanyNo;
        }


        private void BtnInvoiceRegions_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new InvoicePages.InvoicesRegion(_companyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);

        }

        private void Suppliers_cash(object sender, RoutedEventArgs e)
        {
            var supplierCashService = App.AppHost.Services.GetRequiredService<ISupplierCashService>();
            var supplierService = App.AppHost.Services.GetRequiredService<ISupplierService>();

            var Suppliers_cashPage = new Suppliers_cash.Suppliers_cashPage(supplierService,supplierCashService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Suppliers_cashPage);

        }

        private void Supplier_accounts(object sender, RoutedEventArgs e)
        {
            var supplierService = App.AppHost.Services.GetRequiredService<ISupplierService>();
            var supplierAccountService = App.AppHost.Services.GetRequiredService<ISupplierAccountService>();
            var supplierCashService = App.AppHost.Services.GetRequiredService<ISupplierCashService>();

            var Supplier_accountsPage = new  SupplierAccountsPage(supplierService, supplierCashService, supplierAccountService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Supplier_accountsPage);

        }

        private void Stores(object sender, RoutedEventArgs e)
        {
            //var StoresPage = new StoresPage();
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(StoresPage);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //int Cpmpno = 1;
            var Dashboard = new  Pages.Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }


    }
}
