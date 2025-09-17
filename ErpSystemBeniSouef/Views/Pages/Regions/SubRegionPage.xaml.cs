using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.EntityFrameworkCore;
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

namespace ErpSystemBeniSouef.Views.Pages.Regions
{
    /// <summary>
    /// Interaction logic for SubRegionPage.xaml
    /// </summary>
    public partial class SubRegionPage : Page
    {

        #region Global Variables  Region
        private readonly ISubAreaService _subAreaService;
        private readonly IMapper _mapper;
        private readonly IMainAreaService _mainAreaService;

        ObservableCollection<SubAreaDto> observalSubRegion = new ObservableCollection<SubAreaDto>();
        ObservableCollection<SubAreaDto> observalSubRegionFilter = new ObservableCollection<SubAreaDto>();

        IReadOnlyList<SubAreaDto> SubAreaList = new List<SubAreaDto>();
        IReadOnlyList<MainAreaDto> mainAreaDtos = new List<MainAreaDto>();
        #endregion

        #region Constractor Region

        public SubRegionPage(ISubAreaService subAreaService, IMapper mapper, IMainAreaService mainAreaService)
        {
            InitializeComponent(); _subAreaService = subAreaService; _mapper = mapper;
            _mainAreaService = mainAreaService;
            Loaded += async (s, e) =>
            {
                await LoadSubAreas();
                cb_MainRegionName.ItemsSource = mainAreaDtos;
                cb_MainRegionName.SelectedIndex = 0; // اختيار أول عنصر
                dgSubRegions.ItemsSource = observalSubRegion;
            };
        }


        #endregion

        #region Load SubAreas Region

        private async Task LoadSubAreas()
        {
            mainAreaDtos = await _mainAreaService.GetAllAsync();
            SubAreaList = await _subAreaService.GetAllAsync();
            foreach (var subArea in SubAreaList)
            {
                observalSubRegion.Add(subArea);
            }
        }

        #endregion
         
        #region Add Button Region
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string mainRegionName = cb_MainRegionName.Text;
            string subRegionName = txtSbuRegionStartNumber.Text;

            if (string.IsNullOrEmpty(mainRegionName) ||
                 string.IsNullOrEmpty(subRegionName))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            MainAreaDto selectedMainArea = (MainAreaDto)cb_MainRegionName.SelectedItem;

            SubAreaDto FoundNameSubArea = observalSubRegion.Where(s => s.Name == subRegionName && s.mainRegions.Id == selectedMainArea.Id).FirstOrDefault();
            if(FoundNameSubArea is not null)
            {
                MessageBox.Show(" اسم المنطقه الفرعيه مستخدم من قبل ");
                return;
            }
            if (selectedMainArea == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            CreateSubAreaDto createSubAreaDto = new CreateSubAreaDto()
            {
                Name = txtSbuRegionStartNumber.Text.Trim(),
                MainAreaId = selectedMainArea.Id,
            }; 
            bool SubAreaDtoRespons = _subAreaService.Create(createSubAreaDto);
            if (!SubAreaDtoRespons)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            MessageBox.Show("تم إضافة المنطقه بنجاح");
            txtSbuRegionStartNumber.Clear();

            var AddsubAreaDto = _mapper.Map<SubAreaDto>(createSubAreaDto);

            AddsubAreaDto.mainRegions = selectedMainArea;

            int AddedId = observalSubRegion.Max(s =>s.Id);
            AddsubAreaDto.Id = AddedId + 1;
            observalSubRegion.Add(AddsubAreaDto);
             
        }
        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //var regionsPage = new ErpSystemBeniSouef.Views.Pages.Products.RegionsPage();
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);
            var regionsPage = new Products.RegionsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //    if (dgSubRegions.SelectedItems.Count == 0)
            //    {
            //        MessageBox.Show("من فضلك اختر صف قبل الحذف");
            //        return;
            //    }

            //    var selectedItems = dgSubRegions.SelectedItems.Cast<SubArea>().ToList();

            //    foreach (var item in selectedItems)
            //    {
            //        item.IsDeleted = true;
            //         _unitOfWork.Repository<SubArea>().Update(item);
            //        observalSubRegion.Remove(item);
            //    }

            //    int deleteMainArea = _unitOfWork.Complete();
            //    MessageBox.Show("تم حذف المنطقه الاساسية");

        }
         
        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var query = SearchByItemTextBox.Text.ToLower();

            //// فلترة النتائج
            //var filtered = subAreas
            //    .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
            //    .ToList();


            //// تحديث الـ DataGrid
            //observalSubRegion.Clear();
            //foreach (var item in filtered)
            //{
            //    observalSubRegion.Add(item);
            //}

            //// تحديث الاقتراحات
            //var suggestions = filtered.Select(i => i.Name).Distinct();
            //if (suggestions.Any())
            //{
            //    SuggestionsItemsListBox.ItemsSource = suggestions;
            //    SuggestionsPopup.IsOpen = true;
            //}
            //else
            //{
            //    SuggestionsPopup.IsOpen = false;
            //} 
        }

        private void SuggestionsItemsListBoxForText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuggestionsItemsListBox.SelectedItem != null)
            {
                //string fullname = (string)SuggestionsItemsListBox.SelectedItem;
                //SearchByItemTextBox.Text = fullname;
                //SuggestionsPopup.IsOpen = false;

                //// فلترة DataGrid بالاختيار
                //var filtered = subAreas
                //    .Where(i => i.Name == fullname)
                //    .ToList();

                //observalSubRegion.Clear();
                //foreach (var item in filtered)
                //{
                //    observalSubRegion.Add(item);
                //}
            }
        }





    }
}
