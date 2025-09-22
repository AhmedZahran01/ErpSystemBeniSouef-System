using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ErpSystemBeniSouef.Views.Pages.Regions
{
    public partial class SubRegionPage : Page
    {
        #region Global Variables Region 
        
        private readonly ISubAreaService _subAreaService;
        private readonly IMapper _mapper;
        private readonly IMainAreaService _mainAreaService;

        // الأصل (ثابت)
        private List<SubAreaDto> allSubAreas = new();

        // دي اللي مربوطة بالـ DataGrid
        private ObservableCollection<SubAreaDto> observalSubRegionFilter = new();

        private IReadOnlyList<MainAreaDto> mainAreaDtos = new List<MainAreaDto>();
        #endregion

        #region Constructor Region

        public SubRegionPage(ISubAreaService subAreaService, IMapper mapper, IMainAreaService mainAreaService)
        {
            InitializeComponent();
            _subAreaService = subAreaService;
            _mapper = mapper;
            _mainAreaService = mainAreaService;

            Loaded += async (s, e) =>
            {
                await LoadSubAreas();
                cb_MainRegionName.ItemsSource = mainAreaDtos;
                if (mainAreaDtos.Any())
                    cb_MainRegionName.SelectedIndex = 0;

                dgSubRegions.ItemsSource = observalSubRegionFilter ;
            };
        }
        #endregion

        #region Load SubAreas Region
        private async Task LoadSubAreas()
        {
            mainAreaDtos = await _mainAreaService.GetAllAsync();

            allSubAreas = (await _subAreaService.GetAllAsync()).ToList();

            observalSubRegionFilter.Clear();
            foreach (var subArea in allSubAreas)
            {
                observalSubRegionFilter.Add(subArea);
            }
        }
        #endregion

        #region Add Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string mainRegionName = cb_MainRegionName.Text;
            string subRegionName = txtSbuRegionName.Text;

            if (string.IsNullOrEmpty(mainRegionName) || string.IsNullOrEmpty(subRegionName))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MainAreaDto selectedMainArea = (MainAreaDto)cb_MainRegionName.SelectedItem;

            // تأكد إن الاسم مش مكرر
            var found = allSubAreas
                .FirstOrDefault(s => s.Name == subRegionName && s.mainRegions.Id == selectedMainArea.Id);

            if (found is not null)
            {
                MessageBox.Show("  اسم المنطقه الفرعيه مستخدم من قبل مع نفس المنطقه الرْيسيه ");
                return;
            }

            var createSubAreaDto = new CreateSubAreaDto
            {
                Name = subRegionName.Trim(),
                MainAreaId = selectedMainArea.Id,
            };

            var created = _subAreaService.Create(createSubAreaDto);
            if (created is null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة المنطقه بنجاح");
            txtSbuRegionName.Clear();

            // ضيف للأصل والعرض
            allSubAreas.Add(created);
            observalSubRegionFilter.Add(created);
        }
        #endregion

        #region Delete Region

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubRegions.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }

            var selectedItems = dgSubRegions.SelectedItems.Cast<SubAreaDto>().ToList();
            int deletedCount = 0;

            foreach (var item in selectedItems)
            {
                bool success = _subAreaService.SoftDelete(item.Id);
                if (success)
                {
                    allSubAreas.Remove(item);
                    observalSubRegionFilter.Remove(item);
                    deletedCount++;
                }
            }

            if (deletedCount > 0)
            {
                MessageBox.Show($"تم حذف {deletedCount} منطقة ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي منطقة فرعية بسبب خطأ ما");
            }
        }
        #endregion

        #region Search Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchByItemTextBox.Text?.ToLower() ?? "";

            var filtered = allSubAreas
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();

            observalSubRegionFilter.Clear();
            foreach (var item in filtered)
            {
                observalSubRegionFilter.Add(item);
            }

            var suggestions = filtered.Select(i => i.Name).Distinct();
            SuggestionsItemsListBox.ItemsSource = suggestions;
            SuggestionsPopup.IsOpen = suggestions.Any();
        }
        #endregion

        #region Suggestions Region

        private void SuggestionsItemsListBoxForText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuggestionsItemsListBox.SelectedItem is string fullname)
            {
                SearchByItemTextBox.Text = fullname;
                SuggestionsPopup.IsOpen = false;

                var filtered = allSubAreas
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalSubRegionFilter.Clear();
                foreach (var item in filtered)
                {
                    observalSubRegionFilter.Add(item);
                }
            }
        }
        #endregion

        #region Back Region 

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var regionsPage = new Products.RegionsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);
        }
        #endregion

        #region Selection Changed Region

        private void dgSubAreas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dgSubRegions.SelectedItem is SubAreaDto selected)
            {
                txtSbuRegionName.Text = selected.Name;
                cb_MainRegionName.SelectedValue = selected.mainRegions.Id;

                editBtn.Visibility = Visibility.Visible;
            }

        }

        #endregion

        #region Btn Edit Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubRegions.SelectedItem is not SubAreaDto selected)
            {
                MessageBox.Show("من فضلك اختر منطقة للتعديل");
                return;
            }

            string newSubAreaName = txtSbuRegionName.Text.Trim();
            if (string.IsNullOrEmpty(newSubAreaName))
            {
                MessageBox.Show("من فضلك أدخل قيم صحيحه ");
                return;
            }
            MainAreaDto MainComBox = ((MainAreaDto)cb_MainRegionName.SelectedItem);
            // تأكد إن الاسم مش مكرر
            var found = allSubAreas
                .FirstOrDefault(s => s.Name == newSubAreaName && s.mainRegions.Id == MainComBox.Id);

            if (found is not null)
            {
                MessageBox.Show("  اسم المنطقه الفرعيه مستخدم من قبل مع نفس المنطقه الرْيسيه ");
                return;
            }

            var updateDto = new UpdateSubAreaDto()
            {
                Id = selected.Id,
                Name = newSubAreaName,
                MainAreaId = ((MainAreaDto)cb_MainRegionName.SelectedItem).Id,
            };

            SubAreaDto subAreaDto = _subAreaService.Update(updateDto);
            MainAreaDto mainAreaDtod = mainAreaDtos.FirstOrDefault(m => m.Id == MainComBox.Id);
            if (subAreaDto is not null)
            {
                selected.Name = newSubAreaName;
                selected.mainRegions = mainAreaDtod;

                dgSubRegions.Items.Refresh(); // لتحديث الجدول
                MessageBox.Show("تم تعديل المنطقة بنجاح");
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }

        }

        #endregion

    }
}






//using AutoMapper;
//using ErpSystemBeniSouef.Core;
//using ErpSystemBeniSouef.Core.Contract;
//using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
//using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
//using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
//using ErpSystemBeniSouef.Core.Entities;
//using ErpSystemBeniSouef.Dtos.MainAreaDto;
//using ErpSystemBeniSouef.Service.ProductService;
//using ErpSystemBeniSouef.ViewModel;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace ErpSystemBeniSouef.Views.Pages.Regions
//{
//    /// <summary>
//    /// Interaction logic for SubRegionPage.xaml
//    /// </summary>
//    public partial class SubRegionPage : Page
//    {

//        #region Global Variables  Region
//        private readonly ISubAreaService _subAreaService;
//        private readonly IMapper _mapper;
//        private readonly IMainAreaService _mainAreaService;

//        ObservableCollection<SubAreaDto> observalSubRegionLim = new ObservableCollection<SubAreaDto>();
//        ObservableCollection<SubAreaDto> observalSubRegionFilter = new ObservableCollection<SubAreaDto>();

//        IReadOnlyList<SubAreaDto> SubAreaList = new List<SubAreaDto>();
//        IReadOnlyList<MainAreaDto> mainAreaDtos = new List<MainAreaDto>();
//        #endregion

//        #region Constractor Region

//        public SubRegionPage(ISubAreaService subAreaService, IMapper mapper, IMainAreaService mainAreaService)
//        {
//            InitializeComponent(); _subAreaService = subAreaService; _mapper = mapper;
//            _mainAreaService = mainAreaService;
//            Loaded += async (s, e) =>
//            {
//                await LoadSubAreas();
//                cb_MainRegionName.ItemsSource = mainAreaDtos;
//                if (mainAreaDtos.Any())
//                    cb_MainRegionName.SelectedIndex = 0;

//                dgSubRegions.ItemsSource = observalSubRegionLim;
//            };
//        }


//        #endregion

//        #region Load SubAreas Region

//        private async Task LoadSubAreas()
//        {
//            mainAreaDtos = await _mainAreaService.GetAllAsync();
//            SubAreaList = await _subAreaService.GetAllAsync();
//            foreach (var subArea in SubAreaList)
//            {
//                observalSubRegionLim.Add(subArea);
//            }
//            observalSubRegionFilter = observalSubRegionLim;
//        }

//        #endregion

//        #region Add Button Region
//        private void BtnAdd_Click(object sender, RoutedEventArgs e)
//        {
//            string mainRegionName = cb_MainRegionName.Text;
//            string subRegionName = txtSbuRegionName.Text;

//            if (string.IsNullOrEmpty(mainRegionName) ||
//                 string.IsNullOrEmpty(subRegionName))
//            {
//                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
//                return;
//            }
//            MainAreaDto selectedMainArea = (MainAreaDto)cb_MainRegionName.SelectedItem;

//            SubAreaDto FoundNameSubArea = observalSubRegionFilter.Where(s => s.Name == subRegionName && s.mainRegions.Id == selectedMainArea.Id).FirstOrDefault();
//            if (FoundNameSubArea is not null)
//            {
//                MessageBox.Show(" اسم المنطقه الفرعيه مستخدم من قبل ");
//                return;
//            }
//            if (selectedMainArea == null)
//            {
//                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
//                return;
//            }
//            CreateSubAreaDto createSubAreaDto = new CreateSubAreaDto()
//            {
//                Name = txtSbuRegionName.Text.Trim(),
//                MainAreaId = selectedMainArea.Id,
//            };
//            var CreatedSubAreaDto = _subAreaService.Create(createSubAreaDto);
//            if (CreatedSubAreaDto is null)
//            {
//                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
//                return;
//            }
//            MessageBox.Show("تم إضافة المنطقه بنجاح");
//            txtSbuRegionName.Clear();

//            //var AddsubAreaDto = _mapper.Map<SubAreaDto>(createSubAreaDto);

//            //AddsubAreaDto.mainRegions = selectedMainArea;

//            //int AddedId = observalSubRegionFilter.Max(s => s.Id);
//            //AddsubAreaDto.Id = AddedId + 1;
//            observalSubRegionFilter.Add(CreatedSubAreaDto);

//        }
//        #endregion

//        #region Delete Button Region

//        private void BtnDelete_Click(object sender, RoutedEventArgs e)
//        {
//            bool checkSoftDelete = false;
//            if (dgSubRegions.SelectedItems.Count == 0)
//            {
//                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
//                return;
//            }

//            List<SubAreaDto> selectedItemsDto = dgSubRegions.SelectedItems.Cast<SubAreaDto>().ToList();
//            int deletedCount = 0;
//            foreach (var item in selectedItemsDto)
//            {
//                bool success = _subAreaService.SoftDelete(item.Id);
//                if (success)
//                {
//                    observalSubRegionFilter.Remove(item);
//                    deletedCount++;
//                }
//            }
//            if (deletedCount > 0)
//            {
//                string ValueOfString = "منطقه ";
//                if (deletedCount > 1)
//                    ValueOfString = "منطقه ";
//                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
//            }
//            else
//            {
//                MessageBox.Show("لم يتم حذف أي منطقة أساسية بسبب خطأ ما");
//            }

//        }

//        #endregion


//        #region Search By Item FullName  Region

//        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
//        {
//            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

//            // فلترة النتائج
//            var filtered = observalSubRegionFilter
//                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
//                .ToList();

//            // تحديث الـ DataGrid
//            observalSubRegionFilter.Clear();
//            foreach (var item in filtered)
//            {
//                observalSubRegionFilter.Add(item);
//            }

//            // تحديث الاقتراحات
//            var suggestions = filtered.Select(i => i.Name).Distinct();
//            if (suggestions.Any())
//            {
//                SuggestionsItemsListBox.ItemsSource = suggestions;
//                SuggestionsPopup.IsOpen = true;
//            }
//            else
//            {
//                SuggestionsPopup.IsOpen = false;
//            }
//        }

//        #endregion

//        #region Suggestions Items List Box Region

//        private void SuggestionsItemsListBoxForText_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (SuggestionsItemsListBox.SelectedItem != null)
//            {
//                string fullname = (string)SuggestionsItemsListBox.SelectedItem;
//                SearchByItemTextBox.Text = fullname;
//                SuggestionsPopup.IsOpen = false;

//                // فلترة DataGrid بالاختيار
//                var filtered = observalSubRegionFilter
//                    .Where(i => i.Name == fullname)
//                    .ToList();

//                observalSubRegionFilter.Clear();
//                foreach (var item in filtered)
//                {
//                    observalSubRegionFilter.Add(item);
//                }
//            }
//        }

//        #endregion




//        private void BackBtn_Click(object sender, RoutedEventArgs e)
//        {
//            //var regionsPage = new ErpSystemBeniSouef.Views.Pages.Products.RegionsPage();
//            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);
//            var regionsPage = new Products.RegionsPage();
//            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);

//        }






//    }
//}
