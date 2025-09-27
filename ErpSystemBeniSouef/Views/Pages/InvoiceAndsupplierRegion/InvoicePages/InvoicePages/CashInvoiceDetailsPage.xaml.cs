using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Controls;

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages
{
    /// <summary>
    /// Interaction logic for CashInvoiceDetailsPage.xaml
    /// </summary>
    public partial class CashInvoiceDetailsPage : Page
    {
        #region  Properties Region

        private readonly ReturnCashInvoiceDto _invoice;
        int count = 0;
        private readonly IProductService _productService;
        private readonly ICashInvoiceService _cashInvoiceService;
        private readonly IMapper _mapper;
        private readonly int _comanyNo = (int?)App.Current.Properties["CompanyId"] ?? 1;
        IReadOnlyList<CategoryDto> categories = new List<CategoryDto>();
        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        ObservableCollection<InvoiceItemDetailsDto> observCashInvoiceItemDtosFiltered = new ObservableCollection<InvoiceItemDetailsDto>();
        int invoiceIDFromInvoicePage;
        int counId = 0;

        #endregion

        #region Constractor  Region

        public CashInvoiceDetailsPage(ReturnCashInvoiceDto invoice, IProductService productService,
                               ICashInvoiceService cashInvoiceService, IMapper mapper)
        {
            InitializeComponent();
            _invoice = invoice; DataContext = _invoice; _productService = productService;
            _cashInvoiceService = cashInvoiceService;
            _mapper = mapper;
            invoiceIDFromInvoicePage = invoice.Id; InvoiceIdTxt.Text = invoiceIDFromInvoicePage.ToString();

            Loaded += async (s, e) =>
            {
                await Loadproducts();
                cbProductType.ItemsSource = await _productService.GetAllCategoriesAsync();
                cbProductType.SelectedIndex = 0;

                cbProduct.ItemsSource = observProductsListFiltered;
                cbProduct.SelectedIndex = 0;
            };
        }
        #endregion
          
        #region load products to Grid Region

        private async Task Loadproducts()
        {
            IReadOnlyList<ProductDto> products = _productService.GetAll();
            foreach (var product in products)
            {
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
            }

            categories = await _productService.GetAllCategoriesAsync();

            var observCashInvoiceItemDtos = await _cashInvoiceService.GetInvoiceItemsByInvoiceId(invoiceIDFromInvoicePage);
            foreach (var product in observCashInvoiceItemDtos)
            {
                product.DisplayId = count + 1;
                observCashInvoiceItemDtosFiltered.Add(product);
                count++;

            }
            counId = observCashInvoiceItemDtosFiltered.Count() + 1;

            dgInvoiceItems.ItemsSource = observCashInvoiceItemDtosFiltered;
        }

        #endregion
         
        #region BackBtn_Click Region
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        #endregion

        #region cb Product Type Selection Changed Region

        private void cbProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProductType.SelectedItem is CategoryDto s)
            {
                observProductsListFiltered.Clear();
                var filtered = observProductsLisLim.Where(p => p.CategoryId == s.Id).ToList();
                foreach (var product in filtered)
                {
                    observProductsListFiltered.Add(product);
                    cbProduct.ItemsSource = observProductsListFiltered;
                }
                cbProduct.SelectedIndex = 0;

            }

        }

        #endregion

        #region Add Button Region

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (
              !decimal.TryParse(txtQuantity.Text, out decimal CommissionRate) ||
              !decimal.TryParse(txtPrice.Text, out decimal mainPrice2) ||
              !decimal.TryParse(InvoiceIdTxt.Text, out decimal SalePrice2))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            CategoryDto selectedCategory = (CategoryDto)cbProductType.SelectedItem;

            if (selectedCategory == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            ProductDto selectedProduct = (ProductDto)cbProduct.SelectedItem;

            if (selectedCategory == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            CategoryDto pT = (CategoryDto)cbProductType.SelectedItem;
            ProductDto p = (ProductDto)cbProduct.SelectedItem;
            string txtQuant = txtQuantity.Text;
            string Notes = txtNotes.Text;
            string Price = txtPrice.Text;
            int Quant = int.TryParse(txtQuant, out int pa) ? pa : 0;
            int PriceUnit = int.TryParse(Price, out int pr) ? pr : 0;

            string textPrice = txtPrice.Text;

            InvoiceItemDetailsDto invoiceItemDetails = new InvoiceItemDetailsDto()
            {
                InvoiceId = invoiceIDFromInvoicePage,
                ProductName = p.ProductName,
                ProductType = pT.Name,
                ProductTypeId = pT.Id,
                Quantity = Quant,
                Notes = Notes,
                UnitPrice = PriceUnit,
                DisplayId = counId,
                ProductId = p.Id,
            };
            counId++;
            AddCashInvoiceItemsDto d = new AddCashInvoiceItemsDto();
            CashInvoiceItemDto cashInvoiceItemDto = new CashInvoiceItemDto()
            {
                LineTotal = invoiceItemDetails.LineTotal,
                Note = Notes,
                UnitPrice = PriceUnit,
                ProductId = p.Id,
                ProductTypeId = pT.Id,
                Quantity = Quant,
                Id = counId
            };
            d.invoiceItemDtos = new List<CashInvoiceItemDto>();
            d.invoiceItemDtos.Add(cashInvoiceItemDto);
            //_cashInvoiceService.AddInvoiceItems( d);

            observCashInvoiceItemDtosFiltered.Add(invoiceItemDetails);
            dgInvoiceItems.ItemsSource = observCashInvoiceItemDtosFiltered;

            return;
        }

        #endregion

        #region Delete Button Region

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Quantity Text Changed Region

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotal();
        }

        #endregion

        #region Price Text Changed Region

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotal();
        }

        #endregion

        #region Update Total Region

        private void UpdateTotal()
        {
            int quantity = int.TryParse(txtQuantity.Text, out int q) ? q : 0;
            decimal price = decimal.TryParse(txtPrice.Text, out decimal p) ? p : 0;
            decimal total = quantity * price;

            txtTotal.Text = total.ToString("0.00"); // بصيغة رقمية
        }

        #endregion

        #region Add Final Invoice Button Region

        private void AddFinalInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var NewAddedItems = observCashInvoiceItemDtosFiltered.Where(i => i.Id == 0).ToList();
            AddCashInvoiceItemsDto addCashInvoiceItemsDto = new AddCashInvoiceItemsDto();
            addCashInvoiceItemsDto.Id = invoiceIDFromInvoicePage;
            addCashInvoiceItemsDto.invoiceItemDtos = new List<CashInvoiceItemDto>();

            foreach (var NewAddedItem in NewAddedItems)
            {
                CashInvoiceItemDto cashInvoiceItemsDto = _mapper.Map<CashInvoiceItemDto>(NewAddedItem);

                addCashInvoiceItemsDto.invoiceItemDtos.Add(cashInvoiceItemsDto);

                var res = _cashInvoiceService.AddInvoiceItems(addCashInvoiceItemsDto);


            }

        }

        #endregion
         
        #region cb Product Selection Changed Region

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)cbProduct.SelectedItem;
            if (selectedProduct is not null)
            {
                txtPrice.Text = selectedProduct.PurchasePrice.ToString() ?? "";

            }
        }

        #endregion


    }
}
