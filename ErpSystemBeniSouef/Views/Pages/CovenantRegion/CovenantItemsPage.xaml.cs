using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Covenant;
using ErpSystemBeniSouef.Core.DTOs.Covenant;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ErpSystemBeniSouef.Views.Pages.CovenantRegion
{
    /// <summary>
    /// Interaction logic for CovenantItemsPage.xaml
    /// </summary>
    public partial class CovenantItemsPage : Page
    {
        private readonly IProductService _productService;
        private readonly ICovenantService _covenantService;
        private readonly int _covenantId;
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        public IReadOnlyList<CategoryDto> categoryDtos = new List<CategoryDto>();
        ObservableCollection<CovenantItemsDto> CovenantItemsGridList = new ObservableCollection<CovenantItemsDto>();
        ObservableCollection<ReturnCovenantItem> ReturnCovenantItemsGridList = new ObservableCollection<ReturnCovenantItem>();

        public CovenantItemsPage(int covenantId, IProductService productService, ICovenantService covenantService)
        {
            InitializeComponent();
            _productService = productService;
            _covenantService = covenantService;
            _covenantId = covenantId;
            Loaded += async (s, e) =>
            {
                await loadData();
                productTypeComo.ItemsSource = categoryDtos;
                productTypeComo.SelectedIndex = 0;
                CovenantItemsDataGrid.ItemsSource = CovenantItemsGridList;

                //ConvenantDataGrid.ItemsSource = returnCovenants;
                //RepresenComoBox.SelectedIndex = 0;

            };
        }

        public async Task loadData()
        {
            categoryDtos = await _productService.GetAllCategoriesAsync();
            List<ReturnCovenantItem> s = await _covenantService.GetCovenantItemsByCovenantId(_covenantId);
            ReturnCovenantItemsGridList = new ObservableCollection<ReturnCovenantItem>(s);
            //CovenantItemsGridList = new ObservableCollection<CovenantItemsDto>(ReturnCovenantItemsGridList);

            int index = 1;
            foreach (var item in ReturnCovenantItemsGridList)
            {
                CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                });

                CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                });
                CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                });
                CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                });
                CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                });
                CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                }); CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                }); CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                }); CovenantItemsGridList.Add(new CovenantItemsDto()
                {
                    CategoryId = item.CategoryId,
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    DisplayUIId = index++

                });

            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var covenantService = App.AppHost.Services.GetRequiredService<ICovenantService>();
            var Dashboard = new CovenantPage(representativeService, covenantService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #region cb Product Type Selection Changed Region

        private void SelectProductType(object sender, SelectionChangedEventArgs e)
        {
            if (productTypeComo.SelectedItem is CategoryDto s)
            {
                observProductsListFiltered.Clear();

                IReadOnlyList<ProductDto> products = _productService.GetProductsByCategoryId(s.Id);
                foreach (var product in products)
                {
                    observProductsLisLim.Add(product);
                    observProductsListFiltered.Add(product);
                }

                //var filtered = observProductsLisLim.Where(p => p.CategoryId == s.Id).ToList();
                //foreach (var product in filtered)
                //{
                //    observProductsListFiltered.Add(product);
                //}

                ProductCombo.ItemsSource = observProductsListFiltered;
                ProductCombo.SelectedIndex = 0;

            }

        }

        #endregion

        private void AddConvnenantItem(object sender, RoutedEventArgs e)
        {
            int productId = 0;
            if (!int.TryParse(Quantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("من فضلك ادخل كمية صحيحة");
                return;
            }

            if (ProductCombo.SelectedItem is ProductDto productDto)
            {
                productId = productDto.Id;
                var existingItem = CovenantItemsGridList
                       .FirstOrDefault(x => x.ProductId == productDto.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity += qty;
                }
                else
                {
                    CovenantItemsGridList.Add(new CovenantItemsDto
                    {
                        ProductId = productDto.Id,
                        ProductName = productDto.ProductName,
                        Quantity = qty,
                        CategoryId = productDto.CategoryId
                    });
                }

                //CovenantItemsGridList.Add(new CovenantItemsDto()
                //{
                //    Quantity = qty,
                //    ProductName = productDto.ProductName ?? "",
                //    CategoryId = productDto.CategoryId,
                //    ProductId = productId
                //});
            }

        }

        private async void AddConvnenant(object sender, RoutedEventArgs e)
        {
            if (!CovenantItemsGridList.Any())
            {
                MessageBox.Show("لا يوجد عناصر مضافة جديده");
                return;
            }

            AddCovenantItemsDto covenantItemsDto = new AddCovenantItemsDto()
            {
                CovenantId = _covenantId,
                CovenantItems = CovenantItemsGridList
            };
            var s = await _covenantService.addCovenantItems(covenantItemsDto);
            observProductsLisLim.Clear();
            observProductsListFiltered.Clear();


        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (CovenantItemsDataGrid.SelectedItem is CovenantItemsDto item)
            {
                CovenantItemsGridList.Remove(item);
            }
        }
    }
}
