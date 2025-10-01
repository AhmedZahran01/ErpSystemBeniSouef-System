using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.ViewModel;
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
    /// <summary>
    /// Interaction logic for ReturnToSupplieInvoiceItemsPage.xaml
    /// </summary>
    public partial class ReturnToSupplieInvoiceItemsPage : Page
    {
        public ReturnToSupplieInvoiceItemsPage()
        {
            InitializeComponent();
        }

        #region  Properties Region

        private readonly DtoForReturnSupplierInvoice _Returninvoice;
        int countDisplayNo = 0;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        //private readonly IReturnSupplierInvoiceItemService _returnSupplierInvoiceService;
        private readonly int _comanyNo = (int?)App.Current.Properties["CompanyId"] ?? 1;
        IReadOnlyList<CategoryDto> categories = new List<CategoryDto>();
        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        ObservableCollection<ReturnSupplierInvoiceItemDetailsDto> observReturnInvoiceItemDtosListFiltered = new ObservableCollection<ReturnSupplierInvoiceItemDetailsDto>();
        ObservableCollection<ReturnSupplierInvoiceItemDetailsDto> observReturnInvoiceItemDtosList = new ObservableCollection<ReturnSupplierInvoiceItemDetailsDto>();

        int invoiceIDFromInvoicePage;
        int counId = 0;

        #endregion

        #region Constractor  Region

        public ReturnToSupplieInvoiceItemsPage(DtoForReturnSupplierInvoice Returninvoice, IProductService productService,
                  //IReturnSupplierInvoiceItemService returnSupplierInvoiceService,
                  IMapper mapper)
        {
            InitializeComponent();
            _Returninvoice = Returninvoice;
            _productService = productService;
            //_returnSupplierInvoiceService = returnSupplierInvoiceService;
            DataContext = _Returninvoice;
            _mapper = mapper;
            invoiceIDFromInvoicePage = Returninvoice.Id;
            InvoiceIdTxt.Text = invoiceIDFromInvoicePage.ToString();

            Loaded += async (s, e) =>
            {
                await Loadproducts();
                cbProductType.ItemsSource = await _productService.GetAllCategoriesAsync();
                cbProductType.SelectedIndex = 0;

                cbProduct.ItemsSource = observProductsListFiltered;
                if (observProductsListFiltered.Any())
                    cbProduct.SelectedIndex = 0;

            };
        }

        #endregion

        #region load products to Grid Region

        private async Task Loadproducts()
        {
            //observReturnInvoiceItemDtosListFiltered.Clear();
            //observReturnInvoiceItemDtosList.Clear();
            //countDisplayNo = 0;
            //IReadOnlyList<ProductDto> products = _productService.GetAll();
            //foreach (var product in products)
            //{
            //    observProductsLisLim.Add(product);
            //    observProductsListFiltered.Add(product);
            //}

            //categories = await _productService.GetAllCategoriesAsync();

            //var observCashInvoiceItemDtos = await _returnSupplierInvoiceService.GetInvoiceItemsByInvoiceId(invoiceIDFromInvoicePage);
            //foreach (var product in observCashInvoiceItemDtos)
            //{
            //    product.DisplayId = countDisplayNo + 1;
            //    observReturnInvoiceItemDtosListFiltered.Add(product);
            //    observReturnInvoiceItemDtosList.Add(product);
            //    countDisplayNo++;

            //}
            //counId = observReturnInvoiceItemDtosListFiltered.Count() + 1;

            //dgInvoiceItems.ItemsSource = observReturnInvoiceItemDtosListFiltered;

        }

        #endregion

        #region Add Button Region

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //if (
            //  !decimal.TryParse(txtQuantity.Text, out decimal CommissionRate) ||
            //  !decimal.TryParse(txtPrice.Text, out decimal mainPrice2) ||
            //  !decimal.TryParse(InvoiceIdTxt.Text, out decimal SalePrice2))
            //{
            //    MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            //    return;
            //}

            //CategoryDto selectedCategory = (CategoryDto)cbProductType.SelectedItem;

            //if (selectedCategory == null)
            //{
            //    MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            //    return;
            //}

            //ProductDto selectedProduct = (ProductDto)cbProduct.SelectedItem;

            //if (selectedCategory == null)
            //{
            //    MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            //    return;
            //}

            //CategoryDto pT = (CategoryDto)cbProductType.SelectedItem;
            //ProductDto p = (ProductDto)cbProduct.SelectedItem;
            //string txtQuant = txtQuantity.Text;
            //string Notes = txtNotes.Text;
            //string Price = txtPrice.Text;
            //int Quant = int.TryParse(txtQuant, out int pa) ? pa : 0;
            //decimal PriceUnit = decimal.TryParse(Price, out decimal pr) ? pr : 0;

            //string textPrice = txtPrice.Text;

            //ReturnSupplierInvoiceItemDetailsDto invoiceItemDetails = new ReturnSupplierInvoiceItemDetailsDto()
            //{

            //    InvoiceId = invoiceIDFromInvoicePage,
            //    ProductName = p.ProductName,
            //    ProductType = pT.Name,
            //    ProductTypeName = pT.Name,
            //    ProductTypeId = pT.Id,
            //    Quantity = Quant,
            //    Notes = Notes,
            //    UnitPrice = PriceUnit,
            //    DisplayId = counId,
            //    ProductId = p.Id,
            //};
            //counId++;
            //AddReturnSupplierInvoiceItemsDto d = new AddReturnSupplierInvoiceItemsDto();
            //ReturnSupplierInvoiceItemDto ReturnInvoiceItemDto = new ReturnSupplierInvoiceItemDto()
            //{
            //    LineTotal = invoiceItemDetails.LineTotal,
            //    Notes = Notes,
            //    UnitPrice = PriceUnit,
            //    ProductId = p.Id,
            //    ProductTypeName = pT.Name,
            //    Quantity = Quant,
            //    Id = counId
            //};
            //d.invoiceItemDtos = new List<ReturnSupplierInvoiceItemDto>();
            //d.invoiceItemDtos.Add(ReturnInvoiceItemDto);
            ////   _returnSupplierInvoiceService.AddInvoiceItems( d);

            //observReturnInvoiceItemDtosListFiltered.Add(invoiceItemDetails);
            //observReturnInvoiceItemDtosList.Add(invoiceItemDetails);
            //dgInvoiceItems.ItemsSource = observReturnInvoiceItemDtosListFiltered;

            //MessageBox.Show("تم اضافه عنصر للفاتوره الكاش بنجاح");
            //return;
        }

        #endregion

        #region Product Type_Selection Changed Region
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
        #endregion

        #region Delete Button Region

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            //if (dgInvoiceItems.SelectedItems.Count == 0)
            //{
            //    MessageBox.Show("من فضلك اختر عنصر محدد للحذف");
            //    return;
            //}
            //List<ReturnSupplierInvoiceItemDetailsDto> selectedInvoiceItemList = dgInvoiceItems.SelectedItems.Cast<ReturnSupplierInvoiceItemDetailsDto>().ToList();

            //int deletedCount = 0;
            //foreach (var selectedInvoiceItem in selectedInvoiceItemList)
            //{
            //    if (selectedInvoiceItem.Id != 0)
            //    {
            //        bool res = _returnSupplierInvoiceService.SoftDelete(selectedInvoiceItem.Id, selectedInvoiceItem.LineTotal, invoiceIDFromInvoicePage);
            //        if (!res)
            //        {
            //            MessageBox.Show("حدث خطأ أثناء التعديل");
            //            return;
            //        }
            //        else
            //        {
            //            observReturnInvoiceItemDtosListFiltered.Remove(selectedInvoiceItem);
            //            counId--;
            //        }
            //    }
            //    else
            //    {
            //        observReturnInvoiceItemDtosListFiltered.Remove(selectedInvoiceItem);
            //        counId--;

            //    }
            //    deletedCount++;
            //}
            //if (deletedCount == 0)
            //{
            //    MessageBox.Show($"  لم يتم حذف اي عنصر  ");

            //}
            //MessageBox.Show($"  تم حذف  {deletedCount} عنصر   ");
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
            //var NewAddedItems = observReturnInvoiceItemDtosListFiltered.Where(i => i.Id == 0).ToList();
            //AddReturnSupplierInvoiceItemsDto addReturnInvoiceItemsDto = new AddReturnSupplierInvoiceItemsDto();
            //addReturnInvoiceItemsDto.Id = invoiceIDFromInvoicePage;
            //addReturnInvoiceItemsDto.invoiceItemDtos = new List<ReturnSupplierInvoiceItemDto>();
            //decimal InvoiceTotalPrice = 0;
            //foreach (var NewAddedItem in NewAddedItems)
            //{
            //    ReturnSupplierInvoiceItemDto ReturnInvoiceItemsDto = _mapper.Map<ReturnSupplierInvoiceItemDto>(NewAddedItem);

            //    InvoiceTotalPrice += NewAddedItem.LineTotal;
            //    CategoryDto category = categories.Where(i => i.Id == NewAddedItem.ProductTypeId).FirstOrDefault();

            //    addReturnInvoiceItemsDto.invoiceItemDtos.Add(ReturnInvoiceItemsDto);
            //}
            //addReturnInvoiceItemsDto.InvoiceTotalPrice = InvoiceTotalPrice;
            //bool res = await _returnSupplierInvoiceService.AddInvoiceItems(addReturnInvoiceItemsDto);
            //if (res)
            //{
            //    MessageBox.Show("تم تعديل الفاتوره بنجاح");
            //    return;
            //}

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
            var filtered = observReturnInvoiceItemDtosList
                .Where(i => i.ProductName != null && i.ProductName.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observReturnInvoiceItemDtosListFiltered.Clear();
            foreach (var item in filtered)
            {
                observReturnInvoiceItemDtosListFiltered.Add(item);
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                observReturnInvoiceItemDtosListFiltered.Clear();
                foreach (var item in observReturnInvoiceItemDtosList)
                {
                    observReturnInvoiceItemDtosListFiltered.Add(item);
                }
                return;
            }


        }

        #endregion

        #region BackBtn_Click Region
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //var supplierService = App.AppHost.Services.GetRequiredService<ISupplierService>();
            //var cashInvoiceService = App.AppHost.Services.GetRequiredService<IReturnSupplierInvoiceService>();

            //var Dashboard = new Return_to_supplie(supplierService, cashInvoiceService);
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion
    }
}
