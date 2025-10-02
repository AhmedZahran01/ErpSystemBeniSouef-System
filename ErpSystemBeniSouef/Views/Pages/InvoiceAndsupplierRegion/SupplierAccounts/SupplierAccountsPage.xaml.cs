using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Service.supplierCashService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.SupplierAccounts
{
    /// <summary>
    /// Interaction logic for SupplierAccountsPage.xaml
    /// </summary>
    public partial class SupplierAccountsPage : Page
    {
        #region MyRegion

        private readonly ISupplierAccountService _supplierAccountService;
        int countDisplayNo = 0;

        private readonly ISupplierService _supplierService;
        IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        ObservableCollection<ReturnSupplierCashDto> observProductsLisLim = new ObservableCollection<ReturnSupplierCashDto>();

        ObservableCollection<SupplierCashDto> observSupplierAccountLisLim = new ObservableCollection<SupplierCashDto>();
        ObservableCollection<SupplierInvoiceDto> observSupplierInvoiceDtoLisLim = new ObservableCollection<SupplierInvoiceDto>();


        private readonly ISupplierCashService _supplierCashService;

        #endregion

        #region MyRegion

        public SupplierAccountsPage(ISupplierService supplierService, ISupplierCashService supplierCashService,
                                      ISupplierAccountService supplierAccountService)
        {
            InitializeComponent();
            _supplierAccountService = supplierAccountService;
            _supplierService = supplierService;
            _supplierCashService = supplierCashService;
            Loaded += async (s, e) =>
            {
                SuppliersDto = await _supplierService.GetAllAsync();
                cb_SuppliersName.ItemsSource = SuppliersDto;
                cb_SuppliersName.SelectedIndex = 0;
                await LoadInvoices();
                //dgSuppliersCash.ItemsSource = observProductsLisLim;

            };

        }

        #endregion


        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<ReturnSupplierCashDto> invoiceDtos = await _supplierCashService.GetAllSupplierAccounts();
            observProductsLisLim.Clear();
            //observProductsListFiltered.Clear();
            foreach (var product in invoiceDtos)
            {
                product.DisplayId = countDisplayNo + 1;
                observProductsLisLim.Add(product);
                //observProductsListFiltered.Add(product);
                //countDisplayNo++;

            }

        }


        #endregion


        #region MyRegion


        private async void ResultShowButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? StartInterval = txtStartDate.SelectedDate;
            DateTime? EndInterval = txtEndDate.SelectedDate;
            SupplierRDto selectedSupplier = (SupplierRDto)cb_SuppliersName.SelectedItem;
            int SupplierId = selectedSupplier.Id;

            var da = await _supplierAccountService.GetSupplierAccount(SupplierId, StartInterval, EndInterval);
            observSupplierAccountLisLim.Clear();
            foreach (var product in da.Payments)
            {
                observSupplierAccountLisLim.Add(product);
                //observProductsListFiltered.Add(product);
                //countDisplayNo++;
            }
            observSupplierInvoiceDtoLisLim.Clear();
            foreach (var product in da.Invoices)
            {
                observSupplierInvoiceDtoLisLim.Add(product);
                //observProductsListFiltered.Add(product);
                //countDisplayNo++;
            }

            dgSuppliersCash.ItemsSource = observSupplierAccountLisLim;
            dgInvoices.ItemsSource = observSupplierInvoiceDtoLisLim;


        }

        #endregion
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new InvoiceAndsupplierRegion(1);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

    }
}
