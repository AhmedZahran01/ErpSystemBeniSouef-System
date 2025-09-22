using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.StorekeeperResponseDto;
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
    public partial class StorekeepersPage : Page
    {

        #region Global Variables  Region

        private readonly IStoreKeeperService _storeKeeper;
        private readonly IMapper _mapper;
        ObservableCollection<StorekeeperResponseDto> observalstoreKeeperDto = new ObservableCollection<StorekeeperResponseDto>();
        ObservableCollection<StorekeeperResponseDto> observalstoreKeeperforFilter = new ObservableCollection<StorekeeperResponseDto>();
        IReadOnlyList<StorekeeperResponseDto> storeKeeperDto = new List<StorekeeperResponseDto>();

        #endregion

        #region Constractor Region

        public StorekeepersPage(IStoreKeeperService storeKeeper, IMapper mapper)
        {
            InitializeComponent();
            _storeKeeper = storeKeeper;
            _mapper = mapper;

            Loaded += async (s, e) =>
            {
                await LoadStoreKeeper();
                dgStorekeepers.ItemsSource = observalstoreKeeperDto;
            };
        }
        #endregion

        #region Retrieve Default Regions to Grid Region

        private async Task LoadStoreKeeper()
        {
            try
            {
                storeKeeperDto = await _storeKeeper.GetAllAsync();

                observalstoreKeeperDto.Clear();

                if (storeKeeperDto != null && storeKeeperDto.Any())
                {
                    foreach (var item in storeKeeperDto)
                    {
                        observalstoreKeeperDto.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حصل خطأ أثناء تحميل المديرين: {ex.Message}");
            }
        }

        #endregion

        #region Add Btn Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string inputStoreKeeperName = txtStoreKeeperName.Text;
            string inputStoreKeeperUserName = txtStoreKeeperUserName.Text;
            string inputRepresentativePassword = txtPassword.Text;


            if (string.IsNullOrWhiteSpace(inputStoreKeeperName) ||
                 string.IsNullOrWhiteSpace(inputStoreKeeperUserName))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة في كل الحقول");
                return;
            }

            StorekeeperResponseDto CheckStoreKeeperDtoFounded = observalstoreKeeperDto.Where(n => n.Name == inputStoreKeeperName).FirstOrDefault();
            StorekeeperResponseDto CheckStoreKeeperDtotNoFounded = observalstoreKeeperDto.Where(n => n.Username == inputStoreKeeperUserName).FirstOrDefault();
            if (CheckStoreKeeperDtoFounded is not null || CheckStoreKeeperDtotNoFounded is not null)
            {
                if (CheckStoreKeeperDtoFounded is not null)
                    MessageBox.Show(" اسم امين المخازن مستخدم من قيل ");
                if (CheckStoreKeeperDtotNoFounded is not null)
                    MessageBox.Show(" الاسم المستخدم مستخدم من قيل ");
                return;
            }
            CreateStorekeeperDto newStorekeeper = new CreateStorekeeperDto()
            {
                Name = txtStoreKeeperName.Text.Trim(),
                Username = txtStoreKeeperUserName.Text.Trim(),
                Password = txtPassword.Text

            };

            StorekeeperResponseDto createdStorekeeper = _storeKeeper.Create(newStorekeeper);

            if (createdStorekeeper != null)
            {
                MessageBox.Show(" تم إضافة المورد بنجاح");

                observalstoreKeeperDto.Add(createdStorekeeper);
                observalstoreKeeperforFilter.Add(createdStorekeeper);
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


            if (dgStorekeepers.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }

            List<StorekeeperResponseDto> selectedItemsDto = dgStorekeepers.SelectedItems.Cast<StorekeeperResponseDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = _storeKeeper.SoftDelete(item.Id);
                if (success)
                {
                    observalstoreKeeperDto.Remove(item);
                    deletedCount++;
                }
            }

            if (deletedCount > 0)
            {
                string ValueOfString = " مدير";
                if (deletedCount > 1)
                    ValueOfString = " من المديرين";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي مدير بسبب خطأ ما");
            }
        }

        #endregion

        #region My Supplir Search 

        private void SearchByItemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observalstoreKeeperDto
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observalstoreKeeperforFilter.Clear();
            foreach (var item in filtered)
            {
                observalstoreKeeperforFilter.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
                dgStorekeepers.ItemsSource = filtered;
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
                var filtered = observalstoreKeeperDto
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalstoreKeeperforFilter.Clear();
                foreach (var item in filtered)
                {
                    observalstoreKeeperforFilter.Add(item);
                }
            }
        }
        #endregion
         
        #region dgMainRegions_SelectionChanged Region

        private void dgCollectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStorekeepers.SelectedItem is StorekeeperResponseDto selected)
            {
                txtStoreKeeperName.Text = selected.Name;
                txtStoreKeeperUserName.Text = selected.Username.ToString();
                txtPassword.Text = selected.Password;
                editBtn.Visibility = Visibility.Visible;
            }
        }


        #endregion

        #region My updateSupplir

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgStorekeepers.SelectedItem is not StorekeeperResponseDto selected)
            {
                MessageBox.Show("من فضلك اختر مورد للتعديل");
                return;
            }

            string newName = txtStoreKeeperName.Text.Trim();
            string newUsername = txtStoreKeeperUserName.Text.Trim();
            string newPassword = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("من فضلك أدخل اسم امين المخزن");
                return;
            }
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("من فضلك أدخل اسم المستخدم");
                return;
            }
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("من فضلك أدخل اسم الرقم السرى");
                return;
            }

            if (observalstoreKeeperDto.Any(m => m.Name ==
            newName && m.Id != selected.Id && m.Username == newUsername))
            {
                MessageBox.Show("الاسم مستخدم بالفعل");
                return;
            }

            var updateDto = new UpdateStorekeeperDto
            {
                Id = selected.Id,
                Name = newName,
                Username = newUsername,
                Password = newPassword

            };

            bool success = _storeKeeper.Update(updateDto);

            if (success)
            {
                selected.Name = newName;
                selected.Username = newUsername;
                selected.Password = newPassword;

                dgStorekeepers.Items.Refresh();
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
            var Dashboard = new MainRepresentativeAndCollectorPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }


        #endregion

    }
}
