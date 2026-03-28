using ErpSystemBeniSouef.Core.DTOs.Receipt;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace ErpSystemBeniSouef.ReceiptPdfDocumentFolder
{
    public class AllReceiptsPdfDocument : IDocument
    {
        private readonly List<GetAllReceiptsDto> _receipts;
        private readonly CultureInfo _ar = new CultureInfo("ar-EG");

        public AllReceiptsPdfDocument(List<GetAllReceiptsDto> receipts)
        {
            _receipts = receipts ?? new List<GetAllReceiptsDto>();
            AddReceiptNumbersPerInvoice();
        }


        private void AddReceiptNumbersPerInvoice()
        {
            if (_receipts == null) return;

            foreach (var invoice in _receipts)
            {
                if (invoice?.Receipts == null) continue;

                int num = 1;
                decimal totalPriceForEveryReciept = invoice.TotalPrice - invoice.Deposit;

                foreach (var r in invoice.Receipts)
                {
                    r.ReceiptNumber = num;
                    totalPriceForEveryReciept = totalPriceForEveryReciept - r.InstallmentAmount;
                    r.RemainingAfterThisReciept = totalPriceForEveryReciept;
                    num++;
                }
            }
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        // قالب لطباعة إيصال واحد باستخدام بيانات الـ receipt المحدد
        private void ReceiptTemplateSingle(IContainer container, GetAllReceiptsDto d, ReceiptDetailDto receipt)
        {
            // تأكدنا من أن receipt موجود
            if (receipt == null)
            {
                container.Text("No receipt data").FontSize(12);
                return;
            }

            container
                .Padding(4)
                .Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(50);
                            columns.RelativeColumn();
                            columns.ConstantColumn(58);
                            columns.ConstantColumn(63);
                        });

                        void Cell(string text, bool bold = false)
                        {
                            table.Cell()
                                .Border(1)
                                .Padding(5)
                                .AlignCenter()
                                .Text(text)
                                .FontSize(9);
                        }

                        // ================= ROW 1 =================
                        table.Cell()
                            .Border(1.2F)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("كود العميل")
                            .FontSize(11)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            //.Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text((d.CustomerNumber).ToString("N0", _ar))
                            .Text(ArabicDigits.ToArabicDigits(d.CustomerNumber, _ar)) 
                            .FontSize(10)
                             .Bold();

                        // رقم الإيصال: لو عندك حقل يميز الايصال ضع بدل (receipt.Id) أو أي حقل متاح
                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text((receipt.ReceiptNumber != 0 ? receipt.ReceiptNumber.ToString("N0", _ar) : "1"))
                            .Text((ArabicDigits.ToArabicDigits( receipt.ReceiptNumber != 0 ? receipt.ReceiptNumber.ToString("N0", _ar) : "1") ))
                            .FontSize(14);

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("شركة الرحمة")
                            .FontSize(17)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("الإجمالي ")
                            .FontSize(11)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text((d.TotalPrice).ToString("N0", _ar))
                            .Text(ArabicDigits.ToArabicDigits(d.TotalPrice, _ar))

                            .FontSize(12)
                            .Bold();

                        // ================= ROW 2 =================
                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("المنطقة")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                           .ColumnSpan(1)
                           .Border(1)
                           .Padding(5)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text(d.AreaName)
                           .FontSize(14)
                           .Bold();



                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(ArabicDigits.ToArabicDigits((d.Receipts?.Count() ?? 0).ToString("N0", _ar)))
                            //.Text((d.Receipts?.Count() ?? 0).ToString("N0", _ar))
                            //.Text("عدد الايصالات \n" + (d.Receipts?.Count() ?? 0).ToString("N0", _ar))
                            .FontSize(13)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("مفروشات + اجهزه كهربيه+ ادوات منزليه")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("المقدم")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text((d.Deposit).ToString("N0", _ar))
                            .Text(ArabicDigits.ToArabicDigits(d.Deposit, _ar))
                            .FontSize(14)
                            .Bold();

                        // ================= ROW 3 =================
                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("اسم العميل")
                            .FontSize(11)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(3)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.CustomerName)
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("الباقي بعد هذا الإيصال")
                            .FontSize(8)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                              //.Text((receipt.RemainingAfterThisReciept).ToString("N0", _ar)) 
                              .Text(ArabicDigits.ToArabicDigits(receipt.RemainingAfterThisReciept, _ar))

                            .FontSize(14);

                        // ================= ROW 4 =================
                        table.Cell().ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("العنوان : ")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(3)
                            //.Width(360)

                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.Address)
                            .FontSize(14)
                            .Bold();

                        //table.Cell()
                        //    .ColumnSpan(1)

                        //    .Border(1)
                        //    .AlignCenter()

                        //    .AlignMiddle()
                        //    .Text("الهاتف:")
                        //    .FontSize(11)
                        //    .Bold();

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("الهاتف:" + ArabicDigits.ToArabicDigits(d.MobileNumber)  )
                            .FontSize(11);

                        // ================= ROW 5 =================


                        // ================= ROW 5 =================
                        table.Cell()
                            .ColumnSpan(3)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("بموجب هذا الايصال اتعهد بدفع  مبلغ:")
                            .FontSize(10)
                            .Bold();

                        var amountLong = (long)Math.Truncate(receipt.InstallmentAmount); // تجاهل أجزاء العُشرية هنا
                        var amountNumberText = receipt.InstallmentAmount.ToString("N0", _ar);
                        var amountWords = ArabicNumberToWords.ToWords(amountLong);



                        // النص "فقط و قدره :"
                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(" فقط و قدره : " + amountWords + " جنيها لاغير ")
                            .FontSize(13)
                            .Bold();

                        // هنا نعرض الكلمات بدل تكرار الرقم (مثلاً "مئتان")
                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .Padding(5)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text(amountWords + "جنيها لاغير")   // <-- الكلمات بالعربي
                        //    .FontSize(13);

                        //// الخانة الأخيرة "جنيها لاغير" كما كانت
                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text()
                        //    .FontSize(13)
                        //    .Bold();


                        // الخانة الرقمية الكبيرة (كما كانت)
                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(ArabicDigits.ToArabicDigits(amountNumberText) )
                            .FontSize(15);

                        //table.Cell()
                        //    .ColumnSpan(2)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text("بموجب هذا الايصال اتعهد بدفع  مبلغ:")
                        //    .FontSize(10)
                        //    .Bold();

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .Padding(5)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text((receipt.InstallmentAmount).ToString("N0", _ar))
                        //    .FontSize(15);

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text(" فقط و قدره :")
                        //    .FontSize(13)
                        //    .Bold();

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .Padding(5)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text((receipt.InstallmentAmount).ToString("N0", _ar))
                        //    .FontSize(13);

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text("جنيها لاغير")
                        //    .FontSize(13)
                        //    .Bold();

                        // ================= ROW 6 =================
                        table.Cell()
                           .ColumnSpan(1)
                           .Border(1)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text("تاريخ الدفع")
                           .FontSize(13)
                           .Bold();

                        table.Cell()
                           .ColumnSpan(2)
                            //.Padding(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text(!string.IsNullOrEmpty(receipt.InstallmentDueDate.ToString()) ? receipt.InstallmentDueDate.ToString("yyyy/MM/dd") : string.Empty)
                            .Text(ArabicDigits.ToArabicDigits(!string.IsNullOrEmpty(receipt.InstallmentDueDate.ToString()) ? receipt.InstallmentDueDate.ToString("yyyy/MM/dd") : string.Empty))

                            .FontSize(12);

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text("تاريخ الشراء")
                        //    .FontSize(13)
                        //    .Bold();

                        table.Cell()
                           .ColumnSpan(1)
                           .Border(1)
                           //.AlignCenter()
                           .AlignMiddle()
                           .Text("نظام التقسيط: " + d.Plans)
                           .FontSize(13)
                           .Bold();

                        //string dateOfSale = !string.IsNullOrEmpty(d.InvoiceDate.ToString()) ? d.InvoiceDate.ToString("yyyy/MM/dd") : string.Empty;
                        string dateOfSale = !string.IsNullOrEmpty( ArabicDigits.ToArabicDigits(d.InvoiceDate.ToString()) ) ? ArabicDigits.ToArabicDigits(d.InvoiceDate.ToString()) : string.Empty;
                         
                        //table.Cell()
                        //    .Padding(5)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text(dateOfSale)
                        //    .FontSize(12);

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text("نظام القسط")
                        //    .FontSize(13)
                        //    .Bold();

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            //.AlignCenter()
                            .AlignMiddle()
                            .Text(" تاريخ الشراء " + dateOfSale)
                            .FontSize(11)
                            .Bold();

                        //table.Cell()
                        //    .ColumnSpan(1)
                        //    .Border(1)
                        //    .Padding(5)
                        //    .AlignCenter()
                        //    .AlignMiddle()
                        //    .Text(d.Plans)
                        //    .FontSize(14);

                        // ================= ROW 7 =================
                        //Cell("المنتجات", true);
                        table.Cell()
                           .ColumnSpan(1)
                           .Border(1)
                           .Padding(2)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text("المنتجات")
                           .FontSize(13)
                           .Bold();

                        table.Cell()
                            .ColumnSpan(5)
                            .Border(1)
                            .Padding(5)
                            //.AlignCenter()
                            .AlignMiddle()
                            .Text(d.Items)
                            .FontSize(12)
                            .Bold();


                        // ================= ROW 8 =================
                        //Cell("المندوب", true);
                        table.Cell()
                          .ColumnSpan(1)
                          .Border(1)
                          .Padding(2)
                          .AlignCenter()
                          .AlignMiddle()
                          .Text("المندوب")
                          .FontSize(13)
                          .Bold();

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.RepresentativeName)
                            .FontSize(13)
                            .Bold();


                        table.Cell()
                            .ColumnSpan(3)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                           .Text((ArabicDigits.ToArabicDigits(d.RepresentativePhoneNumber) ))
                            //.Text((d.RepresentativePhoneNumber).ToString("N0", _ar)) 
                            .FontSize(12);


                    });
                });
        }

        // Compose: نلف على كل مجموعة _receipts، وبداخل كل مجموعة نطبع كل إيصال كصفحة مستقلة
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5.Landscape());
                page.Margin(6);
                page.DefaultTextStyle(x => x.FontFamily("Cairo").FontSize(9));
                page.Content().ContentFromRightToLeft()
                .Column(col =>
                {
                    int outerCount = _receipts.Count;
                    for (int i = 0; i < outerCount; i++)
                    {
                        var group = _receipts[i];
                        int innerCount = group.Receipts?.Count ?? 0;

                        for (int j = 0; j < innerCount; j++)
                        {
                            var receipt = group.Receipts[j];

                            // اطبع الإيصال الواحد مستخدماً بيانات المجموعة والايصال
                            col.Item().Element(c => ReceiptTemplateSingle(c, group, receipt));

                            // إذا مش آخر إيصال فلازم نعمل PageBreak
                            bool isLast = (i == outerCount - 1) && (j == innerCount - 1);
                            if (!isLast)
                                col.Item().PageBreak();
                        }
                    }
                });
            });
        }



        // النسخة الأصلية (تحتفظ بها إن حبيت تطبع مجموعة واحدة كمجموعة)
        private void ReceiptTemplate(IContainer container, GetAllReceiptsDto d)
        {
            const float RightBoxWidth = 190;
            const float LogoWidth = 80;
            const float LabelColWidth = 85;

            container
                .Padding(4)
                .Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(90);
                            columns.ConstantColumn(70);
                            columns.ConstantColumn(70);
                            columns.RelativeColumn();
                            columns.ConstantColumn(90);
                            columns.ConstantColumn(80);
                        });

                        void Cell(string text, bool bold = false)
                        {
                            table.Cell()
                                .Border(1)
                                .Padding(5)
                                .AlignCenter()
                                .Text(text)
                                .FontSize(9);
                        }

                        // ================= ROW 1 =================
                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("كود العميل : ")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((d.CustomerNumber).ToString("N0", _ar))
                            .FontSize(14);

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((1).ToString("N0", _ar))
                            .FontSize(14);

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("شركة الرحمة")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("الإجمالي ")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((d.TotalPrice).ToString("N0", _ar))
                            .FontSize(14);

                        // ================= ROW 2 =================
                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("المنطقة")
                            .FontSize(14)
                            .Bold();

                        Cell(d.AreaName);

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("عدد الايصالات" + (d.Receipts?.Count() ?? 0).ToString("N0", _ar))
                            .FontSize(11);

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("مفروشات + اجهزه كهربيه+ ادوات منزليه")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("المقدم")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((d.Deposit).ToString("N0", _ar))
                            .FontSize(14);

                        // ================= ROW 3 =================
                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("اسم العميل : ")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(3)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.CustomerName)
                            .FontSize(14);

                        table.Cell()
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(" الباقي بعد هذا الإيصال ")
                            .FontSize(10);

                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((d.TotalPrice - d.Deposit).ToString("N0", _ar))
                            .FontSize(14);

                        // ================= ROW 4 =================
                        table.Cell().ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("العنوان : ")
                            .FontSize(14)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.Address)
                            .FontSize(14);

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("الهاتف:")
                            .FontSize(11)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.MobileNumber)
                            .FontSize(14);

                        // ================= ROW 5 =================
                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("بموجب هذا الايصال اتعهد بدفع  مبلغ:")
                            .FontSize(10)
                            .Bold();

                        // هنا يظهر اول ايصال من المجموعة (قد يكون مستخدم سابقاً)
                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                        //.Text(d.Receipts != null && d.Receipts.Count > 0 ? d.Receipts[0].InstallmentAmount.ToString("N0", _ar) : "0")
                        .Text(ArabicDigits.ToArabicDigits(d.Receipts != null && d.Receipts.Count > 0 ? d.Receipts[0].InstallmentAmount.ToString("N0", _ar) : "٠"))
                            .FontSize(15);

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(" فقط و قدره :")
                            .FontSize(13)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                        //.Text(d.Receipts != null && d.Receipts.Count > 0 ? d.Receipts[0].InstallmentAmount.ToString("N0", _ar) : "0")
                        .Text(ArabicDigits.ToArabicDigits(d.Receipts != null && d.Receipts.Count > 0 ? d.Receipts[0].InstallmentAmount.ToString("N0", _ar) : "٠"))
                            .FontSize(13);

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("جنيها لاغير")
                            .FontSize(13)
                            .Bold();

                        // ================= ROW 6 =================
                        table.Cell()
                           .ColumnSpan(1)
                           .Border(1)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text("تاريخ الدفع")
                           .FontSize(13)
                           .Bold();

                        table.Cell()
                            .Padding(5)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text(d.InstallmentDueDate.ToString("yyyy/MM/dd"))
                            .Text(ArabicDigits.ToArabicDigits(d.InstallmentDueDate.ToString("yyyy/MM/dd")))
                            .FontSize(12);

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("تاريخ الشراء")
                            .FontSize(13)
                            .Bold();

                        table.Cell()
                            .Padding(5)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            //.Text(d.InstallmentDueDate.ToString("yyyy/MM/dd"))
                            .Text(ArabicDigits.ToArabicDigits(d.InstallmentDueDate.ToString("yyyy/MM/dd")))

                            .FontSize(12);

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("نظام القسط")
                            .FontSize(13)
                            .Bold();

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.AreaName)
                            .FontSize(14);

                        // ================= ROW 7 =================
                        Cell("المنتج", true);

                        table.Cell()
                            .ColumnSpan(5)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.CollectorName)
                            .FontSize(12);

                        // ================= ROW 8 =================
                        Cell("المندوب", true);

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.CollectorName)
                            .FontSize(12);

                        table.Cell()
                            .ColumnSpan(3)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.MobileNumber)
                            .FontSize(12);
                    });
                });
        }


    }

    public static class ArabicNumberToWords
    {
        static readonly string[] Units = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
        static readonly string[] Teens = { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        static readonly string[] Tens = { "", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        static readonly string[] Hundreds = { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };

        public static string ToWords(long number)
        {
            if (number == 0) return "صفر";
            if (number < 0) return "سالب " + ToWords(Math.Abs(number));

            var parts = new List<string>();

            if ((number / 1000000) > 0)
            {
                long millions = number / 1000000;
                parts.Add(TripletToWords(millions) + " " + (millions == 1 ? "مليون" : (millions == 2 ? "مليونان" : "ملايين")));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                long thousands = number / 1000;
                if (thousands == 1) parts.Add("ألف");
                else if (thousands == 2) parts.Add("ألفان");
                else parts.Add(TripletToWords(thousands) + " آلاف");
                number %= 1000;
            }

            if (number > 0)
            {
                parts.Add(TripletToWords(number));
            }

            return string.Join(" و ", parts.Where(p => !string.IsNullOrEmpty(p)));
        }

        static string TripletToWords(long number) // 0..999
        {
            var outParts = new List<string>();

            int h = (int)(number / 100);
            int rem = (int)(number % 100);

            if (h > 0)
            {
                // Use special words for 200 (مئتان) and others from Hundreds[]
                outParts.Add(Hundreds[h]);
            }

            if (rem > 0)
            {
                if (rem < 10)
                {
                    outParts.Add(Units[rem]);
                }
                else if (rem < 20)
                {
                    outParts.Add(Teens[rem - 10]);
                }
                else
                {
                    int t = rem / 10;
                    int u = rem % 10;
                    if (u == 0)
                    {
                        outParts.Add(Tens[t]);
                    }
                    else
                    {
                        // join unit and tens with " و "
                        outParts.Add(Units[u] + " و " + Tens[t]);
                    }
                }
            }

            // join hundreds and remainder with " و " if both exist
            return string.Join(" و ", outParts.Where(s => !string.IsNullOrEmpty(s)));
        }


    }

    public static class ArabicDigits
    {
        public static string ToArabicDigits(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input
                .Replace('0', '٠')
                .Replace('1', '١')
                .Replace('2', '٢')
                .Replace('3', '٣')
                .Replace('4', '٤')
                .Replace('5', '٥')
                .Replace('6', '٦')
                .Replace('7', '٧')
                .Replace('8', '٨')
                .Replace('9', '٩');
        }

        public static string ToArabicDigits(int value, CultureInfo culture)
            => ToArabicDigits(value.ToString("N0", culture));

        public static string ToArabicDigits(decimal value, CultureInfo culture)
            => ToArabicDigits(value.ToString("N0", culture));

        public static string ToArabicDigits(DateTime value, string format = "yyyy/MM/dd")
            => ToArabicDigits(value.ToString(format));
    }


}