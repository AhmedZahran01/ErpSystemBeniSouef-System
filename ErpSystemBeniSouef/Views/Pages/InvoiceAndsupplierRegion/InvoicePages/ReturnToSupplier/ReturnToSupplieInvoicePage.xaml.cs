using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.ReturnToSupplieInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.Extensions.DependencyInjection;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.ReturnToSupplier
{
    /// <summary>
    /// Interaction logic for ReturnToSupplieInvoicePage.xaml
    /// </summary>
    public partial class ReturnToSupplieInvoicePage : Page
    { 
        #region Global Variables  Region
        private readonly int _companyNo = 1;
        private readonly ISupplierService _supplierService;
        private readonly IReturnSupplierInvoiceService _returnSupplierInvoiceService;
        private readonly IMapper _mapper;

        IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        ObservableCollection<DtoForReturnSupplierInvoice> observProductsLisLim = new ObservableCollection<DtoForReturnSupplierInvoice>();
        ObservableCollection<DtoForReturnSupplierInvoice> observProductsListFiltered = new ObservableCollection<DtoForReturnSupplierInvoice>();

        #endregion

        #region Constractor Region

        public ReturnToSupplieInvoicePage(ISupplierService supplierService, IReturnSupplierInvoiceService returnSupplierInvoiceService)
        {
            InitializeComponent();
            _supplierService = supplierService;
            _returnSupplierInvoiceService = returnSupplierInvoiceService;
            Loaded += async (s, e) =>
            {
                SuppliersDto = await _supplierService.GetAllAsync();
                dgRepresentatives.ItemsSource = observProductsLisLim;
                dgRepresentatives.SelectedIndex = 0;
                await LoadInvoices();
                cb_ReturnSupplirName.ItemsSource = SuppliersDto;

            };

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<DtoForReturnSupplierInvoice> invoiceDtos = await _returnSupplierInvoiceService.GetAllAsync();
            observProductsLisLim.Clear();
            observProductsListFiltered.Clear();
            foreach (var product in invoiceDtos)
            {
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
            }

        }


        #endregion

        #region Add Button Region

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? invoiceDate = txtSupplirDate.SelectedDate;
            if (invoiceDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }

            SupplierRDto selectedSupplier = (SupplierRDto)cb_ReturnSupplirName.SelectedItem;

            if (selectedSupplier == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            AddReturnSupplierInvoiceDto InputProduct = new AddReturnSupplierInvoiceDto()
            {
                //InvoiceDate = invoiceDate,
                SupplierId = selectedSupplier.Id

            };

            DtoForReturnSupplierInvoice CreateInvoiceDtoRespons = _returnSupplierInvoiceService.AddInvoice(InputProduct);
            if (CreateInvoiceDtoRespons is null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة الفاتوره الكاش بنجاح");

            cb_ReturnSupplirName.SelectedIndex = 0;
            txtSupplirDate.SelectedDate = null;
            observProductsLisLim.Add(CreateInvoiceDtoRespons);
            observProductsListFiltered.Add(CreateInvoiceDtoRespons);

        }

        #endregion

        #region Delete Button Region

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool checkSoftDelete = false;
            if (dgRepresentatives.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }
            List<DtoForReturnSupplierInvoice> selectedItemsDto = dgRepresentatives.SelectedItems.Cast<DtoForReturnSupplierInvoice>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                //bool success = await _returnSupplierInvoiceService.SoftDeleteAsync(item.Id);
                if (true)
                //if (success)
                {
                    observProductsLisLim.Remove(item);
                    observProductsListFiltered.Remove(item);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                string ValueOfString = "فاتوره   ";
                if (deletedCount > 1)
                    ValueOfString = "من الفواتير   ";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي فاتوره بسبب خطأ ما");
            }

        }
        #endregion

        #region dgMainRegions_SelectionChanged Region

        private void dgAllInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRepresentatives.SelectedItem is DtoForReturnSupplierInvoice selected)
            {
                //SupplierRDto selectedSupplier = SuppliersDto.FirstOrDefault(s => s.Id == selected.SupplierId);
                //cb_ReturnSupplirName.SelectedItem = selectedSupplier;
                //txtSupplirDate.SelectedDate = selected.InvoiceDate;
                //editBtn.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Btn Edit Click  Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
        //    if (dgRepresentatives.SelectedItem is not DtoForReturnSupplierInvoice selected)
        //    {
        //        MessageBox.Show("من فضلك اختر فاتوره محدده للتعديل");
        //        return;
        //    }

        //    DateTime UpdateInvoiceDate = txtSupplirDate.SelectedDate ?? DateTime.UtcNow;
        //    //if(UpdateInvoiceDate is null )
        //    //{
        //    //    UpdateInvoiceDate = DateTime.UtcNow;
        //    //}

        //    int updateSupplierId = ((SupplierRDto)cb_ReturnSupplirName.SelectedItem).Id;

        //    var updateDto = new UpdateInvoiceDto()
        //    {
        //        Id = selected.Id,
        //        InvoiceDate = UpdateInvoiceDate,
        //        SupplierId = updateSupplierId
        //    };

        //    bool success = _returnSupplierInvoiceService.Update(updateDto);

        //    if (success)
        //    {
        //        SupplierRDto supplierDto = SuppliersDto.FirstOrDefault(i => i.Id == selected.SupplierId);


        //        selected.Id = ((SupplierRDto)cb_ReturnSupplirName.SelectedItem).Id;
        //        selected.Supplier = ((SupplierRDto)cb_ReturnSupplirName.SelectedItem);
        //        selected.SupplierName = ((SupplierRDto)cb_ReturnSupplirName.SelectedItem).Name;
        //        selected.InvoiceDate = UpdateInvoiceDate;
        //        txtSupplirDate.SelectedDate = UpdateInvoiceDate;
        //        MessageBox.Show("تم تعديل المنطقة بنجاح");
        //        dgRepresentatives.Items.Refresh(); // لتحديث الجدول
        //    }
        //    else
        //    {
        //        MessageBox.Show("حدث خطأ أثناء التعديل");
            //}
        }



        #endregion

        #region Search By Item FullName  Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observProductsLisLim
                .Where(i => i.SupplierName != null && i.SupplierName.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observProductsListFiltered.Clear();
            foreach (var item in filtered)
            {
                observProductsListFiltered.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.SupplierName);
            if (suggestions.Any())
            {
                dgRepresentatives.ItemsSource = filtered;
                //SuggestionsItemsListBox.ItemsSource = suggestions;
                //SuggestionsPopup.IsOpen = true;
            }
            else
            {
                //SuggestionsPopup.IsOpen = false;
            }

        }

        #endregion

        #region ReturnInvoice Mouse Double Region

        private void dgCashInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgRepresentatives.SelectedItem is DtoForReturnSupplierInvoice selectedInvoice)
            {
            //    var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            //    var cashInvoiceService = App.AppHost.Services.GetRequiredService<IReturnSupplierInvoiceItemService>();
            //    var mapper = App.AppHost.Services.GetRequiredService<IMapper>();

            //    // افتح صفحة التفاصيل
            //    var detailsPage = new ReturnSupplierInvoiceDetailsPage(selectedInvoice, productService, cashInvoiceService, mapper);
            //    NavigationService?.Navigate(detailsPage);
            }
        }


        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicesRegion(_companyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
        }

        #endregion
    }
}
