using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CashCustomerInvoiceServices;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.CustomersRegion.CashPage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace ErpSystemBeniSouef.Views.Pages.CustomersRegion
{
    /// <summary>
    /// Interaction logic for CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        #region Global Variables  Region
        private int _currentCustomerId = 0;

        //private readonly int _companyNo = 1;
        //private readonly ISupplierService _supplierService;
        //private readonly ICashInvoiceService _cashInvoiceService;
        //private readonly IMapper _mapper;
        //IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        int countDisplayNo = 0;

        List<ReturnCustomerInvoiceListDTO> observCustomerInvoiceList = new List<ReturnCustomerInvoiceListDTO>();
        ObservableCollection<ReturnCustomerInvoiceListDTO> observCustomerInvoiceFilteredList = new ObservableCollection<ReturnCustomerInvoiceListDTO>();

        private readonly ICustomerInvoiceService _customerInvoiceService;
        private readonly IProductService _productService;
        private readonly IMainAreaService _mainAreaService;
        private readonly ISubAreaService _subAreaService;
        private readonly IRepresentativeService _representativeService;
        ObservableCollection<ProductDto> observProductsList = new ObservableCollection<ProductDto>();
        ObservableCollection<ProductDto> observProductsListFiltered = new ObservableCollection<ProductDto>();

        private IReadOnlyList<SubAreaDto> allSubAreas = new List<SubAreaDto>();
        private ObservableCollection<SubAreaDto> observalSubRegionFilter = new();


        ObservableCollection<DisplayForUiCustomerinvoicedtos> displayItemsGrid = new ObservableCollection<DisplayForUiCustomerinvoicedtos>();
        int countItemGridDisplayNo = 1;

        List<Customerinvoicedtos> customerinvoicedtosList = new List<Customerinvoicedtos>();

        List<Installmentsdtos> installmentsdtosList = new List<Installmentsdtos>();


        #endregion

        #region Constractor Region

        public CustomersPage(ICustomerInvoiceService customerInvoiceService, IProductService productService,
                              IMainAreaService mainAreaService, ISubAreaService subAreaService, IRepresentativeService representativeService)
        {
            InitializeComponent();
            _customerInvoiceService = customerInvoiceService;
            _productService = productService;
            _mainAreaService = mainAreaService;
            _subAreaService = subAreaService;
            _representativeService = representativeService;
            Loaded += async (s, e) =>
            {
                //SuppliersDto = await _supplierService.GetAllAsync();
                //cb_SuppliersName.ItemsSource = SuppliersDto;
                //cb_SuppliersName.SelectedIndex = 0;
                //await LoadInvoices();


                await LoadInvoices();
                CustomersGrid.ItemsSource = observCustomerInvoiceFilteredList;
                ProductTypeCombo.ItemsSource = await _productService.GetAllCategoriesAsync();
                ProductTypeCombo.SelectedIndex = 0;

                ProductCombo.ItemsSource = observProductsListFiltered;
                MainAreaCombo.ItemsSource = _mainAreaService.GetAll();
                MainAreaCombo.SelectedIndex = 0;

                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();
                RepresentativeCombo.SelectedIndex = 0;



            };

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            var CustomerInvoiceList = await _customerInvoiceService.GetAllCustomerInvoicesAsync();
            observCustomerInvoiceList.Clear();
            observCustomerInvoiceFilteredList.Clear();
            if (CustomerInvoiceList.Data != null)
            {
                foreach (var product in CustomerInvoiceList.Data)
                {
                    product.DisplayId = countDisplayNo + 1;
                    observCustomerInvoiceList.Add(product);
                    observCustomerInvoiceFilteredList.Add(product);
                    countDisplayNo++;
                }
            }


            IReadOnlyList<ProductDto> products = _productService.GetAll();
            foreach (var product in products)
            {
                observProductsList.Add(product);
            }

            allSubAreas = _subAreaService.GetAll();

        }


        #endregion

        #region cb Product Type Selection Changed Region

        private void cbProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductTypeCombo.SelectedItem is CategoryDto s)
            {
                var filtered = observProductsList.Where(p => p.CategoryId == s.Id).ToList();
                observProductsListFiltered.Clear();
                foreach (var product in filtered)
                {
                    observProductsListFiltered.Add(product);
                }
                ProductCombo.ItemsSource = observProductsListFiltered;
                ProductCombo.SelectedIndex = 0;

            }

        }

        #endregion

        #region  Add Customer Region

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("الرجاء تصحيح الحقول المحددة باللون الأحمر", "خطأ في الإدخال", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show(" ... \n جاري الحفظ في قاعده البيانات  تم التحقق من جميع البيانات بنجاح!", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);

            decimal totalOfItems = 0;
            int NoOfMonthsTxt1 = int.TryParse(setNoOfMonthsTxt.Text, out int amount) ? amount : 0;
            int ValueOfPriceTxt1 = int.TryParse(setValueOfPriceTxt.Text, out int priceV) ? priceV : 0;

            int NoOfMonthsTxt2 = int.TryParse(setNoOfMonthsTxt2.Text, out int amount2) ? amount2 : 0;
            int ValueOfPriceTxt2 = int.TryParse(setValueOfPriceTxt2.Text, out int priceV2) ? priceV2 : 0;

            decimal depositFromUi = decimal.TryParse(PayTxt.Text, out decimal depositValue) ? depositValue : 0;


            decimal TotalOfInvoiceItems = NoOfMonthsTxt1 * ValueOfPriceTxt1 +
                                            NoOfMonthsTxt2 * ValueOfPriceTxt2 + depositFromUi;
            foreach (var item in customerinvoicedtosList)
            {
                totalOfItems += item.Total;
            }

            if (totalOfItems != TotalOfInvoiceItems)
            {
                MessageBox.Show("نظام التقسيط غير صحيح \n \n يرجي المراجعه ");

                return;
            }
            RepresentativeDto selectedRepresentative = (RepresentativeDto)RepresentativeCombo.SelectedItem;
            SubAreaDto selectedsubArea = (SubAreaDto)SubAreaCombo.SelectedItem;
            DateTime? invoiceDate = FirstInvoiceDatePicker.SelectedDate;
            DateTime? saleDatePicker = SaleDatePicker.SelectedDate;

            Installmentsdtos FirstInstallmentsdtos = new Installmentsdtos()
            {
                NumberOfMonths = NoOfMonthsTxt1,
                Amount = ValueOfPriceTxt1,
            };

            Installmentsdtos secondInstallmentsdtos = new Installmentsdtos()
            {
                NumberOfMonths = NoOfMonthsTxt2,
                Amount = ValueOfPriceTxt2,
            };
            installmentsdtosList.Add(FirstInstallmentsdtos);
            installmentsdtosList.Add(secondInstallmentsdtos);
            CreateCustomerInvoiceDTO ddssa = new CreateCustomerInvoiceDTO()
            {
                Address = AddressTxt.Text,
                CollectorId = selectedRepresentative.Id,
                RepresentativeId = selectedRepresentative.Id,

                customerinvoicedtos = customerinvoicedtosList,
                CustomerNumber = int.TryParse(CustomerNumberTxt.Text, out int quantity) ? quantity : 0,
                MobileNumber = PhoneTxt.Text,
                Name = NameTxt.Text,
                NationalNumber = numberIdTxt.Text,
                SubAreaId = selectedsubArea.Id,
                Deposit = depositFromUi,
                FirstInvoiceDate = invoiceDate ?? DateTime.UtcNow,
                SaleDate = saleDatePicker ?? DateTime.UtcNow,

                //installmentsdtos = null,
                installmentsdtos = installmentsdtosList,

            };
            _customerInvoiceService.CreateCustomerInvoiceAsync(ddssa);
        }

        #endregion

        #region Validate Inputs Region

        private bool ValidateInputs()
        {
            bool isValid = true;

            // Helper function to reset style
            void ResetControl(Control ctrl)
            {
                ctrl.ClearValue(Border.BorderBrushProperty);
                ctrl.ClearValue(ToolTipProperty);
            }

            // Helper function to set error style
            void SetError(Control ctrl, string message)
            {
                ctrl.BorderBrush = Brushes.Red;
                ctrl.ToolTip = message;
                isValid = false;
            }

            void SetErrorForComboBox(Control ctrl, string message)
            {
                ctrl.BorderBrush = Brushes.Red;
                ctrl.Background = new SolidColorBrush(Color.FromRgb(255, 230, 230)); // Light-red
                ctrl.ToolTip = message;
                isValid = false;
            }

            // Reset all first
            ResetControl(CustomerNumberTxt);
            ResetControl(NameTxt);
            ResetControl(PayTxt);
            ResetControl(AddressTxt);
            ResetControl(PhoneTxt);
            ResetControl(numberIdTxt);
            ResetControl(SaleDatePicker);
            ResetControl(FirstInvoiceDatePicker);
            ResetControl(MainAreaCombo);
            ResetControl(SubAreaCombo);
            ResetControl(RepresentativeCombo);

            // رقم العميل Required
            if (string.IsNullOrWhiteSpace(CustomerNumberTxt.Text))
                SetError(CustomerNumberTxt, "يرجى إدخال رقم العميل");

            // اسم العميل Required
            if (string.IsNullOrWhiteSpace(NameTxt.Text))
                SetError(NameTxt, "يرجى إدخال اسم العميل");

            // المقدم Required و رقم
            if (string.IsNullOrWhiteSpace(PayTxt.Text) || !decimal.TryParse(PayTxt.Text, out _))
                SetError(PayTxt, "يرجى إدخال مبلغ صحيح");

            // العنوان Required
            if (string.IsNullOrWhiteSpace(AddressTxt.Text))
                SetError(AddressTxt, "يرجى إدخال العنوان");

            // رقم الموبايل Required و رقم
            if (string.IsNullOrWhiteSpace(PhoneTxt.Text) || !long.TryParse(PhoneTxt.Text, out _))
                SetError(PhoneTxt, "يرجى إدخال رقم موبايل صحيح");

            // الرقم القومي Required و رقم
            if (string.IsNullOrWhiteSpace(numberIdTxt.Text) || !long.TryParse(numberIdTxt.Text, out _))
                SetError(numberIdTxt, "يرجى إدخال رقم قومي صحيح");

            // تواريخ Required
            if (!SaleDatePicker.SelectedDate.HasValue)
                SetError(SaleDatePicker, "يرجى تحديد تاريخ البيع");

            if (!FirstInvoiceDatePicker.SelectedDate.HasValue)
                SetError(FirstInvoiceDatePicker, "يرجى تحديد تاريخ أول فاتورة");

            // Combobox Required
            if (MainAreaCombo.SelectedItem == null)
                SetErrorForComboBox(MainAreaCombo, "يرجى اختيار المنطقة");

            if (SubAreaCombo.SelectedItem == null)
                SetErrorForComboBox(SubAreaCombo, "يرجى اختيار المنطقة الفرعية");

            if (RepresentativeCombo.SelectedItem == null)
                SetErrorForComboBox(RepresentativeCombo, "يرجى اختيار المندوب");

            return isValid;
        }

        #endregion

        #region cb Main Area Selection Changed Region
        private void cbMainArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainAreaCombo.SelectedItem is MainAreaDto s)
            {
                var filtered = allSubAreas.Where(p => p.MainAreaId == s.Id).ToList();
                observalSubRegionFilter.Clear();
                foreach (var product in filtered)
                {
                    observalSubRegionFilter.Add(product);
                }
                SubAreaCombo.ItemsSource = observalSubRegionFilter;
                SubAreaCombo.SelectedIndex = 0;

            }
        }

        #endregion

        #region Delete Button Region

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر عميل أولاً من الجدول.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // رسالة تأكيد
            var result = MessageBox.Show(
                "هل أنت متأكد أنك تريد حذف هذا العميل والفواتير الخاصة به؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            // تنفيذ الحذف من السيرفس
            var deleteResponse = await _customerInvoiceService.DeleteCustomerAsync(_currentCustomerId);

            if (!deleteResponse.Success)
            {
                MessageBox.Show(deleteResponse.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("تم حذف العميل وكل البيانات المرتبطة به بنجاح.", "تم", MessageBoxButton.OK, MessageBoxImage.Information);

            // تنظيف الإدخالات
            ClearCustomerInputs();

            // إعادة تحميل البيانات
            countDisplayNo = 0;
            await LoadInvoices();

            // ربط الجدول بعد التحديث
            CustomersGrid.ItemsSource = null;
            CustomersGrid.ItemsSource = observCustomerInvoiceFilteredList;

            // تعطيل الأزرار
            UpdateCustomerData.IsEnabled = false;
            deleteCustomerInvoice.IsEnabled = false;

            _currentCustomerId = 0;
        }

        private void ClearCustomerInputs()
        {
            CustomerNumberTxt.Text = "";
            NameTxt.Text = "";
            PhoneTxt.Text = "";
            AddressTxt.Text = "";
            numberIdTxt.Text = "";
            PayTxt.Text = "";

            SaleDatePicker.SelectedDate = null;
            FirstInvoiceDatePicker.SelectedDate = null;

            MainAreaCombo.SelectedItem = null;
            SubAreaCombo.SelectedItem = null;
            RepresentativeCombo.SelectedItem = null;

            displayItemsGrid.Clear();
            dgInvoiceItems.ItemsSource = displayItemsGrid;
        }


        #endregion

        #region Edit Button Region

        private async void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // لازم يكون فيه عميل مختار
                if (_currentCustomerId == 0)
                {
                    MessageBox.Show("لم يتم اختيار عميل للتعديل", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // التحقق من الحقول المطلوبة
                if (!ValidateInputs())
                {
                    MessageBox.Show("يرجى تصحيح الأخطاء أولاً", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int RepresentativeDtoId = ((RepresentativeDto)RepresentativeCombo.SelectedItem).Id;
                // جمع البيانات الجديدة
                var updatedCustomer = new UpdateCustomerInvoiceDTO
                {
                    //Id = _currentCustomerId.Value,
                    CustomerNumber = int.Parse(CustomerNumberTxt.Text),
                    Name = NameTxt.Text,
                    NationalNumber = numberIdTxt.Text,
                    MobileNumber = PhoneTxt.Text,
                    Address = AddressTxt.Text,
                    //MainAreaId = ((MainAreaDto)MainAreaCombo.SelectedItem).Id,
                    SubAreaId = ((SubAreaDto)SubAreaCombo.SelectedItem).Id,
                    RepresentativeId = RepresentativeDtoId,
                    Deposit = decimal.Parse(PayTxt.Text),
                    SaleDate = SaleDatePicker.SelectedDate ?? DateTime.Now,
                    FirstInvoiceDate = FirstInvoiceDatePicker.SelectedDate ?? DateTime.Now,
                    //Items = customerinvoicedtosList.ToList(),
                    CollectorId = RepresentativeDtoId
                };

                // إرسال التعديل للسيرفس
                var Satus = await _customerInvoiceService.UpdateCustomerInvoiceAsync(_currentCustomerId, updatedCustomer);

                if (Satus.Success)
                {
                    MessageBox.Show("تم تعديل العميل والفاتورة بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);

                    // تحديث الجدول
                    LoadInvoices();

                    // تنظيف البيانات
                    ClearInputs();
                    _currentCustomerId = 0;
                }
                else
                {
                    MessageBox.Show("حدث خطأ أثناء التعديل: " + Satus.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputs()
        {
            CustomerNumberTxt.Text = "";
            NameTxt.Text = "";
            numberIdTxt.Text = "";
            PhoneTxt.Text = "";
            AddressTxt.Text = "";
            PayTxt.Text = "";
            MainAreaCombo.SelectedItem = null;
            SubAreaCombo.SelectedItem = null;
            RepresentativeCombo.SelectedItem = null;
            SaleDatePicker.SelectedDate = null;
            FirstInvoiceDatePicker.SelectedDate = null;

            displayItemsGrid.Clear();
            customerinvoicedtosList.Clear();
            dgInvoiceItems.ItemsSource = null;

            _currentCustomerId = 0;
        }



        #endregion

        #region Add Invoice Item Customer Region

        private void AddInvoiceItemCustomer_Click(object sender, RoutedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            CategoryDto selectedProductType = (CategoryDto)ProductTypeCombo.SelectedItem;
            Customerinvoicedtos customerinvoicedtoss = new Customerinvoicedtos()
            {
                Price = decimal.TryParse(PriceTxt.Text, out decimal price) ? price : 0,
                Quantity = int.TryParse(QuantityTxt.Text, out int quantity) ? quantity : 0,
                ProductId = selectedProduct.Id,
            };
            customerinvoicedtosList.Add(customerinvoicedtoss);

            displayItemsGrid.Add(new DisplayForUiCustomerinvoicedtos()
            {
                Price = decimal.TryParse(PriceTxt.Text, out decimal pwa) ? pwa : 0,
                Quantity = int.TryParse(QuantityTxt.Text, out int pa) ? pa : 0,
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.ProductName,
                ProductCategoryName = selectedProductType.Name,
                DisplayId = countItemGridDisplayNo++
            });

            dgInvoiceItems.ItemsSource = displayItemsGrid;

        }

        #endregion

        #region Product Selection Changed Region

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductDto selectedProduct = (ProductDto)ProductCombo.SelectedItem;
            if (selectedProduct is not null)
            {
                PriceTxt.Text = selectedProduct.PurchasePrice.ToString() ?? "";
            }
        }

        #endregion

        #region Toggle Basic Card Region

        private void ToggleBasicCard_Click(object sender, RoutedEventArgs e)
        {
            if (BasicCardContent.Visibility == Visibility.Visible)
            {
                BasicCardContent.Visibility = Visibility.Collapsed;
                ToggleBasicCardBtn.Content = "▲";   // شكل الفتح
            }
            else
            {
                BasicCardContent.Visibility = Visibility.Visible;
                ToggleBasicCardBtn.Content = "▼";   // شكل القفل
            }
        }

        #endregion

        #region Toggle Sub Basic Card Region

        private void ToggleSubBasicCard_Click(object sender, RoutedEventArgs e)
        {
            if (SubBasicCardContent.Visibility == Visibility.Visible)
            {
                SubBasicCardContent.Visibility = Visibility.Collapsed;
                ToggleSubBasicCardBtn.Content = "▲";
            }
            else
            {
                SubBasicCardContent.Visibility = Visibility.Visible;
                ToggleSubBasicCardBtn.Content = "▼";
            }
        }

        #endregion

        #region Toggle Plans Card Region

        private void TogglePlansCard_Click(object sender, RoutedEventArgs e)
        {
            if (PlansCardContent.Visibility == Visibility.Visible)
            {
                PlansCardContent.Visibility = Visibility.Collapsed;
                TogglePlansCardBtn.Content = "▲";
            }
            else
            {
                PlansCardContent.Visibility = Visibility.Visible;
                TogglePlansCardBtn.Content = "▼";
            }
        }

        #endregion

        #region Toggle Products Card Region
        private void ToggleProductsCard_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsCardContent.Visibility == Visibility.Visible)
            {
                ProductsCardContent.Visibility = Visibility.Collapsed;
                ToggleProductsCardBtn.Content = "▲";
            }
            else
            {
                ProductsCardContent.Visibility = Visibility.Visible;
                ToggleProductsCardBtn.Content = "▼";
            }
        }

        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #endregion

        private void CustomersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomersGrid.SelectedItem is ReturnCustomerInvoiceListDTO selectedCustomer)
            {
                FillCustomerInputs(selectedCustomer);
                _currentCustomerId = selectedCustomer.Id;

                UpdateCustomerData.IsEnabled = true;
                deleteCustomerInvoice.IsEnabled = true;
            }
        }

        private void FillCustomerInputs(ReturnCustomerInvoiceListDTO customer)
        {
            // بيانات العميل
            CustomerNumberTxt.Text = customer.CustomerNumber.ToString();
            NameTxt.Text = customer.Name;
            PhoneTxt.Text = customer.MobileNumber;
            AddressTxt.Text = customer.Address;
            numberIdTxt.Text = customer.NationalNumber;

            // المقدم
            PayTxt.Text = customer.Deposit.ToString();

            // التواريخ
            SaleDatePicker.SelectedDate = customer.SaleDate;
            FirstInvoiceDatePicker.SelectedDate = customer.FirstInvoiceDate;

            // MainArea
            var mainArea = _mainAreaService.GetAll()
                .FirstOrDefault(x => x.Id == customer.MainAreaId);
            if (mainArea != null)
            {
                MainAreaCombo.SelectedItem = mainArea;
                MainAreaCombo.SelectedValue = customer.MainAreaId;
            }

            // SubArea (بعد ما الـ MainArea يتغير)
            var subArea = allSubAreas
                .FirstOrDefault(x => x.Id == customer.SubAreaId);
            if (subArea != null)
            {
                SubAreaCombo.SelectedItem = subArea;
                //MainAreaCombo.SelectedValue = customer.SubAreaId;
            }

            // المندوب
            var rep = RepresentativeCombo.Items
                .Cast<RepresentativeDto>()
                .FirstOrDefault(x => x.Id == customer.RepresentativeId);
            if (rep != null)
            {
                RepresentativeCombo.SelectedItem = rep;
            }

            // تحميل العناصر (Items)
            //LoadInvoiceItems(customer.Id);
        }

        private async void LoadInvoiceItems(int invoiceId)
        {
            displayItemsGrid.Clear();
            customerinvoicedtosList.Clear();

            var details = await _customerInvoiceService.GetCustomerInvoiceByIdAsync(invoiceId);

            foreach (var item in details.Data.CustomerInvoiceItems)
            {
                customerinvoicedtosList.Add(new Customerinvoicedtos
                {
                    ProductId = item.ProductIdDto,
                    Quantity = item.Quantity,
                    Price = item.Price
                });

                displayItemsGrid.Add(new DisplayForUiCustomerinvoicedtos
                {
                    DisplayId = countItemGridDisplayNo++,
                    ProductId = item.ProductIdDto,
                    ProductName = item.ProductName,
                    ProductCategoryName = item.CategoryName,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            dgInvoiceItems.ItemsSource = displayItemsGrid;
        }

        private void DeleteButton_Click2(object sender, RoutedEventArgs e)
        {

            var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var subAreaService = App.AppHost.Services.GetRequiredService<ISubAreaService>();
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var cashCustomerInvoiceService = App.AppHost.Services.GetRequiredService<ICashCustomerInvoiceService>();


            var detailsPage = new CashCustomerInvoicePage(productService, mainAreaService, subAreaService,
                                        representativeService, cashCustomerInvoiceService);

            NavigationService?.Navigate(detailsPage);
        }

    }
}
