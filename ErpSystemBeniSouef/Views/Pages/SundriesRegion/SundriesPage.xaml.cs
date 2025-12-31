using ErpSystemBeniSouef.Core.Contract.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
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

namespace ErpSystemBeniSouef.Views.Pages.SundriesRegion
{
    /// <summary>
    /// Interaction logic for SundriesPage.xaml
    /// </summary>

    public partial class SundriesPage : Page
    {
        #region Constractor Region

        private readonly IPettyCashService _pettyCashService;
        private ObservableCollection<ReturnPettyCashDto> PettyCashList;

        public SundriesPage(IPettyCashService pettyCashService)
        {
            InitializeComponent();
            _pettyCashService = pettyCashService;

            Loaded += async (s, e) =>
            {
                await LoadPettyCash();
                pettyCashGrid.ItemsSource = PettyCashList;
            };

        }

        #endregion

        #region Load Petty Cash to Grid Region

        private async Task LoadPettyCash()
        {
            var data = await _pettyCashService.GetAllPettyCash();
            PettyCashList = new ObservableCollection<ReturnPettyCashDto>(
                data.Select((item, index) => new ReturnPettyCashDto
                {
                    Id = item.Id,
                    Date = item.Date,
                    Reason = item.Reason,
                    Amount = item.Amount,
                    DipslayUIId = index + 1
                })
            );

        }

        #endregion

        #region Add Sundries Btn_Click Region

        private async void AddSundriesBtn_Click(object sender, RoutedEventArgs e)
        {
            string sundriesReason = SundriesReason.Text;
            string sundriesTotal = SundriesTotal.Text;

            DateTime? invoiceDate = SundriesDate.SelectedDate;
            if (invoiceDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }
            bool checkPase = decimal.TryParse(sundriesTotal, out decimal amount);
            if (!checkPase)
            {
                amount = 0;
            }
             
            AddPettyCashDto addPettyCashDto = new AddPettyCashDto
            {
                Amount = amount,
                Reason = sundriesReason,
                Date = invoiceDate.Value
            };
            if (!ValidateForm())
                return;

            AddButton.IsEnabled = false;
            try
            {
                bool added = await _pettyCashService.AddPettyCash(addPettyCashDto);
                if (added)
                {
                    int nextId = PettyCashList.Any() ? PettyCashList.Max(x => x.DipslayUIId) + 1 : 1;

                    PettyCashList.Add(new ReturnPettyCashDto
                    {
                        DipslayUIId = nextId,
                        Date = addPettyCashDto.Date,
                        Reason = addPettyCashDto.Reason,
                        Amount = addPettyCashDto.Amount
                    });

                    MessageBox.Show("تمت الإضافة بنجاح");
                    SundriesReason.Clear();
                    SundriesTotal.Clear();
                    SundriesDate.SelectedDate = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ غير متوقع");
            }
            finally
            {
                AddButton.IsEnabled = true;
            }

            //AddButton.IsEnabled = false;
            //bool AddSundries = await _pettyCashService.AddPettyCash(addPettyCashDto);
            //if (AddSundries)
            //{
            //    int lastDisplayId = PettyCashList.Any()
            //        ? PettyCashList.Max(x => x.DiplayUIId)
            //        : 0;

            //    PettyCashList.Add(new ReturnPettyCashDto
            //    {
            //        DiplayUIId = lastDisplayId+1,
            //        Date = addPettyCashDto.Date,
            //        Reason = addPettyCashDto.Reason,
            //        Amount = addPettyCashDto.Amount
            //    });

            //    MessageBox.Show("تمت الإضافة بنجاح");
            //}
            //else
            //    MessageBox.Show("حدث خطأ أثناء الإضافة");
            //AddButton.IsEnabled = true;

        }

        #endregion

        #region Validate Form  Region

        private bool ValidateForm()
        {
            bool isValid = true;

            // Reset
            SundriesTotal.Tag = null;
            SundriesTotalError.Visibility = Visibility.Collapsed;

            if (!decimal.TryParse(SundriesTotal.Text, out _))
            {
                SundriesTotal.Tag = "Error";
                SundriesTotalError.Visibility = Visibility.Visible;
                isValid = false;
            }
            return isValid;
        }

        #endregion

        #region Dahbord Back Button Click Region

        private void DahbordBackButton_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
        }

        #endregion

        #region Delete Sundries Btn_Click Region

        private async void DeleteSundriesBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = pettyCashGrid.SelectedItem as ReturnPettyCashDto;
            if (selectedItem == null)
            {
                MessageBox.Show("من فضلك اختر صف للحذف");
                return;
            }

            var result = MessageBox.Show(
                "هل أنت متأكد من حذف النثرية؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            bool deleted = await _pettyCashService.DeletePettyCash(selectedItem.Id);

            if (deleted)
            {
                PettyCashList.Remove(selectedItem);
                MessageBox.Show("تم الحذف بنجاح");
                ReIndexDisplayIds();
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء الحذف");
            }
        }

        #endregion

        private void ReIndexDisplayIds()
        {
            for (int i = 0; i < PettyCashList.Count; i++)
                PettyCashList[i].DipslayUIId = i + 1;
        }


        private void DecimalOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(
                ((TextBox)sender).Text + e.Text,
                out _
            );
        }


    }
}
