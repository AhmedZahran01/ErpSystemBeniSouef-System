using ErpSystemBeniSouef.Core;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ErpSystemBeniSouef.Views.Pages.Regions
{
    /// <summary>
    /// Interaction logic for MainRegionPage.xaml
    /// </summary>
    public partial class MainRegionPage : Page
    {
        ObservableCollection<MainArea> observalMainRegions = new();
        List<MainArea> mainRegions = new List<MainArea>();
        private readonly IUnitOfWork _unitOfWork;

        public MainRegionPage(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            SeedDefaultRegions();
            dgMainRegions.ItemsSource = observalMainRegions;
        }

        private void SeedDefaultRegions()
        {
            mainRegions = _unitOfWork.Repository<MainArea>().GetAll();
            foreach (var item in mainRegions)
            {
                observalMainRegions.Add(item);
            }
        }
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegionName.Text) ||
               !int.TryParse(txtRegionStartNumber.Text, out int startNumber))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            MainArea newMainArea = new MainArea()
            {
                Name = txtRegionName.Text.Trim(),
                StartNumbering = startNumber
            };
            int addValue = 0;
            _unitOfWork.Repository<MainArea>().Add(newMainArea);
            addValue = await _unitOfWork.CompleteAsync();

            if (addValue == 1)
            {
                MessageBox.Show("  تم اضافه المنطقه الاساسية ");
                txtRegionName.Clear();
                txtRegionStartNumber.Clear();

                mainRegions = _unitOfWork.Repository<MainArea>().GetAll();

                //observalMainRegions.Add(item);
                observalMainRegions.Add(newMainArea);

            }
            else
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgMainRegions.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر صف قبل الحذف");
                return;
            }

            var selectedItems = dgMainRegions.SelectedItems.Cast<MainArea>().ToList();

            foreach (var item in selectedItems)
            {
                item.IsDeleted = true;
                _unitOfWork.Repository<MainArea>().Update(item);
                observalMainRegions.Remove(item);
            }

            int deleteMainArea = _unitOfWork.Complete();
            MessageBox.Show("تم حذف المنطقه الاساسية");

        }
        //private void BtnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    var selectedItems = dgMainRegions.SelectedItems.Cast<MainArea>().ToList();
        //    int countOfSelectedItems = selectedItems.Count();

        //    if (dgMainRegions.SelectedItem is not MainArea selected || selectedItems.Count() < 1)
        //    {
        //        MessageBox.Show("من فضلك اختر صف قبل الحذف");
        //        return;
        //    }
        //    else if (countOfSelectedItems > 1)
        //    {
        //        foreach (var item in selectedItems)
        //        {
        //            item.IsDeleted = true;
        //            _mainAreaRepo.Repository<MainArea>().Update(item);
        //            int deleteMainAreas = _mainAreaRepo.Complete();
        //            MessageBox.Show(" تم حذف المنطقه الاساسية ");
        //            observalMainRegions.Remove(selected);

        //        }
        //    }
        //    else
        //    {

        //        selected.IsDeleted = true;
        //        _mainAreaRepo.Repository<MainArea>().Update(selected);
        //        int deleteMainArea = _mainAreaRepo.Complete();
        //        MessageBox.Show(" تم حذف المنطقه الاساسية ");
        //        observalMainRegions.Remove(selected);
        //    }

        //}

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.RegionsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text.ToLower();

            // فلترة النتائج
            var filtered = mainRegions
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();


            // تحديث الـ DataGrid
            observalMainRegions.Clear();
            foreach (var item in filtered)
            {
                observalMainRegions.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.Name).Distinct();
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

        private void SuggestionsItemsListBoxForText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuggestionsItemsListBox.SelectedItem != null)
            {
                string fullname = (string)SuggestionsItemsListBox.SelectedItem;
                SearchByItemTextBox.Text = fullname;
                SuggestionsPopup.IsOpen = false;

                // فلترة DataGrid بالاختيار
                var filtered = mainRegions
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalMainRegions.Clear();
                foreach (var item in filtered)
                {
                    observalMainRegions.Add(item);
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // فضي التكست بوكس
            SearchByItemTextBox.Clear();

            // اقفل الـ Popup بتاع الاقتراحات
            SuggestionsPopup.IsOpen = false;

            // رجّع كل البيانات للـ DataGrid
            observalMainRegions.Clear();
            foreach (var item in mainRegions)
            {
                observalMainRegions.Add(item);
            }
        }



    }
}
