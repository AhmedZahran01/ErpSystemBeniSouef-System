using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CashCustomerInvoiceServices;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.CreateCashCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.ReturnAllCashCustomerInvoices;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.ViewModel;
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

namespace ErpSystemBeniSouef.Views.Pages.CustomersRegion.CashPage
{
    /// <summary>
    /// Interaction logic for CashCustomerInvoicePage.xaml
    /// </summary>
    public partial class CashCustomerInvoicePage : Page
    {

        #region Global Variables  Region 
        int countDisplayNo = 0;
        private readonly IProductService _productService;
        private readonly IMainAreaService _mainAreaService;
        private readonly ISubAreaService _subAreaService;
        private readonly IRepresentativeService _representativeService;
        private readonly ICashCustomerInvoiceService _cashCustomerInvoiceService;

        ObservableCollection<ProductDto> observProductsList = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();

        private IReadOnlyList<SubAreaDto> allSubAreas = new List<SubAreaDto>();
        private ObservableCollection<SubAreaDto> observalSubRegionFilter = new();
        private List<ReturnAllCashCustomerInvoicesDTO> returnAllCashCustomers = new();

        ObservableCollection<DisplayForUiCustomerinvoicedtos> displayItemsGrid = new ObservableCollection<DisplayForUiCustomerinvoicedtos>();

        List<Cashcustomerinvoicedtos> cashcustomerinvoicedtosFromUi = new List<Cashcustomerinvoicedtos>();
        int countItemGridDisplayNo = 1;
        decimal cashInvoiceTotal = 0;

        int selectedCashCustomerInvoiceId = 0;
        #endregion

        #region Constractor Region

        public CashCustomerInvoicePage(IProductService productService, IMainAreaService mainAreaService,
                              ISubAreaService subAreaService, IRepresentativeService representativeService
                              , ICashCustomerInvoiceService cashCustomerInvoiceService)
        {
            InitializeComponent();
            _productService = productService;
            _mainAreaService = mainAreaService;
            _subAreaService = subAreaService;
            _representativeService = representativeService;
            this._cashCustomerInvoiceService = cashCustomerInvoiceService;
            Loaded += async (s, e) =>
            {
                await LoadInvoices();
                ProductTypeCombo.ItemsSource = await _productService.GetAllCategoriesAsync();
                ProductTypeCombo.SelectedIndex = 0;

                ProductCombo.ItemsSource = observProductsListFiltered;
                MainAreaCombo.ItemsSource = _mainAreaService.GetAll();
                MainAreaCombo.SelectedIndex = 0;

                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();
                RepresentativeCombo.SelectedIndex = 0;

                CashDataGrid.ItemsSource = returnAllCashCustomers;
            };
        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<ProductDto> products = _productService.GetAll();
            foreach (var product in products)
            {
                observProductsList.Add(product);
            }

            allSubAreas = _subAreaService.GetAll();
            var returnAllCashCustomerss = await _cashCustomerInvoiceService.GetAllCashCustomerInvoices();
            returnAllCashCustomers = returnAllCashCustomerss.Data;

        }


        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            CategoryDto selectedProductType = (CategoryDto)ProductTypeCombo.SelectedItem;
            Customerinvoicedtos customerinvoicedtoss = new Customerinvoicedtos()
            {
                Price = decimal.TryParse(PriceTxt.Text, out decimal price) ? price : 0,
                Quantity = int.TryParse(QuantityTxt.Text, out int quantity) ? quantity : 0,
                ProductId = selectedProduct.Id,
            };
            decimal productPrice = decimal.TryParse(PriceTxt.Text, out decimal pwa) ? pwa : 0;
            int quantityPrice = int.TryParse(QuantityTxt.Text, out int pa) ? pa : 0;
            displayItemsGrid.Add(new DisplayForUiCustomerinvoicedtos()
            {
                Price = productPrice,
                Quantity = quantityPrice,
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.ProductName,
                ProductCategoryName = selectedProductType.Name,
                DisplayId = countItemGridDisplayNo++
            });

            cashcustomerinvoicedtosFromUi.Add(new Cashcustomerinvoicedtos()
            {
                Price = productPrice,
                ProductId = selectedProduct.Id,
                Quantity = quantityPrice,
            });

            ItemsDataGrid.ItemsSource = displayItemsGrid;
            cashInvoiceTotal += customerinvoicedtoss.Total;
            TotalAmountTxt.Text = cashInvoiceTotal.ToString();
        }

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

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            if (selectedProduct is not null)
            {
                PriceTxt.Text = selectedProduct.PurchasePrice.ToString() ?? "";
            }
        }

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
                subAreaCombo.ItemsSource = observalSubRegionFilter;
                subAreaCombo.SelectedIndex = 0;

            }
        }

        private void AddCashCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            string representiveName = "";
            if (RepresentativeCombo.SelectedItem is RepresentativeDto selctedRepresentive)
            {
                representiveName = selctedRepresentive.Name;
            }

            string mainAreaName = "";
            if (MainAreaCombo.SelectedItem is MainAreaDto s)
            {
                mainAreaName = s.Name;
            }

            string SubAreaName = "";
            if (subAreaCombo.SelectedItem is SubAreaDto subArea)
            {
                SubAreaName = subArea.Name;
            }

            DateTime? saleDatePicker = SaleDateTxt.SelectedDate;

            ReturnAllCashCustomerInvoicesDTO cashCustomerInvoicesDTO = new ReturnAllCashCustomerInvoicesDTO()
            {
                CollectorName = representiveName,
                MainAreaName = mainAreaName,
                RepresentativeName = representiveName,
                SubAreaName = SubAreaName,
                SaleDate = saleDatePicker ?? DateTime.UtcNow,
                serialNumber = 2,
                total = cashInvoiceTotal
            };

            CreateCashCustomerInvoiceDTO createCashCustomer = new CreateCashCustomerInvoiceDTO()
            {
                cashcustomerinvoicedtos = cashcustomerinvoicedtosFromUi,
                RepresentativeId = 2,
                SubAreaId = 2,
                SaleDate = saleDatePicker ?? DateTime.UtcNow,

            };

            var res = _cashCustomerInvoiceService.AddCashCustomerInvoice(createCashCustomer);
        }

        private void CustomersGrid_MouseDoubleClick(object sender, SelectionChangedEventArgs e)
        {
            if (CashDataGrid.SelectedItem is ReturnAllCashCustomerInvoicesDTO s)
            {
                selectedCashCustomerInvoiceId = s.serialNumber;

            }
        }

        private void DeleteCashCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCashCustomerInvoiceId != 0)
            {
                var DeleteRes = _cashCustomerInvoiceService.DeleteCashCustomerInvoiceAsync(selectedCashCustomerInvoiceId);
            }
        }

    }
}
