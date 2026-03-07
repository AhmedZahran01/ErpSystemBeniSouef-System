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
                decimal totalPriceForEveryReciept = invoice.TotalPrice-invoice.Deposit;

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

                        // رقم الإيصال: لو عندك حقل يميز الايصال ضع بدل (receipt.Id) أو أي حقل متاح
                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((receipt.ReceiptNumber != 0 ? receipt.ReceiptNumber.ToString("N0", _ar) : "1"))
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

                        table.Cell()
                           .ColumnSpan(1)
                           .Border(1)
                           .Padding(5)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text(d.AreaName)
                           .FontSize(14);


                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("عدد الايصالات \n" + (d.Receipts?.Count() ?? 0).ToString("N0", _ar))
                            .FontSize(13);

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
                            .Text((receipt.RemainingAfterThisReciept).ToString("N0", _ar))
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

                        table.Cell()
                            .ColumnSpan(1)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((receipt.InstallmentAmount).ToString("N0", _ar))
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
                            .Text((receipt.InstallmentAmount).ToString("N0", _ar))
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
                            .Text(!string.IsNullOrEmpty(receipt.InstallmentDueDate.ToString()) ? receipt.InstallmentDueDate.ToString("yyyy/MM/dd") : string.Empty)
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
                            .Text(!string.IsNullOrEmpty(receipt.InstallmentDueDate.ToString()) ? receipt.InstallmentDueDate.ToString("yyyy/MM/dd") : string.Empty)
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
                            .Text(d.Plans)
                            .FontSize(14);

                        // ================= ROW 7 =================
                        Cell("المنتجات", true);

                        table.Cell()
                            .ColumnSpan(5)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.Items)
                            .FontSize(12);

                        // ================= ROW 8 =================
                        Cell("المندوب", true);

                        table.Cell()
                            .ColumnSpan(2)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.RepresentativeName)
                            .FontSize(12);

                        table.Cell()
                            .ColumnSpan(3)
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                           .Text((d.RepresentativeNumber).ToString("N0", _ar)) 
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
                            .Text(d.Receipts != null && d.Receipts.Count > 0 ? d.Receipts[0].InstallmentAmount.ToString("N0", _ar) : "0")
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
                            .Text(d.Receipts != null && d.Receipts.Count > 0 ? d.Receipts[0].InstallmentAmount.ToString("N0", _ar) : "0")
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
                            .Text(d.InstallmentDueDate.ToString("yyyy/MM/dd"))
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
                            .Text(d.InstallmentDueDate.ToString("yyyy/MM/dd"))
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
}