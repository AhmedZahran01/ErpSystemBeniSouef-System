using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.MainAreaServices;
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

namespace ErpSystemBeniSouef.Views.Pages.Products
{
    public partial class AllProductsPage : Page
    {
        #region Global Properties Region

        ObservableCollection<ProductDto> observProductsList = new ObservableCollection<ProductDto>();
        IReadOnlyList<CategoryDto> categories = new List<CategoryDto>();
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor Region

        public AllProductsPage(IProductService productService, IMapper mapper)
        {
            InitializeComponent();
            _productService = productService;
            _mapper = mapper;
            Loaded += async (s, e) => await Loadproducts();
            AllProductsDataGrid.ItemsSource = observProductsList;
            cb_type.ItemsSource = _productService.GetAllCategories();
            cb_type.SelectedIndex = 0;
        }
        #endregion

        #region Retrieve products to Grid Region

        private async Task Loadproducts()
        {
            IReadOnlyList<ProductDto> products = await _productService.GetAllAsync();
            foreach (var product in products)
            {
                observProductsList.Add(product);
            }
            categories = await _productService.GetAllCategoriesAsync();
        }

        #endregion

        #region Add Button Region

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(ProductName.Text) ||
               !decimal.TryParse(RepresentivePercentage.Text, out decimal CommissionRate) ||
               !decimal.TryParse(mainPrice.Text, out decimal mainPrice2) ||
               !decimal.TryParse(SalePrice.Text, out decimal SalePrice2))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            CategoryDto SelectedCategory = (CategoryDto)cb_type.SelectedValue;
            if (SelectedCategory == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            string percent = RepresentivePercentage.Text;
            string salesPri = SalePrice.Text;
            string mainPri = mainPrice.Text;
            CreateProductDto InputProduct = new CreateProductDto()
            {
                ProductName = ProductName.Text,
                PurchasePrice = decimal.TryParse(mainPri, out decimal mainP) ? mainP : 0,
                CommissionRate = decimal.TryParse(percent, out decimal p) ? p : 0,
                SalePrice = decimal.TryParse(salesPri, out decimal subp) ? subp : 0,
                CategoryId = SelectedCategory.Id,
            };

            bool productDtoRespons = _productService.Create(InputProduct);
            if (!productDtoRespons)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("  تم اضافه المنطقه الاساسية ");

            SalePrice.Clear();
            mainPrice.Clear();
            RepresentivePercentage.Clear();
            ProductName.Clear();

            int lastId = observProductsList.LastOrDefault()?.Id ?? 0;
            InputProduct.Id = lastId + 1;
            var productD = _mapper.Map<ProductDto>(InputProduct);
            CategoryDto categoryDto = categories.
                   Where(i => i.Id == InputProduct.CategoryId).FirstOrDefault();
            productD.Category = categoryDto;
            observProductsList.Add(productD);

        }

        #endregion

        #region Delete Button Region

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            bool checkSoftDelete = false;
            if (AllProductsDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }
            List<ProductDto> selectedItemsDto = AllProductsDataGrid.SelectedItems.Cast<ProductDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = _productService.SoftDelete(item.Id);
                if (success)
                {
                    observProductsList.Remove(item);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                string ValueOfString = "منتج ";
                if (deletedCount > 1)
                    ValueOfString = "منتجات ";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي منطقة أساسية بسبب خطأ ما");
            }

        }
        #endregion

        #region Search By Item FullName  Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text.ToLower();

            // فلترة النتائج
            var filtered = observProductsList
                .Where(i => i.ProductName != null && i.ProductName.ToLower().Contains(query))
                .ToList();
            //// تحديث الـ DataGrid
            //observalMainRegionsDtoforFilter.Clear();
            //foreach (var item in observalMainRegionsDtoforFilter)
            //{
            //    observalMainRegionsDtoforFilter.Add(item);
            //}
        }

        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        #endregion

    }

}
