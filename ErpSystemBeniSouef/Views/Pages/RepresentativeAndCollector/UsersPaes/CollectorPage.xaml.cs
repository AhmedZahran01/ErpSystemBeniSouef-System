using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.Collector;
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

namespace ErpSystemBeniSouef.Views.Pages.RepresentativeAndCollector.UsersPaes
{
    public partial class CollectorPage : Page
    {
        #region Global Variables  Region

        int compId = (int?)App.Current.Properties["CompanyId"] ?? 1;

        private readonly ICollectorService _collector;
        ObservableCollection<CollectorDto> observalCollectorDto = new ObservableCollection<CollectorDto>();
        ObservableCollection<CollectorDto> observalCollectorFilter = new ObservableCollection<CollectorDto>();
        IReadOnlyList<CollectorDto> collectorDto = new List<CollectorDto>();

        #endregion

        #region Constractor Region

        public CollectorPage(ICollectorService collector)
        {
            InitializeComponent();
            _collector = collector;
            Loaded += async (s, e) =>
            { await LoadCollector(); };
            dgCollectors.ItemsSource = observalCollectorDto;
        }

        #endregion

        #region LoadCollector
        private async Task LoadCollector()
        {
            try
            {
                collectorDto = await _collector.GetAllAsync();

                observalCollectorDto.Clear();

                if (collectorDto != null && collectorDto.Any())
                {
                    foreach (var item in collectorDto)
                    {
                        observalCollectorDto.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حصل خطأ أثناء تحميل المحصلين: {ex.Message}");
            }
        }

        #endregion

        #region Add Btn Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string inputCollectorName = txtCollectorName.Text;

            if (string.IsNullOrWhiteSpace(inputCollectorName))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة في الحقل");
                return;
            }

            CreateCollectorDto newCollector = new CreateCollectorDto()
            {
                Name = txtCollectorName.Text.Trim(),

            };
            CollectorDto createdCollector = _collector.Create(newCollector);

            if (createdCollector != null)
            {
                MessageBox.Show(" تم إضافة المحصل بنجاح");

                observalCollectorDto.Add(createdCollector);
                dgCollectors.ItemsSource = observalCollectorDto;
            }
            else
            {
                MessageBox.Show(" حصل خطأ أثناء الإضافة");
            }


        }

        #endregion

        #region  Delete Btn Region

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {


            if (dgCollectors.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }

            List<CollectorDto> selectedItemsDto = dgCollectors.SelectedItems.Cast<CollectorDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = _collector.SoftDelete(item.Id);
                if (success)
                {
                    observalCollectorDto.Remove(item);
                    dgCollectors.ItemsSource = observalCollectorDto;
                    deletedCount++;

                }
            }

            if (deletedCount > 0)
            {
                string ValueOfString = "محصل";
                if (deletedCount > 1)
                    ValueOfString = "المحصلين";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} أساسية");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي محصل أساسية بسبب خطأ ما");
            }
        }

        #endregion

        #region My Supplir Search 

        private void SearchByItemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observalCollectorDto
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observalCollectorFilter.Clear();
            foreach (var item in filtered)
            {
                observalCollectorFilter.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
                dgCollectors.ItemsSource = filtered;
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
                var filtered = observalCollectorDto
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalCollectorFilter.Clear();
                foreach (var item in filtered)
                {
                    observalCollectorFilter.Add(item);
                }
            }
        }
        #endregion

        #region dgMainRegions_SelectionChanged Region

        private void dgCollectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCollectors.SelectedItem is CollectorDto selected)
            {
                txtCollectorName.Text = selected.Name;
                editBtn.Visibility = Visibility.Visible;
            }
        }


        #endregion

        #region My updateSupplir

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCollectors.SelectedItem is not CollectorDto selected)
            {
                MessageBox.Show("من فضلك اختر محصل  للتعديل");
                return;
            }

            string newName = txtCollectorName.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("من فضلك أدخل اسم المحصل");
                return;
            }

            if (observalCollectorDto.Any(m => m.Name == newName && m.Id != selected.Id))
            {
                MessageBox.Show("الاسم مستخدم بالفعل");
                return;
            }

            var updateDto = new UpdateCollectorDto
            {
                Id = selected.Id,
                Name = newName
            };

            bool success = _collector.Update(updateDto);

            if (success)
            {
                selected.Name = newName; // عدل نفس العنصر
                dgCollectors.Items.Refresh();
                MessageBox.Show("تم تعديل المحصل  بنجاح");
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }

        #endregion

        #region  Back Btn Click Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new RepresentativeAndCollector.MainRepresentativeAndCollectorPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion





    }
}
