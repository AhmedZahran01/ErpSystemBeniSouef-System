using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.EntityFrameworkCore;
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

namespace ErpSystemBeniSouef.Views.Pages.Regions
{
    /// <summary>
    /// Interaction logic for SubRegionPage.xaml
    /// </summary>
    public partial class SubRegionPage : Page
    {
        ObservableCollection<SubArea> observalSubRegion = new ObservableCollection<SubArea>();
        ObservableCollection<MainArea> observalMainRegions = new ObservableCollection<MainArea>();
        List<SubArea> subAreas = new List<SubArea>();
        List<MainArea> mainAreas = new List<MainArea>();
        private readonly IUnitOfWork _unitOfWork;

          
        public SubRegionPage(IUnitOfWork unitOfWork)
        {
            InitializeComponent(); 
            _unitOfWork = unitOfWork;
            SeedSubRegionsAndMainAreas();
            cb_MainRegionName.ItemsSource = mainAreas;
            cb_MainRegionName.SelectedIndex = 0; // اختيار أول عنصر
            dgSubRegions.ItemsSource = observalSubRegion;
        }

        private async Task SeedSubRegionsAndMainAreas()
        {
            mainAreas = _unitOfWork.Repository<MainArea>().GetAll();
            //SubArea d = await _unitOfWork.Repository<SubArea>().GetByIdAsync(2);
            subAreas = _unitOfWork.Repository<SubArea>().GetAll( s => s.mainRegions);
            foreach (var subArea in subAreas)
            {
                observalSubRegion.Add(subArea);
            }


        }


        //private List<SubArea> SeedDefaultRegions()
        //{
        //    //List<MainRegions> defaults = dbContext.MainRegions.ToList();
        //    //foreach (var region in defaults)
        //    //{
        //    //    observalMainRegions.Add(region);
        //    //}
        //    //foreach (var item in observalMainRegions)
        //    //{
        //    //    mainRegionNameslist.Add(item.MainRegionName);
        //    //}


        //}

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //var regionsPage = new ErpSystemBeniSouef.Views.Pages.Products.RegionsPage();
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);
            var regionsPage = new Products.RegionsPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(regionsPage);

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {  
            if (dgSubRegions.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر صف قبل الحذف");
                return;
            }

            var selectedItems = dgSubRegions.SelectedItems.Cast<SubArea>().ToList();

            foreach (var item in selectedItems)
            {
                item.IsDeleted = true;
                 _unitOfWork.Repository<SubArea>().Update(item);
                observalSubRegion.Remove(item);
            }

            int deleteMainArea = _unitOfWork.Complete();
            MessageBox.Show("تم حذف المنطقه الاساسية");

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string mainRegionName = cb_MainRegionName.Text;
            string subRegionName = txtSbuRegionStartNumber.Text;

            int idOfSelectedMainArea = _unitOfWork.Repository<MainArea>().GetBy(n => n.Name == mainRegionName).Select(m => m.Id).FirstOrDefault();
            if (string.IsNullOrEmpty(mainRegionName) ||
                 string.IsNullOrEmpty(subRegionName)) 
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }
            SubArea newsubArea = new SubArea()
            {
                Name = txtSbuRegionStartNumber.Text.Trim(),
                MainAreaId = idOfSelectedMainArea
            };
            int addValue = 0;
            _unitOfWork.Repository<SubArea>().Add(newsubArea);
            addValue =  _unitOfWork.Complete();

            if (addValue == 1)
            {
                MessageBox.Show("  تم اضافه المنطقه الاساسية ");
                txtSbuRegionStartNumber.Clear(); 

                subAreas = _unitOfWork.Repository<SubArea>().GetAll(m => m.mainRegions);
                observalSubRegion.Add(newsubArea);

            }
            else
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
            } 
        }

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text.ToLower();

            // فلترة النتائج
            var filtered = subAreas
                .Where(i => i.Name != null && i.Name.ToLower().Contains(query))
                .ToList();


            // تحديث الـ DataGrid
            observalSubRegion.Clear();
            foreach (var item in filtered)
            {
                observalSubRegion.Add(item);
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
                var filtered = subAreas
                    .Where(i => i.Name == fullname)
                    .ToList();

                observalSubRegion.Clear();
                foreach (var item in filtered)
                {
                    observalSubRegion.Add(item);
                }
            }
        }



    }
}
