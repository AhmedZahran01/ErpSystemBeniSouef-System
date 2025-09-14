
#region Using Region

using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Infrastructer;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
#endregion

namespace ErpSystemBeniSouef.Views.Pages.Regions
{
    public partial class MainRegionPage : Page
    {
        #region Global Variables  Region

        private readonly IMainAreaService _mainAreaService;
        private readonly IMapper _mapper;
        ObservableCollection<MainAreaDto> observalMainRegionsDto = new ObservableCollection<MainAreaDto>();
        List<MainArea> mainRegions = new List<MainArea>();

        #endregion

        #region Constractor Region

        public MainRegionPage(IMainAreaService mainAreaService, IMapper mapper)
        {
            InitializeComponent();
            _mainAreaService = mainAreaService;
            _mapper = mapper;
            SeedDefaultRegions();
            dgMainRegions.ItemsSource = observalMainRegionsDto;
        }

        #endregion

        #region Retrieve Default Regions to Grid Region

        private void SeedDefaultRegions()
        {
            try
            {
                mainRegions = _mainAreaService.GetAll().ToList();
            }
            catch
            { 
            }
            // AutoMapper يحولهم لـ DTOs
            var mapped = _mapper.Map<List<MainAreaDto>>(mainRegions);

            // امسح القديم واضيف الجديد بدون إنشاء Object جديد
            observalMainRegionsDto.Clear();
            foreach (var item in mapped)
            {
                observalMainRegionsDto.Add(item);
            }
              

        }


        #region Comment SeedDefaultRegions Region

        //private void SeedDefaultRegions()
        //{ 
        //    mainRegions = _mainAreaService.GetAll().ToList();
        //    foreach (var item in mainRegions) 
        //    {
        //        MainAreaDto addToObservablCollection = new MainAreaDto()
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            StartNumbering = item.StartNumbering,
        //            IsDeleted = item.IsDeleted,

        //        };
        //        observalMainRegionsDto.Add(addToObservablCollection);
        //    }
        //}

        //private void SeedDefaultRegions()
        //{
        //    // استرجع الـ MainAreas من السيرفس
        //    var mainRegions = _mainAreaService.GetAll();

        //    // اعمل Map للـ ObservableCollection مباشرة
        //    observalMainRegionsDto = new ObservableCollection<MainAreaDto>(
        //        mainRegions.Select(item => new MainAreaDto
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            StartNumbering = item.StartNumbering,
        //            IsDeleted = item.IsDeleted
        //        })
        //    );

        //    // اربط الـ DataGrid بالـ ObservableCollection
        //    dgMainRegions.ItemsSource = observalMainRegionsDto;
        //} 
        #endregion

        #endregion

        #region Add Btn Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegionName.Text) ||
               !int.TryParse(txtRegionStartNumber.Text, out int startNumber))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            CreateMainAreaDto newMainArea = new CreateMainAreaDto()
            {
                Name = txtRegionName.Text.Trim(),
                StartNumbering = startNumber
            };
            int addValue = 0;
            addValue = _mainAreaService.Create(newMainArea);

            if (addValue == 1)
            {
                MessageBox.Show("  تم اضافه المنطقه الاساسية ");
                txtRegionName.Clear();
                txtRegionStartNumber.Clear();

                MainArea lastMainArea = _mainAreaService.GetAll().LastOrDefault();

                MainAreaDto newMainAreaForObserv = new MainAreaDto()
                {
                    Id = lastMainArea.Id,
                    Name = lastMainArea.Name,
                    StartNumbering = lastMainArea.StartNumbering,
                    IsDeleted = lastMainArea.IsDeleted,

                };
                observalMainRegionsDto.Add(newMainAreaForObserv); 
            }
            else
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            }

        }

        #endregion

        #region  Delete Btn Region

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool checkSoftDelete = false;

            if (dgMainRegions.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }

            List<MainAreaDto> selectedItemsDto = dgMainRegions.SelectedItems.Cast<MainAreaDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = _mainAreaService.SoftDelete(item.Id);
                if (success)
                {
                    observalMainRegionsDto.Remove(item);
                    deletedCount++;
                }
            }

            if (deletedCount > 0)
            {
                string ValueOfString = "منطقة";
                if (deletedCount > 1)
                    ValueOfString = "مناطق";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} أساسية");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي منطقة أساسية بسبب خطأ ما");
            }
        }

        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.RegionsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion

        #region Search By Item Name Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text.ToLower();

            // فلترة النتائج
            var filtered = observalMainRegionsDto
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observalMainRegionsDto.Clear();
            foreach (var item in filtered)
            {
                observalMainRegionsDto.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
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
                var filtered = observalMainRegionsDto
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalMainRegionsDto.Clear();
                foreach (var item in filtered)
                {
                    observalMainRegionsDto.Add(item);
                }
            }
        }
        #endregion

        #region Btn Reset Region

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // فضي التكست بوكس
            SearchByItemTextBox.Clear();

            // اقفل الـ Popup بتاع الاقتراحات
            SuggestionsPopup.IsOpen = false;

            // رجّع كل البيانات للـ DataGrid
            observalMainRegionsDto.Clear();
            foreach (var item in mainRegions)
            {
                var mapped = _mapper.Map<MainAreaDto>(item);
                observalMainRegionsDto.Add(mapped);
            }
        }

        #endregion


    }
}
