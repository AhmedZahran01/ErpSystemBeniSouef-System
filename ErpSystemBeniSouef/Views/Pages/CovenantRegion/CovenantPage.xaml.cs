using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Covenant;
using ErpSystemBeniSouef.Core.DTOs.Covenant;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ErpSystemBeniSouef.Views.Pages.CovenantRegion
{
    public partial class CovenantPage : Page
    {
        private readonly IRepresentativeService _representativeService;
        private readonly ICovenantService _covenantService;

        private ObservableCollection<ReturnCovenant> CovenantList =
            new ObservableCollection<ReturnCovenant>();

        public CovenantPage(
            IRepresentativeService representativeService,
            ICovenantService covenantService)
        {
            InitializeComponent();
            _representativeService = representativeService;
            _covenantService = covenantService;

            Loaded += async (s, e) =>
            {
                await LoadData();
                ConvenantDataGrid.ItemsSource = CovenantList;
            };
        }
        private async Task LoadData()
        {
            RepresenComoBox.ItemsSource = await _representativeService.GetAllAsync();
            RepresenComoBox.SelectedIndex = 0;

            var covenants = await _covenantService.GetAllCovenants();

            CovenantList.Clear();

            int index = 1;
            foreach (var item in covenants)
            {
                CovenantList.Add(new ReturnCovenant
                {
                    Id = item.Id,
                    CovenantType = item.CovenantType,
                    CovenantDate = item.CovenantDate,
                    DisplayUIId = index++
                });
            }
        }


        #region Add Covenant

        private async void AddConventBtn(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            AddButton.IsEnabled = false;

            try
            {
                var input = new AddCovenantToRepresentative
                {
                    MonthDate = DateMonth.SelectedDate!.Value,
                    CovenantDate = ConvenentDate.SelectedDate!.Value,
                    CovenantType = TypeOfCovenant.SelectedValue!.ToString(),
                    RepresentativeId =
                        ((RepresentativeDto)RepresenComoBox.SelectedItem).Id
                };

                bool added = await _covenantService.addCovenant(input);

                if (!added)
                {
                    MessageBox.Show("حدث خطأ أثناء الإضافة");
                    return;
                }

                int nextId = CovenantList.Any()
      ? CovenantList.Max(x => x.DisplayUIId) + 1
      : 1;

                CovenantList.Add(new ReturnCovenant
                {
                    DisplayUIId = nextId,
                    CovenantDate = input.CovenantDate,
                    CovenantType = input.CovenantType,
                    Id = 0 // لو السيرفر بيرجعه بعدين
                });

                MessageBox.Show("تم إضافة العهدة بنجاح");
                ClearForm();
            }
            finally
            {
                AddButton.IsEnabled = true;
            }
        }

        #endregion

        #region Validation

        private bool ValidateForm()
        {
            bool valid = true;

            if (DateMonth.SelectedDate == null)
                valid = false;

            if (ConvenentDate.SelectedDate == null)
                valid = false;

            if (RepresenComoBox.SelectedItem == null)
                valid = false;

            if (TypeOfCovenant.SelectedValue == null)
                valid = false;

            if (!valid)
                MessageBox.Show("من فضلك ادخل البيانات كاملة");

            return valid;
        }

        private void ClearForm()
        {
            ConvenentDate.SelectedDate = null;
            TypeOfCovenant.SelectedIndex = 0;
        }

        #endregion

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.MainWindow.Frame
                .NavigationService.Navigate(new Dashboard());
        }

        private void DataConvenantItemsOpen(object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            var productService =
                App.AppHost.Services.GetRequiredService<IProductService>();
            var covenantService =
                App.AppHost.Services.GetRequiredService<ICovenantService>();

            MainWindowViewModel.MainWindow.Frame
                .NavigationService.Navigate(
                    new CovenantItemsPage(productService, covenantService));
        }

        private async void DeleteCovenantBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ConvenantDataGrid.SelectedItem as ReturnCovenant;

            if (selectedItem == null)
            {
                MessageBox.Show("من فضلك اختر صف للحذف");
                return;
            }

            var result = MessageBox.Show(
                "هل أنت متأكد من حذف العهدة؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            DeleteButton.IsEnabled = false;

            try
            {
                bool deleted = await _covenantService.DeleteCovenant(selectedItem.Id);

                if (deleted)
                {
                    CovenantList.Remove(selectedItem);
                    ReIndexDisplayIds();
                    MessageBox.Show("تم الحذف بنجاح");
                }

                else
                {
                    MessageBox.Show("حدث خطأ أثناء الحذف");
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ غير متوقع");
            }
            finally
            {
                DeleteButton.IsEnabled = true;
            }
        }

        private void ReIndexDisplayIds()
        {
            for (int i = 0; i < CovenantList.Count; i++)
                CovenantList[i].DisplayUIId = i + 1;
        }

    }
}
