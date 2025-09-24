using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;
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
    /// Interaction logic for CashInvoiceDetailsPage.xaml
    /// </summary>
    public partial class CashInvoiceDetailsPage : Page
    {
        #region  Properties Region

        private readonly ReturnCashInvoiceDto _invoice;
        private readonly IProductService _productService;
        private readonly ICashInvoiceService _cashInvoiceService;
        private readonly int _comanyNo = (int?)App.Current.Properties["CompanyId"] ?? 1;
        IReadOnlyList<CategoryDto> categories = new List<CategoryDto>();
        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        ObservableCollection<InvoiceItemDetailsDto> observCashInvoiceItemDtosListFiltered = new ObservableCollection<InvoiceItemDetailsDto>();
        int invoiceIDFromPage;
        int counId = 0;

        #endregion

        #region Constractor  Region

        public CashInvoiceDetailsPage(ReturnCashInvoiceDto invoice, IProductService productService , 
                  ICashInvoiceService cashInvoiceService)
        {
            InitializeComponent();
            _invoice = invoice;
            _productService = productService;
            _cashInvoiceService = cashInvoiceService;
            DataContext = _invoice;
            invoiceIDFromPage = invoice.Id;
            InvoiceIdTxt.Text = invoiceIDFromPage.ToString();

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
            IReadOnlyList<ProductDto> products =  _productService.GetAll();
            foreach (var product in products)
            {
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
            }
            categories = await _productService.GetAllCategoriesAsync();

            //var observCashInvoiceItemDtosListFilteredList =  await _cashInvoiceService.GetInvoiceItemsByInvoiceId(invoiceIDFromPage);
            //foreach (var product in observCashInvoiceItemDtosListFilteredList)
            //{
            //    observCashInvoiceItemDtosListFiltered.Add(product); 
            //}

            //dgInvoiceItems.ItemsSource = observCashInvoiceItemDtosListFiltered;
        }

        #endregion




















        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

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

            }

        }
         
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryDto pT = (CategoryDto) cbProductType.SelectedItem;
            ProductDto p =(ProductDto) cbProduct.SelectedItem;
            string txtQuant = txtQuantity.Text;
            string Notes = txtNotes.Text;
            string Price = txtPrice.Text;
            int Quant = int.TryParse(txtQuant, out int pa) ? pa : 0;
            int PriceUnit = int.TryParse(Price, out int pr) ? pr : 0;

            string textPrice = txtPrice.Text;

            InvoiceItemDetailsDto invoiceItemDetails = new InvoiceItemDetailsDto()
            {
                InvoiceId = invoiceIDFromPage,
                ProductName = p.ProductName,
                ProductType = pT.Name,
                Quantity = Quant,
                Notes = Notes ,
                UnitPrice = PriceUnit ,
                Id = counId+1
            };
            AddCashInvoiceItemsDto d = new AddCashInvoiceItemsDto();
            CashInvoiceItemDto cashInvoiceItemDto = new CashInvoiceItemDto()
            {
                TotalAmount = invoiceItemDetails.LineTotal,
                Note = Notes,
                UnitPrice = PriceUnit,
                ProductId = p.Id,
                Quantity = Quant ,
                Id = counId
            };
            d.invoiceItemDtos = new List<CashInvoiceItemDto>();
             d.invoiceItemDtos.Add(cashInvoiceItemDto);
             //_cashInvoiceService.AddInvoiceItems( d);

            observCashInvoiceItemDtosListFiltered.Add(invoiceItemDetails);
            dgInvoiceItems.ItemsSource = observCashInvoiceItemDtosListFiltered;

            return;
        }

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotal();
        }

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotal();
        }


        private void UpdateTotal()
        {
            int quantity = int.TryParse(txtQuantity.Text, out int q) ? q : 0;
            decimal price = decimal.TryParse(txtPrice.Text, out decimal p) ? p : 0;
            decimal total = quantity * price;

            txtTotal.Text = total.ToString("0.00"); // بصيغة رقمية
        }

        private void AddFinalInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
          var x =  observCashInvoiceItemDtosListFiltered;
        }
    }
}
