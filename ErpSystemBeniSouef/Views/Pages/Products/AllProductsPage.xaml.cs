using AutoMapper;
using ErpSystemBeniSouef.Core.Contract; 
using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.HelperFunctions;
using ErpSystemBeniSouef.ViewModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls; 
namespace ErpSystemBeniSouef.Views.Pages.Products
{
    public partial class AllProductsPage : Page
    {
        #region Global Properties Region

        //private readonly int _comanyNo = AppGlobalCompanyId.CompanyId;
        private readonly int _comanyNo = (int?)App.Current.Properties["CompanyId"]??1;

        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        IReadOnlyList<CategoryDto> categories = new List<CategoryDto>();
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor Region

        public AllProductsPage( IProductService productService, IMapper mapper)
        {
            InitializeComponent(); 
            _productService = productService;
            _mapper = mapper;
            //Loaded += async (s, e) => await Loadproducts();
            //cb_type.ItemsSource = _productService.GetAllCategories();
            //cb_type.SelectedIndex = 0;
            Loaded += async (s, e) =>
            {
                await Loadproducts();
                cb_type.ItemsSource = await _productService.GetAllCategoriesAsync();
                cb_type.SelectedIndex = 0;
            };

            AllProductsDataGrid.ItemsSource = observProductsListFiltered;
        }
        #endregion

        #region load products to Grid Region

        private async Task Loadproducts()
        {
            IReadOnlyList<ProductDto> products = await _productService.GetAllProductsAsync(_comanyNo);
            foreach (var product in products)
            {
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
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
            CategoryDto selectedCategory = (CategoryDto)cb_type.SelectedItem;
             
            if (selectedCategory == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            string percent = RepresentivePercentage.Text;
            string salesPri = SalePrice.Text;
            string mainPri = mainPrice.Text;
            string Product_Name = ProductName.Text;

            ProductDto CheckProductNameFounded = observProductsLisLim.Where(n => n.ProductName == Product_Name).FirstOrDefault();
            if (CheckProductNameFounded is not null )
            {
                MessageBox.Show(" الاسم مستخدم من قيل ");
                return;
            }
            //return;

            CreateProductDto InputProduct = new CreateProductDto()
            {
                ProductName = ProductName.Text,
                PurchasePrice = decimal.TryParse(mainPri, out decimal mainP) ? mainP : 0,
                CommissionRate = decimal.TryParse(percent, out decimal p) ? p : 0,
                SalePrice = decimal.TryParse(salesPri, out decimal subp) ? subp : 0,
                CategoryId = selectedCategory.Id,
                CompanyId = _comanyNo
            };

            ProductDto CreateproductDtoRespons = _productService.Create(InputProduct);
            if (CreateproductDtoRespons is null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة المنتج بنجاح");

            SalePrice.Clear();
            mainPrice.Clear();
            RepresentivePercentage.Clear();
            ProductName.Clear();

            //int lastId = observProductsLisLim.LastOrDefault()?.Id ?? 0;
            //InputProduct.Id = lastId + 1;
            //var productD = _mapper.Map<ProductDto>(InputProduct);
            //CategoryDto categoryDto = categories.
            //       Where(i => i.Id == InputProduct.CategoryId).FirstOrDefault();
            //productD.Category = categoryDto;

            observProductsListFiltered.Add(CreateproductDtoRespons);
            observProductsLisLim.Add(CreateproductDtoRespons);

        }

        #endregion

        #region Delete Button Region

        private async void DeleteButton_Click_1(object sender, RoutedEventArgs e)
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
                bool success = await _productService.SoftDeleteAsync(item.Id);
                if (success)
                {
                    observProductsLisLim.Remove(item);
                    observProductsListFiltered.Remove(item);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                string ValueOfString = "منتج ";
                if (deletedCount > 1)
                    ValueOfString = "من المنتجات ";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي منتج بسبب خطأ ما");
            }

        }
        #endregion

        #region Search By Item FullName  Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observProductsLisLim
                .Where(i => i.ProductName != null && i.ProductName.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observProductsListFiltered.Clear();
            foreach (var item in filtered)
            {
                observProductsListFiltered.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.ProductName);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
                AllProductsDataGrid.ItemsSource = filtered;
                SuggestionsPopup.IsOpen = true;
            }
            else
            {
                SuggestionsPopup.IsOpen = false;
            }

        }

        #endregion

        #region Suggestions Items List Box Region

        private void SuggestionsItemsListBoxForText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuggestionsItemsListBox.SelectedItem != null)
            {
                string fullname = (string)SuggestionsItemsListBox.SelectedItem;
                SearchByItemTextBox.Text = fullname;
                SuggestionsPopup.IsOpen = false;

                // فلترة DataGrid بالاختيار
                var filtered = observProductsLisLim
                    .Where(i => i.ProductName == fullname)
                    .ToList();

                observProductsListFiltered.Clear();
                foreach (var item in filtered)
                {
                    observProductsListFiltered.Add(item);
                }
            }
        }
        #endregion
         
        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion
         
        #region dgMainRegions_SelectionChanged Region

        private void dgAllProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllProductsDataGrid.SelectedItem is ProductDto selected)
            {
                ProductName.Text = selected.ProductName;
                RepresentivePercentage.Text = selected.CommissionRate.ToString();
                mainPrice.Text = selected.PurchasePrice.ToString();
                SalePrice.Text = selected.SalePrice.ToString();
                cb_type.SelectedValue = selected.CategoryId;
                //cb_type.SelectedValue = selected.Category;

                editBtn.Visibility = Visibility.Visible;
            }
        }


        #endregion

        #region Btn Edit Click  Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (AllProductsDataGrid.SelectedItem is not ProductDto selected)
            {
                MessageBox.Show("من فضلك اختر منطقة للتعديل");
                return;
            }

            string newName = ProductName.Text.Trim();

            if (!decimal.TryParse(RepresentivePercentage.Text, out decimal NewRepresentivePercentageValue))
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }

            if (!decimal.TryParse(mainPrice.Text, out decimal newmainPriceValue))
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }

            if (!decimal.TryParse(SalePrice.Text, out decimal newSalePriceValue))
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }


            // DTO للتحديث
            //var updateDto = new UpdateProductDto()
            //{
            //    Id = selected.Id,
            //    ProductName = newName,
            //    CategoryId = selected.CategoryId,
            //    CommissionRate = selected.CommissionRate,
            //    SalePrice = selected.SalePrice,
            //    PurchasePrice = selected.PurchasePrice

            //};

            var updateDto = new UpdateProductDto()
            {
                Id = selected.Id,
                ProductName = newName,
                CategoryId = ((CategoryDto)cb_type.SelectedItem).Id,
                CommissionRate = NewRepresentivePercentageValue,
                SalePrice = newSalePriceValue,
                PurchasePrice = newmainPriceValue
            };

            bool success = _productService.Update(updateDto);

            if (success)
            {
                CategoryDto categoryDto = categories.FirstOrDefault(i => i.Id == selected.CategoryId );


                // عدل العنصر في ObservableCollection
                //selected.ProductName = newName;
                //selected.CommissionRate = selected.CommissionRate;
                //selected.SalePrice = selected.SalePrice;
                //selected.PurchasePrice = selected.PurchasePrice;
                //selected.CategoryId = selected.CategoryId;
                //selected.Category = categoryDto;

                selected.ProductName = newName; 
                selected.SalePrice = selected.SalePrice;
                selected.CommissionRate = NewRepresentivePercentageValue;
                selected.PurchasePrice = newmainPriceValue;
                selected.CategoryId = ((CategoryDto)cb_type.SelectedItem).Id;
                selected.Category = ((CategoryDto)cb_type.SelectedItem);


                MessageBox.Show("تم تعديل المنطقة بنجاح");
                AllProductsDataGrid.Items.Refresh(); // لتحديث الجدول
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }

        #endregion

    }

}
