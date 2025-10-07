using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;
using System.Windows;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ErpSystemBeniSouef.ViewModel;

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.SupplierAccounts
{
    /// <summary>
    /// Interaction logic for SupplierAccountsPage.xaml
    /// </summary>
    public partial class SupplierAccountsPage : Page
    {
        #region Global Variables Region

        private readonly ISupplierAccountService _supplierAccountService;
        int countDisplayNo = 0; int countDisplayNoInvoiceDto = 0;
        int countDisplayNoSupplierAccoun = 0;
        string _MoveType = ""; string _SaleType = "";

        private readonly ISupplierService _supplierService;
        IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        ObservableCollection<ReturnSupplierCashDto> observProductsLisLim = new ObservableCollection<ReturnSupplierCashDto>();

        ObservableCollection<SupplierCashDto> observSupplierAccountLisLim = new ObservableCollection<SupplierCashDto>();
        ObservableCollection<SupplierInvoiceDto> observSupplierInvoiceDtoLisLim = new ObservableCollection<SupplierInvoiceDto>();

        private readonly ISupplierCashService _supplierCashService;

        #endregion

        #region Constractor Region

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

            };

            txtStartDate.SelectedDate = DateTime.Parse("7/23/2024 12:00:00 AM");
            txtEndDate.SelectedDate = DateTime.Parse("7/23/2027 12:00:00 AM");

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            //IReadOnlyList<ReturnSupplierCashDto> invoiceDtos = await _supplierCashService.GetAllSupplierAccounts();
            //observProductsLisLim.Clear(); 
            //foreach (var product in invoiceDtos)
            //{
            //    product.DisplayId = countDisplayNo + 1;
            //    observProductsLisLim.Add(product);  

            //}

        }


        #endregion

        #region Result Show Button_Click Region


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
                product.DisplayId = countDisplayNoSupplierAccoun + 1;
                observSupplierAccountLisLim.Add(product);
                //observProductsListFiltered.Add(product);
                countDisplayNoSupplierAccoun++;
            }
            observSupplierInvoiceDtoLisLim.Clear();
            foreach (var product in da.Invoices)
            {
                int InvoiceTypeId =(int) product.invoiceType;
                if (InvoiceTypeId == 1) { _MoveType = "اضافه"; _SaleType = "كاش"; }
                else if (InvoiceTypeId  == 2) { _MoveType = "اضافه"; _SaleType = "تقسيط"; }
                else if (InvoiceTypeId  == 3) { _MoveType = "ارجاع للمورد"; _SaleType = ""; }


                
                product.DisplayId = countDisplayNoInvoiceDto + 1;
                product.MoveType =  _MoveType;
                product.SaleType =  _SaleType;
                observSupplierInvoiceDtoLisLim.Add(product);
                //observProductsListFiltered.Add(product);
                countDisplayNoInvoiceDto++;
            }

            dgSuppliersCash.ItemsSource = observSupplierAccountLisLim;
            dgInvoices.ItemsSource = observSupplierInvoiceDtoLisLim;


        }

        #endregion

        #region Back Btn Click Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new InvoiceAndsupplierRegion(1);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        #endregion


    }
}
