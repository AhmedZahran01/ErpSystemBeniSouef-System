using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CashCustomerInvoiceServices;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.CreateCashCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.ReturnAllCashCustomerInvoices; 
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.ViewModel; 
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace ErpSystemBeniSouef.Views.Pages.CustomersRegion.CashPage
{
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
        private List<ReturnAllCashCustomerInvoicesDTO> returnAllCashCustomersFilter = new();

        ObservableCollection<DisplayForUiCustomerinvoicedtos> displayItemsGrid = new ObservableCollection<DisplayForUiCustomerinvoicedtos>();

        List<Cashcustomerinvoicedtos> cashcustomerinvoicedtosFromUi = new List<Cashcustomerinvoicedtos>();
        int countItemGridDisplayNo = 1;
        decimal cashInvoiceTotal = 0;
        //List<int> ListOfIds = new List<int>();
        int selectedCashCustomerInvoiceIdOfDB = 0;
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

                CashDataGrid.ItemsSource = returnAllCashCustomersFilter;
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
            returnAllCashCustomersFilter = returnAllCashCustomers;
        }


        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion

        #region Add Product Click Region

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            CategoryDto selectedProductType = (CategoryDto)ProductTypeCombo.SelectedItem;
            int quantity = int.TryParse(QuantityTxt.Text, out int pa) ? pa : 0;
            if (quantity <= 0)
            {
                MessageBox.Show("من فضلك ادخل كميه صحيحه ");
                return;
            }
            Customerinvoicedtos customerinvoicedtoss = new Customerinvoicedtos()
            {
                Price = decimal.TryParse(PriceTxt.Text, out decimal price) ? price : 0,
                Quantity = quantity,
                ProductId = selectedProduct.Id,
            };
            decimal productPrice = decimal.TryParse(PriceTxt.Text, out decimal pwa) ? pwa : 0;
            displayItemsGrid.Add(new DisplayForUiCustomerinvoicedtos()
            {
                Price = productPrice,
                Quantity = quantity,
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.ProductName,
                ProductCategoryName = selectedProductType.Name,
                DisplayId = countItemGridDisplayNo++
            });

            cashcustomerinvoicedtosFromUi.Add(new Cashcustomerinvoicedtos()
            {
                Price = productPrice,
                ProductId = selectedProduct.Id,
                Quantity = quantity,
            });

            ItemsDataGrid.ItemsSource = displayItemsGrid;
            cashInvoiceTotal += customerinvoicedtoss.Total;
            TotalAmountTxt.Text = cashInvoiceTotal.ToString();
        }

        #endregion

        #region Product Type Selection Changed Region

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

        #region Product Selection Changed Region

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            if (selectedProduct is not null)
            {
                PriceTxt.Text = selectedProduct.PurchasePrice.ToString() ?? "";
            }
        }

        #endregion

        #region Main Area Selection Changed Region

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

        #endregion

        #region Add Cash Customer Btn Click Region

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
            if (saleDatePicker == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }
            ReturnAllCashCustomerInvoicesDTO cashCustomerInvoicesDTO = new ReturnAllCashCustomerInvoicesDTO()
            {
                CollectorName = representiveName,
                MainAreaName = mainAreaName,
                RepresentativeName = representiveName,
                SubAreaName = SubAreaName,
                SaleDate = saleDatePicker ?? DateTime.UtcNow,
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
            if (res.IsCompleted == false)
            {
                returnAllCashCustomersFilter.Add(new ReturnAllCashCustomerInvoicesDTO()
                {
                    RepresentativeName = representiveName,
                    CollectorName = representiveName,
                    MainAreaName = mainAreaName,
                    SaleDate = saleDatePicker ?? DateTime.UtcNow,
                    //serialNumber= 2,
                    total = cashInvoiceTotal,
                    SubAreaName = SubAreaName,
                    serialNumber = returnAllCashCustomersFilter.Last().serialNumber + 1,
                    serialNumberIdFromDB = returnAllCashCustomersFilter.Last().serialNumberIdFromDB + 1
                });
                CashDataGrid.Items.Refresh();
                MessageBox.Show($"تم اضافه فاتوره كاش  ");
            } 
            //CreateCash(createCashCustomer  );

        }

        //private async Task CreateCash(CreateCashCustomerInvoiceDTO createCashCustomer)
        //{ 
        //        var res = await _cashCustomerInvoiceService.AddCashCustomerInvoice(createCashCustomer);
        //if (res.Success)
        //{
        //    //returnAllCashCustomersFilter.Add(new ReturnAllCashCustomerInvoicesDTO()
        //    //{
        //    //    RepresentativeName = representiveName,
        //    //    CollectorName = representiveName,
        //    //    MainAreaName = mainAreaName,
        //    //    SaleDate = saleDatePicker ?? DateTime.UtcNow,
        //    //    //serialNumber= 2,
        //    //    total = cashInvoiceTotal,
        //    //    SubAreaName = SubAreaName,
        //    //});

        //} 
        //}
        #endregion

        #region Customers Grid Mouse Double Click Region

        //private void CustomersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    if (CashDataGrid.SelectedItem is ReturnAllCashCustomerInvoicesDTO s)
        //    {
        //        selectedCashCustomerInvoiceIdOfDB = s.serialNumberIdFromDB;
        //        selectedCashCustomerInvoiceId = s.serialNumber;

        //    }
        //}

        #endregion

        #region Delete Cash Customer Btn Click Region

        private void DeleteCashCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CashDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر صف قبل الحذف");
                return;
            }
            var DeleteRes = _cashCustomerInvoiceService.DeleteCashCustomerInvoiceAsync(selectedCashCustomerInvoiceIdOfDB);
            if (DeleteRes.IsCompleted == false)
            {
                returnAllCashCustomersFilter.Remove(returnAllCashCustomersFilter[selectedCashCustomerInvoiceId - 1]);
                CashDataGrid.Items.Refresh();
                MessageBox.Show($"تم حذف فاتوره كاش  ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي فاتوره عميل بسبب خطأ ما");
            }
        }


        #endregion

        #region Customers Grid SelectionChanged Region

        private void CustomersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CashDataGrid.SelectedItem is ReturnAllCashCustomerInvoicesDTO s)
            {
                selectedCashCustomerInvoiceIdOfDB = s.serialNumberIdFromDB;
                selectedCashCustomerInvoiceId = s.serialNumber;

            }
        }

        #endregion

    }
}
