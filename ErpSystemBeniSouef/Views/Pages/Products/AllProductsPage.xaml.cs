using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;
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
        #endregion

        #region Constractor Region

        public AllProductsPage(IProductService productService)
        {
            InitializeComponent();
            _productService = productService; 
            Loaded += async (s, e) => await Loadproducts();
            DataGrid.ItemsSource = observProductsList;
            cb_type.ItemsSource = _productService.GetAllCategories();
            cb_type.SelectedIndex = 0; // اختيار أول عنصر
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //if (
            //    string.IsNullOrWhiteSpace(ProductName.Text) ||
            //   !int.TryParse(RepresentivePercentage.Text, out int startNumber) ||
            //   !int.TryParse(mainPrice.Text, out int mainPrice2) ||
            //   !int.TryParse(subValuePrice.Text, out int subValuePrice2))
            //{
            //    MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            //    return;
            //}

            //string percent = RepresentivePercentage.Text;
            //string scbPri = subValuePrice.Text;
            //string mainPri = mainPrice.Text;
            //int lastId = observProductsList.LastOrDefault()?.Id ?? 0;
            //Product InputProduct = new Product()
            //{
            //    Id = lastId + 1,
            //    Name = ProductName.Text,
            //    Main_purchase_price = double.TryParse(mainPri, out double mainP) ? mainP : 0,
            //    Percentage_Representative = double.TryParse(percent, out double p) ? p : 0,
            //    Sale_purchase_price = double.TryParse(scbPri, out double subp) ? subp : 0,
            //    Type = cb_type.SelectedItem?.ToString(),
            //};
            //observProductsList.Add(InputProduct);

        }

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            //if (DataGrid.SelectedItem is Product Product)
            //{
            //    observProductsList.Remove(Product);
            //    MessageBox.Show("تم الحذف");
            //}
            //else
            //{
            //    MessageBox.Show("من فضلك اختر صف قبل الحذف");
            //}
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
