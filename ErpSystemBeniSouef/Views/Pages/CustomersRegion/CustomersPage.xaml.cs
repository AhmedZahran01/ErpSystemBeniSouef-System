using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.InvoiceServices.CashInvoiceService;
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

namespace ErpSystemBeniSouef.Views.Pages.CustomersRegion
{
    /// <summary>
    /// Interaction logic for CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        #region Global Variables  Region
        //private readonly int _companyNo = 1;
        //private readonly ISupplierService _supplierService;
        //private readonly ICashInvoiceService _cashInvoiceService;
        //private readonly IMapper _mapper;
        //IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        int countDisplayNo = 0;

        List<ReturnCustomerInvoiceListDTO> observCustomerInvoiceList = new List<ReturnCustomerInvoiceListDTO>();
        ObservableCollection<ReturnCustomerInvoiceListDTO> observCustomerInvoiceFilteredList = new ObservableCollection<ReturnCustomerInvoiceListDTO>();

        private readonly ICustomerInvoiceService _customerInvoiceService;
        private readonly IProductService _productService;
        private readonly IMainAreaService _mainAreaService;
        private readonly ISubAreaService _subAreaService;
        private readonly IRepresentativeService _representativeService;
        ObservableCollection<ProductDto> observProductsList = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();

        private IReadOnlyList<SubAreaDto> allSubAreas = new List<SubAreaDto>();
        private ObservableCollection<SubAreaDto> observalSubRegionFilter = new();

        #endregion

        #region Constractor Region

        public CustomersPage(ICustomerInvoiceService customerInvoiceService, IProductService productService,
                              IMainAreaService mainAreaService , ISubAreaService subAreaService , IRepresentativeService representativeService) 
        {
            InitializeComponent();
            _customerInvoiceService = customerInvoiceService;
            _productService = productService;
            _mainAreaService = mainAreaService;
            _subAreaService = subAreaService;
            _representativeService = representativeService;
            Loaded += async (s, e) =>
            {
                //SuppliersDto = await _supplierService.GetAllAsync();
                //cb_SuppliersName.ItemsSource = SuppliersDto;
                //cb_SuppliersName.SelectedIndex = 0;
                //await LoadInvoices();

                 
                await LoadInvoices();
                CustomersGrid.ItemsSource = observCustomerInvoiceFilteredList;
                ProductTypeCombo.ItemsSource = await _productService.GetAllCategoriesAsync();
                ProductTypeCombo.SelectedIndex = 0;

                ProductCombo.ItemsSource = observProductsListFiltered;
                MainAreaCombo.ItemsSource = _mainAreaService.GetAll();

                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();



            };

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            var CustomerInvoiceList = await _customerInvoiceService.GetAllCustomerInvoicesAsync();
            observCustomerInvoiceList.Clear(); 
            observCustomerInvoiceFilteredList.Clear(); 
            foreach (var product in CustomerInvoiceList.Data)
            {
                product.DisplayId = countDisplayNo + 1;
                observCustomerInvoiceList.Add(product);
                observCustomerInvoiceFilteredList.Add(product);
                countDisplayNo++;
            }


            IReadOnlyList<ProductDto> products = _productService.GetAll();
            foreach (var product in products)
            {
                observProductsList.Add(product);
            }

            allSubAreas =  _subAreaService.GetAll();

        }


        #endregion


        #region cb Product Type Selection Changed Region

        private void cbProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductTypeCombo.SelectedItem is CategoryDto s)
            {
                var filtered = observProductsList.Where(p => p.CategoryId == s.Id).ToList();
                observProductsListFiltered.Clear();
                foreach (var product in filtered)
                {
                    observProductsListFiltered.Add(product);
                }
                ProductCombo.ItemsSource = observProductsListFiltered;
                ProductCombo.SelectedIndex = 0;

            }

        }

        #endregion
         

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {

        }


        #region cb Main Area Selection Changed Region
        private void cbMainArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainAreaCombo.SelectedItem is MainAreaDto s)
            {
                var filtered = allSubAreas.Where(p => p.MainAreaId == s.Id).ToList();
                observalSubRegionFilter.Clear();
                foreach (var product in filtered)
                {
                    observalSubRegionFilter.Add(product);
                }
                SubAreaCombo.ItemsSource = observalSubRegionFilter;
                SubAreaCombo.SelectedIndex = 0;

            }
        }

        #endregion

         

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddInvoiceItemCustomer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
