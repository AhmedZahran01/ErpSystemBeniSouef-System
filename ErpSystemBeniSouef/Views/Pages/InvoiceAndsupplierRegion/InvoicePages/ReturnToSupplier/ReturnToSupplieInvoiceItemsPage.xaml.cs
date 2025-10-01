using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.ReturnToSupplieInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer.Migrations;
using ErpSystemBeniSouef.Service.InvoiceServices.CashInvoiceService;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages;
using Microsoft.Extensions.DependencyInjection;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.ReturnToSupplier
{ 
    public partial class ReturnToSupplieInvoiceItemsPage : Page
    {

        #region  Properties Region

        private readonly DtoForReturnSupplierInvoice _invoice;
        int countDisplayNo = 0;
        private readonly IProductService _productService;
        private readonly IReturnSupplierInvoiceItemService _returnSupplierInvoiceItem;
        private readonly IMapper _mapper;
        private readonly int _comanyNo = (int?)App.Current.Properties["CompanyId"] ?? 1;
        IReadOnlyList<CategoryDto> categories = new List<CategoryDto>();
        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        ObservableCollection<ReturnSupplierInvoiceItemDetailsDto> observCashInvoiceItemDtosFiltered = new ObservableCollection<ReturnSupplierInvoiceItemDetailsDto>();
        ObservableCollection<ReturnSupplierInvoiceItemDetailsDto> observCashInvoiceItemDtosList = new ObservableCollection<ReturnSupplierInvoiceItemDetailsDto>();
        int invoiceIDFromInvoicePage;
        int counId = 0;

        #endregion

        #region Constractor  Region

        public ReturnToSupplieInvoiceItemsPage(DtoForReturnSupplierInvoice invoice, IProductService productService,
                               IReturnSupplierInvoiceItemService cashInvoiceService, IMapper mapper)
        {
            InitializeComponent();
            _invoice = invoice; DataContext = _invoice; _productService = productService;
            _returnSupplierInvoiceItem = cashInvoiceService;
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

            var observCashInvoiceItemDtos = await _returnSupplierInvoiceItem.GetInvoiceItemsByInvoiceId(invoiceIDFromInvoicePage);
            foreach (var product in observCashInvoiceItemDtos)
            {
                product.DisplayId = countDisplayNo + 1;
                observCashInvoiceItemDtosFiltered.Add(product);
                observCashInvoiceItemDtosList.Add(product);
                countDisplayNo++;

            }
            counId = observCashInvoiceItemDtosFiltered.Count() + 1;

            dgInvoiceItems.ItemsSource = observCashInvoiceItemDtosFiltered;
        }

        #endregion

        #region BackBtn_Click Region
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var supplierService = App.AppHost.Services.GetRequiredService<ISupplierService>();
            var returnSupplierInvoice = App.AppHost.Services.GetRequiredService<IReturnSupplierInvoiceService>();

            var Dashboard = new ReturnToSupplieInvoicePage(supplierService, returnSupplierInvoice);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
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
            decimal PriceUnit = decimal.TryParse(Price, out decimal pr) ? pr : 0;

            string textPrice = txtPrice.Text;

            ReturnSupplierInvoiceItemDetailsDto invoiceItemDetails = new ReturnSupplierInvoiceItemDetailsDto()
            {
                InvoiceId = invoiceIDFromInvoicePage,
                ProductName = p.ProductName,
                ProductType = pT.Name,
                ProductTypeName = pT.Name,
                ProductTypeId = pT.Id,
                Quantity = Quant,
                Notes = Notes,
                UnitPrice = PriceUnit,
                DisplayId = counId,
                ProductId = p.Id,
            };
            counId++;
            AddReturnSupplierInvoiceItemsDto d = new AddReturnSupplierInvoiceItemsDto();
            ReturnSupplierInvoiceItemDto cashInvoiceItemDto = new ReturnSupplierInvoiceItemDto()
            {
                LineTotal = invoiceItemDetails.LineTotal,
                Note = Notes,
                UnitPrice = PriceUnit,
                ProductId = p.Id,
                ProductTypeName = pT.Name,
                Quantity = Quant,
                Id = counId
            };
            d.invoiceItemDtos = new List<ReturnSupplierInvoiceItemDto>();
            d.invoiceItemDtos.Add(cashInvoiceItemDto);
            //_cashInvoiceService.AddInvoiceItems( d);

            observCashInvoiceItemDtosFiltered.Add(invoiceItemDetails);
            observCashInvoiceItemDtosList.Add(invoiceItemDetails);
            dgInvoiceItems.ItemsSource = observCashInvoiceItemDtosFiltered;

            MessageBox.Show("تم اضافه عنصر للفاتوره الكاش بنجاح");
            return;
        }

        #endregion

        #region Delete Button Region

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgInvoiceItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر عنصر محدد للحذف");
                return;
            }
            List<ReturnSupplierInvoiceItemDetailsDto> selectedInvoiceItemList = dgInvoiceItems.SelectedItems.Cast<ReturnSupplierInvoiceItemDetailsDto>().ToList();

            int deletedCount = 0;
            foreach (var selectedInvoiceItem in selectedInvoiceItemList)
            {
                if (selectedInvoiceItem.Id != 0)
                {
                    bool res = _returnSupplierInvoiceItem.SoftDelete(selectedInvoiceItem.Id, selectedInvoiceItem.LineTotal, invoiceIDFromInvoicePage);
                    if (!res)
                    {
                        MessageBox.Show("حدث خطأ أثناء التعديل");
                        return;
                    }
                    else
                    {
                        observCashInvoiceItemDtosFiltered.Remove(selectedInvoiceItem);
                        counId--;
                    }
                }
                else
                {
                    observCashInvoiceItemDtosFiltered.Remove(selectedInvoiceItem);
                    counId--;

                }
                deletedCount++;
            }
            if (deletedCount == 0)
            {
                MessageBox.Show($"  لم يتم حذف اي عنصر  ");

            }
            MessageBox.Show($"  تم حذف  {deletedCount} عنصر   ");
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

        private async void AddFinalInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var NewAddedItems = observCashInvoiceItemDtosFiltered.Where(i => i.Id == 0).ToList();
            AddReturnSupplierInvoiceItemsDto addCashInvoiceItemsDto = new AddReturnSupplierInvoiceItemsDto();
            addCashInvoiceItemsDto.Id = invoiceIDFromInvoicePage;
            addCashInvoiceItemsDto.invoiceItemDtos = new List<ReturnSupplierInvoiceItemDto>();
            decimal InvoiceTotalPrice = 0;
            foreach (var NewAddedItem in NewAddedItems)
            {
                ReturnSupplierInvoiceItemDto cashInvoiceItemsDto = _mapper.Map<ReturnSupplierInvoiceItemDto>(NewAddedItem);

                InvoiceTotalPrice += NewAddedItem.LineTotal;
                CategoryDto category = categories.Where(i => i.Id == NewAddedItem.ProductTypeId).FirstOrDefault();

                addCashInvoiceItemsDto.invoiceItemDtos.Add(cashInvoiceItemsDto);
            }
            addCashInvoiceItemsDto.InvoiceTotalPrice = InvoiceTotalPrice;
            bool res = await _returnSupplierInvoiceItem.AddInvoiceItems(addCashInvoiceItemsDto);
            if (res)
            {
                MessageBox.Show("تم تعديل الفاتوره الكاش بنجاح");
                return;
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

        #region Search By Item FullName  Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observCashInvoiceItemDtosList
                .Where(i => i.ProductName != null && i.ProductName.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observCashInvoiceItemDtosFiltered.Clear();
            foreach (var item in filtered)
            {
                observCashInvoiceItemDtosFiltered.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.ProductName);
            if (suggestions.Any())
            {
                dgInvoiceItems.ItemsSource = filtered;
                //SuggestionsItemsListBox.ItemsSource = suggestions;
                //SuggestionsPopup.IsOpen = true;
            }
            else
            {
                //SuggestionsPopup.IsOpen = false;
            }

        }

        #endregion


    }
}
