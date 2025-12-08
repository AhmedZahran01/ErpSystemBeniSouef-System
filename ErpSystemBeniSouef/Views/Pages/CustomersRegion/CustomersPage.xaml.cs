using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.CreateCashCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
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


        ObservableCollection<DisplayForUiCustomerinvoicedtos> displayItemsGrid = new ObservableCollection<DisplayForUiCustomerinvoicedtos>();
        int countItemGridDisplayNo = 1;

        List<Customerinvoicedtos> customerinvoicedtosList = new List<Customerinvoicedtos>();

        List<Installmentsdtos> installmentsdtosList = new List<Installmentsdtos>();


        #endregion

        #region Constractor Region

        public CustomersPage(ICustomerInvoiceService customerInvoiceService, IProductService productService,
                              IMainAreaService mainAreaService, ISubAreaService subAreaService, IRepresentativeService representativeService)
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

            allSubAreas = _subAreaService.GetAll();

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
            decimal totalOfItems = 0;
            int NoOfMonthsTxt1 = int.TryParse(setNoOfMonthsTxt.Text, out int amount) ? amount : 0;
            int ValueOfPriceTxt1 = int.TryParse(setValueOfPriceTxt.Text, out int priceV) ? priceV : 0;

            int NoOfMonthsTxt2 = int.TryParse(setNoOfMonthsTxt2.Text, out int amount2) ? amount2 : 0;
            int ValueOfPriceTxt2 = int.TryParse(setValueOfPriceTxt2.Text, out int priceV2) ? priceV2 : 0;
          
            decimal depositFromUi = decimal.TryParse(PayTxt.Text, out decimal depositValue) ? depositValue : 0;


            decimal TotalOfInvoiceItems = NoOfMonthsTxt1 * ValueOfPriceTxt1 + 
                                            NoOfMonthsTxt2 * ValueOfPriceTxt2 + depositFromUi;
            foreach (var item in customerinvoicedtosList)
            {
                totalOfItems += item.Total;
            }

            if (totalOfItems != TotalOfInvoiceItems)
            {
                MessageBox.Show("نظام التقسيط غير صحيح \n \n يرجي المراجعه ");

                return;
            }
            RepresentativeDto selectedRepresentative = (RepresentativeDto)RepresentativeCombo.SelectedItem;
            SubAreaDto selectedsubArea = (SubAreaDto)SubAreaCombo.SelectedItem;
            DateTime? invoiceDate = FirstInvoiceDatePicker.SelectedDate;
            DateTime? saleDatePicker = SaleDatePicker.SelectedDate;

            Installmentsdtos FirstInstallmentsdtos = new Installmentsdtos()
            {
                NumberOfMonths = NoOfMonthsTxt1,
                Amount = ValueOfPriceTxt1,
            };

            Installmentsdtos secondInstallmentsdtos = new Installmentsdtos()
            {
                NumberOfMonths = NoOfMonthsTxt2,
                Amount = ValueOfPriceTxt2,
            };
            installmentsdtosList.Add(FirstInstallmentsdtos);
            installmentsdtosList.Add(secondInstallmentsdtos);
            CreateCustomerInvoiceDTO ddssa = new CreateCustomerInvoiceDTO()
            {
                Address = AddressTxt.Text,
                CollectorId = selectedRepresentative.Id,
                RepresentativeId = selectedRepresentative.Id,

                customerinvoicedtos = customerinvoicedtosList,
                CustomerNumber = int.TryParse(CustomerNumberTxt.Text, out int quantity) ? quantity : 0,
                MobileNumber = PhoneTxt.Text,
                Name = NameTxt.Text,
                NationalNumber = numberIdTxt.Text,
                SubAreaId = selectedsubArea.Id,
                Deposit = depositFromUi,
                FirstInvoiceDate = invoiceDate ?? DateTime.UtcNow,
                SaleDate = saleDatePicker ?? DateTime.UtcNow,

                //installmentsdtos = null,
                installmentsdtos = installmentsdtosList,

            };
            _customerInvoiceService.CreateCustomerInvoiceAsync(ddssa);
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
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            CategoryDto selectedProductType = (CategoryDto)ProductTypeCombo.SelectedItem;
            Customerinvoicedtos customerinvoicedtoss = new Customerinvoicedtos()
            {
                Price = decimal.TryParse(PriceTxt.Text, out decimal price) ? price : 0,
                Quantity = int.TryParse(QuantityTxt.Text, out int quantity) ? quantity : 0,
                ProductId = selectedProduct.Id,
            };
            customerinvoicedtosList.Add(customerinvoicedtoss);

            displayItemsGrid.Add(new DisplayForUiCustomerinvoicedtos()
            {
                Price = decimal.TryParse(PriceTxt.Text, out decimal pwa) ? pwa : 0,
                Quantity = int.TryParse(QuantityTxt.Text, out int pa) ? pa : 0,
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.ProductName,
                ProductCategoryName = selectedProductType.Name,
                DisplayId = countItemGridDisplayNo++
            });

            dgInvoiceItems.ItemsSource = displayItemsGrid;

        }

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            if (selectedProduct is not null)
            {
                PriceTxt.Text = selectedProduct.PurchasePrice.ToString() ?? "";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleBasicCard_Click(object sender, RoutedEventArgs e)
        {
            if (BasicCardContent.Visibility == Visibility.Visible)
            {
                BasicCardContent.Visibility = Visibility.Collapsed;
                ToggleBasicCardBtn.Content = "▲";   // شكل الفتح
            }
            else
            {
                BasicCardContent.Visibility = Visibility.Visible;
                ToggleBasicCardBtn.Content = "▼";   // شكل القفل
            }
        }

        private void ToggleSubBasicCard_Click(object sender, RoutedEventArgs e)
        {
            if (SubBasicCardContent.Visibility == Visibility.Visible)
            {
                SubBasicCardContent.Visibility = Visibility.Collapsed;
                ToggleSubBasicCardBtn.Content = "▲";
            }
            else
            {
                SubBasicCardContent.Visibility = Visibility.Visible;
                ToggleSubBasicCardBtn.Content = "▼";
            }
        }

        private void TogglePlansCard_Click(object sender, RoutedEventArgs e)
        {
            if (PlansCardContent.Visibility == Visibility.Visible)
            {
                PlansCardContent.Visibility = Visibility.Collapsed;
                TogglePlansCardBtn.Content = "▲";
            }
            else
            {
                PlansCardContent.Visibility = Visibility.Visible;
                TogglePlansCardBtn.Content = "▼";
            }
        }

        private void ToggleProductsCard_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsCardContent.Visibility == Visibility.Visible)
            {
                ProductsCardContent.Visibility = Visibility.Collapsed;
                ToggleProductsCardBtn.Content = "▲";
            }
            else
            {
                ProductsCardContent.Visibility = Visibility.Visible;
                ToggleProductsCardBtn.Content = "▼";
            }
        }


    }
}
