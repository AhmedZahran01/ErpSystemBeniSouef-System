using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
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

namespace ErpSystemBeniSouef.Views.Pages.RepresentativeAndCollector.UsersPaes
{ 
    public partial class RepresentativePage : Page
    {

        #region Global Variables  Region

        private readonly IRepresentativeService _representative;
        private readonly IMapper _mapper;
        ObservableCollection<RepresentativeDto> observalRepresentativeDto = new ObservableCollection<RepresentativeDto>();
        ObservableCollection<RepresentativeDto> observalRepresentativeDtoforFilter = new ObservableCollection<RepresentativeDto>();
        IReadOnlyList<RepresentativeDto> representativeDto = new List<RepresentativeDto>();

        #endregion

        #region Constractor Region

        public RepresentativePage(IRepresentativeService representative, IMapper mapper)
        {
            InitializeComponent();
            _representative = representative;
            _mapper = mapper;

            Loaded += async (s, e) =>
            {
                await LoadRepresentative();
                dgRepresentatives.ItemsSource = observalRepresentativeDto;

            };
        }
        #endregion
         
        #region Retrieve Default Regions to Grid Region

        private async Task LoadRepresentative()
        {
            try
            {
                representativeDto = await _representative.GetAllAsync();

                observalRepresentativeDto.Clear();

                if (representativeDto != null && representativeDto.Any())
                {
                    foreach (var item in representativeDto)
                    {
                        observalRepresentativeDto.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حصل خطأ أثناء تحميل المناديب: {ex.Message}");
            }
        }

        #endregion

        #region Add Btn Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string inputRepresentativeName = txtRepresentativeName.Text;
            string inputRepresentativeNumber = txtRepresentativeNumber.Text;
            string inputRepresentativePassword = txtPassword.Text;

            int inputStartNumberI = 0;
            if (int.TryParse(txtRepresentativeNumber.Text, out int inputStartNumberI2))
            {
                inputStartNumberI = inputStartNumberI2;
            }
            else
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }
            if (string.IsNullOrWhiteSpace(inputRepresentativeName) ||
               !int.TryParse(inputRepresentativeNumber, out int startNumber))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة في كل الحقول");
                return;
            }

            RepresentativeDto CheckRepresentativeDtoFounded = observalRepresentativeDto.Where(n => n.Name == inputRepresentativeName).FirstOrDefault();
            RepresentativeDto CheckRepresentativeDtotNoFounded = observalRepresentativeDto.Where(n => n.UserNumber == inputStartNumberI).FirstOrDefault();
            if (CheckRepresentativeDtoFounded is not null || CheckRepresentativeDtotNoFounded is not null)
            {
                if (CheckRepresentativeDtoFounded is not null)
                    MessageBox.Show(" الاسم مستخدم من قيل ");
                if (CheckRepresentativeDtotNoFounded is not null)
                    MessageBox.Show(" الرقم مستخدم من قيل ");
                return;
            }
            CreateRepresentativeDto newrepresentative = new CreateRepresentativeDto()
            {
                Name = txtRepresentativeName.Text.Trim(),
                UserNumber = startNumber,
                Password = txtPassword.Text

            };

            RepresentativeDto createdSupplier = _representative.Create(newrepresentative);

            if (createdSupplier != null)
            {
                MessageBox.Show(" تم إضافة المندوب بنجاح"); 
                observalRepresentativeDto.Add(createdSupplier);  
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
            if (dgRepresentatives.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }

            List<RepresentativeDto> selectedItemsDto = dgRepresentatives.SelectedItems.Cast<RepresentativeDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = _representative.SoftDelete(item.Id);
                if (success)
                {
                    observalRepresentativeDto.Remove(item);
                    deletedCount++;
                }
            }

            if (deletedCount > 0)
            {
                string ValueOfString = "المندوب";
                if (deletedCount > 1)
                    ValueOfString = "المناديب";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي مندوب بسبب خطأ ما");
            }
        }

        #endregion

        #region My Supplir Search 

        private void SearchByItemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observalRepresentativeDto
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observalRepresentativeDtoforFilter.Clear();
            foreach (var item in filtered)
            {
                observalRepresentativeDtoforFilter.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
                dgRepresentatives.ItemsSource = filtered;
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
                var filtered = observalRepresentativeDto
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalRepresentativeDtoforFilter.Clear();
                foreach (var item in filtered)
                {
                    observalRepresentativeDtoforFilter.Add(item);
                }
            }
        }
        #endregion
         
        #region dgMainRegions_SelectionChanged Region

        private void dgCollectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRepresentatives.SelectedItem is RepresentativeDto selected)
            {
                txtRepresentativeName.Text = selected.Name;
                txtRepresentativeNumber.Text = selected.UserNumber.ToString();
                txtPassword.Text = selected.Password;
                editBtn.Visibility = Visibility.Visible;
            }
        }


        #endregion

        #region My updateSupplir

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgRepresentatives.SelectedItem is not RepresentativeDto selected)
            {
                MessageBox.Show("من فضلك اختر مورد للتعديل");
                return;
            }

            string newName = txtRepresentativeName.Text.Trim();
            string newpassword = txtPassword.Text.Trim();
            if (!int.TryParse(txtRepresentativeNumber.Text, out int newUsernumber))
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح");
                return;
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("من فضلك أدخل اسم المندوبين");
                return;
            }

            if (observalRepresentativeDto.Any(m => m.Name == newName && m.Id != selected.Id))
            {
                MessageBox.Show("الاسم مستخدم بالفعل");
                return;
            }

            var updateDto = new UpdateRepresentativeDto
            {
                Id = selected.Id,
                Name = newName,
                UserNumber = newUsernumber,
                Password = newpassword

            };

            bool success = _representative.Update(updateDto);

            if (success)
            {
                selected.Name = newName;
                selected.UserNumber = newUsernumber;
                selected.Password = newpassword;


                dgRepresentatives.Items.Refresh();
                MessageBox.Show("تم تعديل المندوب بنجاح");
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }

        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Views.Pages.RepresentativeAndCollector.MainRepresentativeAndCollectorPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion


    }
}
