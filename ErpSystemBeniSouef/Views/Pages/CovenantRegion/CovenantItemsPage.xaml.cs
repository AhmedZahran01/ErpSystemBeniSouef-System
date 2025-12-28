using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Covenant;
using ErpSystemBeniSouef.Core.DTOs.Covenant;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Service.RepresentativeService;
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

namespace ErpSystemBeniSouef.Views.Pages.CovenantRegion
{
    /// <summary>
    /// Interaction logic for CovenantItemsPage.xaml
    /// </summary>
    public partial class CovenantItemsPage : Page
    {
        private readonly IProductService _productService;
        private readonly ICovenantService _covenantService;
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsLisLim = new ObservableCollection<ProductDto>();
        public IReadOnlyList<CategoryDto> categoryDtos = new List<CategoryDto>();
        List<CovenantItemsDto> covenantItems = new List<CovenantItemsDto>();

        public CovenantItemsPage(IProductService productService, ICovenantService covenantService)
        {
            InitializeComponent();
            _productService = productService;
            _covenantService = covenantService;
            Loaded += async (s, e) =>
            {
                await loadData();
                productTypeComo.ItemsSource = categoryDtos;
                productTypeComo.SelectedIndex = 0;

                //ConvenantDataGrid.ItemsSource = returnCovenants;
                //RepresenComoBox.SelectedIndex = 0;

            };
        }

        public async Task loadData()
        {
            categoryDtos = await _productService.GetAllCategoriesAsync();

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

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
            int ProId = 0;
            if (ProductCombo.SelectedItem is ProductDto productDto)
            {
                ProId = productDto.Id;

                covenantItems.Add(new CovenantItemsDto()
                {
                    Amount = 2,
                    CategoryId = productDto.CategoryId,
                    ProductId = ProId
                });
            }

        }

        private void AddConvnenant(object sender, RoutedEventArgs e)
        {
            AddCovenantItemsDto covenantItemsDto = new AddCovenantItemsDto()
            {
                CovenantId = 2,
                CovenantItems = covenantItems
            };
            var s =  _covenantService.addCovenantItems(covenantItemsDto);


        }
    }
}
