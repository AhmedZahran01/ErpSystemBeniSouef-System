using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.MainAreaServices;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages
{
    /// <summary>
    /// Interaction logic for Cashinvoice.xaml
    /// </summary>
    public partial class Cashinvoice : Page
    {

        #region Global Variables  Region
        private readonly int _companyNo = 1;
        private readonly ISupplierService _supplierService;
        private readonly ICashInvoiceService _cashInvoiceService;
        private readonly IMapper _mapper; 
 
        IReadOnlyList<SupplierDto> SuppliersDto = new List<SupplierDto>();
        ObservableCollection<ReturnCashInvoiceDto> observProductsLisLim = new ObservableCollection<ReturnCashInvoiceDto>();
        ObservableCollection<ReturnCashInvoiceDto> observProductsListFiltered = new ObservableCollection<ReturnCashInvoiceDto>();

        #endregion

        #region Constractor Region

        //List<Suppliers> SupplierNames = new List<Suppliers>();
        //List<CashInvoice> CashInvoiceData = new List<CashInvoice>();
        public Cashinvoice(ISupplierService supplierService, ICashInvoiceService cashInvoiceService)
        {
            InitializeComponent();
            _supplierService = supplierService;
            _cashInvoiceService = cashInvoiceService;
            Loaded += async (s, e) =>
            {
                cb_SuppliersName.ItemsSource = await _supplierService.GetAllAsync(); ; 
                cb_SuppliersName.SelectedIndex = 0;
                await LoadInvoices();
                dgCashInvoice.ItemsSource = observProductsLisLim;

            };

        }

        #endregion

        #region LoadInvoices Dta Region
         
        private async Task LoadInvoices()
        {
            IReadOnlyList<ReturnCashInvoiceDto> products = await _cashInvoiceService.GetAllAsync();
            foreach (var product in products)
            {
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
            } 
        }


        #endregion








        #region Back btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicesRegion(_companyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
        }

        #endregion

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
