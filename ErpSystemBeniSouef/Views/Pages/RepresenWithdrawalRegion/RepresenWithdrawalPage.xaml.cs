using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.RepresentativeWithdrawal;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeWithdrawalDtos;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ErpSystemBeniSouef.Views.Pages.RepresenWithdrawalRegion
{
    /// <summary>
    /// Interaction logic for RepresenWithdrawalPage.xaml
    /// </summary>
    public partial class RepresenWithdrawalPage : Page
    {
        #region Constractor Region

        private readonly IRepresentativeService _representativeService;
        private readonly IRepresentativeWithdrawalService _representativeWithdrawal;
        private ObservableCollection<ReturnRepresentativeWithdrawalDto> representativeWithdrawalDtos;

        public RepresenWithdrawalPage(IRepresentativeWithdrawalService representativeWithdrawal, IRepresentativeService representativeService)
        {
            InitializeComponent();
            _representativeWithdrawal = representativeWithdrawal;
            _representativeService = representativeService;

            Loaded += async (s, e) =>
            {
                await LoadrepresentativeWithdrawal();
                pettyCashGrid.ItemsSource = representativeWithdrawalDtos;
                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();

            };

        }

        #endregion

        #region Load representative Withdrawal to Grid Region

        private async Task LoadrepresentativeWithdrawal()
        {
            var data = await _representativeWithdrawal.GetAllRepresentativeWithdrawal();
            representativeWithdrawalDtos = new ObservableCollection<ReturnRepresentativeWithdrawalDto>(
                data.Select((item, index) => new ReturnRepresentativeWithdrawalDto
                {
                    Id = item.Id,
                    Date = item.Date,
                    Reason = item.Reason,
                    Amount = item.Amount,
                    DisplayUIId = index + 1,
                    representativeName = item.representativeName,
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

            AddRepresentativeWithdrawalDto addPettyCashDto = new AddRepresentativeWithdrawalDto
            {
                Amount = amount,
                Reason = sundriesReason,
                Date = invoiceDate.Value
            };
            if (!ValidateForm())
                return;



            RepresentativeDto selectedSupplier = (RepresentativeDto)RepresentativeCombo.SelectedItem;

            if (selectedSupplier == null)
            {
                MessageBox.Show("  من فضلك ادخل بيانات صحيحة واختر مندوب صحيح");
                return;
            }
            addPettyCashDto.representativeId = selectedSupplier.Id;
            AddButton.IsEnabled = false;
            try
            {
                bool added = await _representativeWithdrawal.AddRepresentativeWithdrawal(addPettyCashDto);
                if (added)
                {
                    int nextId = representativeWithdrawalDtos.Any() ? representativeWithdrawalDtos.Max(x => x.DisplayUIId) + 1 : 1;

                    representativeWithdrawalDtos.Add(new ReturnRepresentativeWithdrawalDto
                    {
                        DisplayUIId = nextId,
                        Date = addPettyCashDto.Date,
                        Reason = addPettyCashDto.Reason,
                        Amount = addPettyCashDto.Amount,
                        representativeId = selectedSupplier.Id,
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

        }

        #endregion


        #region Delete Sundries Btn_Click Region

        private async void DeleteSundriesBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = pettyCashGrid.SelectedItem as ReturnRepresentativeWithdrawalDto;
            if (selectedItem == null)
            {
                MessageBox.Show("من فضلك اختر صف للحذف");
                return;
            }

            var result = MessageBox.Show(
                "هل أنت متأكد من حذف هذا السحب؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            bool deleted = await _representativeWithdrawal.DeleteRepresentativeWithdrawal(selectedItem.Id);

            if (deleted)
            {
                representativeWithdrawalDtos.Remove(selectedItem);
                MessageBox.Show("تم الحذف بنجاح");
                ReIndexDisplayIds();
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء الحذف");
            }
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
            var DashbordRegion = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(DashbordRegion);
        }

        #endregion


        private void ReIndexDisplayIds()
        {
            for (int i = 0; i < representativeWithdrawalDtos.Count; i++)
                representativeWithdrawalDtos[i].DisplayUIId = i + 1;
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
