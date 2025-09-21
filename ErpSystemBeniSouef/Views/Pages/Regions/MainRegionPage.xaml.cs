
#region Using Region

using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace ErpSystemBeniSouef.Views.Pages.Regions
{
    public partial class MainRegionPage : Page
    {
        #region Global Variables  Region

        private readonly IMainAreaService _mainAreaService;
        private readonly IMapper _mapper;
        ObservableCollection<MainAreaDto> observalMainRegionsDto = new ObservableCollection<MainAreaDto>();
        ObservableCollection<MainAreaDto> observalMainRegionsDtoforFilter = new ObservableCollection<MainAreaDto>();
        IReadOnlyList<MainAreaDto> mainRegionsDto = new List<MainAreaDto>();

        #endregion

        #region Constractor Region

        public MainRegionPage(IMainAreaService mainAreaService, IMapper mapper)
        {
            InitializeComponent();  _mainAreaService = mainAreaService; _mapper = mapper;

            Loaded += async (s, e) =>
            { await LoadMainRegions(); };
            dgMainRegions.ItemsSource = observalMainRegionsDto;
        }
        #endregion

        #region Retrieve Default Regions to Grid Region

        private async Task LoadMainRegions()
        {
            try { mainRegionsDto = await _mainAreaService.GetAllAsync(); }
            catch { MessageBox.Show("  برجاء المحاوله في وقت لاحق "); }
            // امسح القديم واضيف الجديد بدون إنشاء Object جديد
            observalMainRegionsDto.Clear();
            foreach (var item in mainRegionsDto)
            {
                observalMainRegionsDto.Add(item);
            }
        }

        #endregion

        #region Add Btn Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string inputRegionName = txtRegionName.Text;
            string inputStartNumber = txtRegionStartNumber.Text;
            int inputStartNumberI = 0;
            if (int.TryParse(txtRegionStartNumber.Text, out int inputStartNumberI2))
            {
                inputStartNumberI = inputStartNumberI2;
            }
            else
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }
            if (string.IsNullOrWhiteSpace(inputRegionName) ||
               !int.TryParse(inputStartNumber, out int startNumber))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة في كل الحقول");
                return;
            }

            MainAreaDto CheckMainAreaNameFounded = observalMainRegionsDto.Where(n => n.Name == inputRegionName).FirstOrDefault();
            MainAreaDto CheckMainAreaStartNoFounded = observalMainRegionsDto.Where(n => n.StartNumbering == inputStartNumberI).FirstOrDefault();
            if (CheckMainAreaNameFounded is not null || CheckMainAreaStartNoFounded is not null)
            {
                if (CheckMainAreaNameFounded is not null)
                    MessageBox.Show(" الاسم مستخدم من قيل ");
                if (CheckMainAreaStartNoFounded is not null)
                    MessageBox.Show(" الرقم مستخدم من قيل ");
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

                MainAreaDto lastMainArea = observalMainRegionsDto.Last();
                //MainArea lastMainArea = _mainAreaService.GetAll().LastOrDefault();
                if(lastMainArea is null)
                {
                    lastMainArea = new MainAreaDto() {Id= 0};
                }
                MainAreaDto newMainAreaForObserv = new MainAreaDto()
                {
                    Id = lastMainArea.Id + 1,
                    Name = newMainArea.Name,
                    StartNumbering = newMainArea.StartNumbering,
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
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observalMainRegionsDto
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observalMainRegionsDtoforFilter.Clear();
            foreach (var item in filtered)
            //foreach (var item in observalMainRegionsDtoforFilter)
            {
                observalMainRegionsDtoforFilter.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
                dgMainRegions.ItemsSource = filtered;
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

                observalMainRegionsDtoforFilter.Clear();
                foreach (var item in filtered)
                {
                    observalMainRegionsDtoforFilter.Add(item);
                }
            }
        }
        #endregion

        #region dgMainRegions_SelectionChanged Region

        private void dgMainRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMainRegions.SelectedItem is MainAreaDto selected)
            {
                txtRegionName.Text = selected.Name;
                txtRegionStartNumber.Text = selected.StartNumbering.ToString();
                editBtn.Visibility = Visibility.Visible;
            }
        }


        #endregion

        #region BtnEdit_Click Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgMainRegions.SelectedItem is not MainAreaDto selected)
            {
                MessageBox.Show("من فضلك اختر منطقة للتعديل");
                return;
            }

            string newName = txtRegionName.Text.Trim();
            if (!int.TryParse(txtRegionStartNumber.Text, out int newStartNumber))
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }

            // تحقق من التكرار
            if (observalMainRegionsDto.Any(m => m.Name == newName && m.Id != selected.Id))
            {
                MessageBox.Show("الاسم مستخدم بالفعل");
                return;
            }

            // DTO للتحديث
            var updateDto = new UpdateMainAreaDto
            {
                Id = selected.Id,
                Name = newName,
                StartNumbering = newStartNumber
            };

            bool success = _mainAreaService.Update(updateDto);

            if (success)
            {
                // عدل العنصر في ObservableCollection
                selected.Name = newName;
                selected.StartNumbering = newStartNumber;

                dgMainRegions.Items.Refresh(); // لتحديث الجدول
                MessageBox.Show("تم تعديل المنطقة بنجاح");

                var index = observalMainRegionsDto
               .Select((item, i) => new { item, i })
               .FirstOrDefault(x => x.item.Id == updateDto.Id)?.i;

                if (index != null)
                {
                    observalMainRegionsDto[index.Value] = new MainAreaDto
                    {
                        Id = updateDto.Id,
                        Name = updateDto.Name,
                        StartNumbering = updateDto.StartNumbering
                    };
                }
                //MainAreaDto newUpdateMainAreaForObserv = new MainAreaDto()
                //{
                //    Id = updateDto.Id ,
                //    Name = updateDto.Name,
                //    StartNumbering = updateDto.StartNumbering,
                //};
                //observalMainRegionsDto.Remove(newUpdateMainAreaForObserv);

            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }

        #endregion

        #region comment  MyRegion

        //#region Btn Reset Region

        //private void BtnReset_Click(object sender, RoutedEventArgs e)
        //{
        //    // فضي التكست بوكس
        //    SearchByItemTextBox.Clear();

        //    // اقفل الـ Popup بتاع الاقتراحات
        //    SuggestionsPopup.IsOpen = false;

        //    // رجّع كل البيانات للـ DataGrid
        //    observalMainRegionsDto.Clear();
        //    LoadMainRegions();
        //    foreach (var item in mainRegionsDto)
        //    {
        //        var mapped = _mapper.Map<MainAreaDto>(item);
        //        observalMainRegionsDto.Add(mapped);
        //    }
        //}

        //#endregion

        #endregion
    }
}
