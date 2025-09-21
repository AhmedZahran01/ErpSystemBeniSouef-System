using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
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

namespace ErpSystemBeniSouef.Views.Pages.RepresentativeAndCollector.UsersPaes
{
    /// <summary>
    /// Interaction logic for SuppliersPage.xaml
    /// </summary>
    public partial class SuppliersPage : Page
    {
        #region Global Variables  Region

        int compId = (int?)App.Current.Properties["CompanyId"]?? 0;

        ObservableCollection<SupplierDto> observalsuppliers = new();
        ObservableCollection<SupplierDto> observalsuppliersFilter = new();
        IReadOnlyList<SupplierDto> supplierDtos = new List<SupplierDto>();
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;


        #endregion

        #region Constractor Region
        public SuppliersPage(ISupplierService supplierService)
        {
            InitializeComponent(); _supplierService = supplierService;

            Loaded += async (s, e) =>
            {
                await LoadSuppliers();

                dgsuppliers.ItemsSource = observalsuppliers;
            };
        }

        #endregion

        #region Load Suppliers Region

        private async Task LoadSuppliers()
        {
            supplierDtos = await _supplierService.GetAllAsync();
            observalsuppliers.Clear();
            foreach (var item in supplierDtos)
            {
                observalsuppliers.Add(item);
            }

        }

        #endregion

        #region Add Btn Region

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string inputSupplierName = txtSupplierName.Text;
            if (string.IsNullOrWhiteSpace(inputSupplierName))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة ");
                return;
            }
            CreateSupplierDto newSupplirArea = new CreateSupplierDto()
            {
                Name = txtSupplierName.Text.Trim(),

            };
            SupplierDto createdSupplier = _supplierService.Create(newSupplirArea);

            if (createdSupplier != null)
            {
                MessageBox.Show(" تم إضافة المورد بنجاح");

                observalsuppliers.Add(createdSupplier);
                dgsuppliers.ItemsSource = observalsuppliers;

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
            if (dgsuppliers.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }

            List<SupplierDto> selectedItemsDto = dgsuppliers.SelectedItems.Cast<SupplierDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = _supplierService.SoftDelete(item.Id);
                if (success)
                {
                    observalsuppliers.Remove(item);
                    dgsuppliers.ItemsSource = observalsuppliers;
                    deletedCount++;
                }
            }

            if (deletedCount > 0)
            {
                string ValueOfString = "مورد";
                if (deletedCount > 1)
                    ValueOfString = "موردين";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي مورد بسبب خطأ ما");
            }
        }

        #endregion

        #region dgMainRegions_SelectionChanged Region

        private void dgSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgsuppliers.SelectedItem is SupplierDto selected)
            {
                txtSupplierName.Text = selected.Name; 
                editBtn.Visibility = Visibility.Visible;
            }
        }


        #endregion
          
        #region BtnEdit_Click Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgsuppliers.SelectedItem is not SupplierDto selected)
            {
                MessageBox.Show("من فضلك اختر مورد للتعديل");
                return;
            } 
            string newName = txtSupplierName.Text.Trim();
             
            // DTO للتحديث
            var updateDto = new UpdateSupplierDto
            { 
                Name = newName,  
                Id= selected.Id,
            };

            Supplier supplierValue = _supplierService.Update(selected.Id , newName);

            if (supplierValue is not null)
            {
                // عدل العنصر في ObservableCollection
                supplierValue.Name = newName; 

                dgsuppliers.Items.Refresh(); // لتحديث الجدول
                MessageBox.Show("تم تعديل المورد بنجاح");


                var index = observalsuppliers
               .Select((item, i) => new { item, i })
               .FirstOrDefault(x => x.item.Id == updateDto.Id)?.i;

                if (index != null)
                {
                    observalsuppliers[index.Value] = new SupplierDto
                    {
                        Id = updateDto.Id,
                        Name = updateDto.Name, 
                    };
                }

            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }

        #endregion
         

        #region Search By Item Name Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observalsuppliers
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observalsuppliersFilter.Clear();
            foreach (var item in filtered) 
            {
                observalsuppliersFilter.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name);
            if (suggestions.Any())
            {
                SuggestionsItemsListBox.ItemsSource = suggestions;
                dgsuppliers.ItemsSource = filtered;
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
                var filtered = observalsuppliers
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalsuppliersFilter.Clear();
                foreach (var item in filtered)
                {
                    observalsuppliersFilter.Add(item);
                }
            }
        }
        #endregion
         
        #region  Back Btn Click Region
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new MainRepresentativeAndCollectorPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        #endregion
    }
}
