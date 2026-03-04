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
        //private readonly GetAllReceiptsDto _receipts;
        private readonly CultureInfo _ar = new CultureInfo("ar-EG");

        public AllReceiptsPdfDocument(List<GetAllReceiptsDto> receipts)
        {
            _receipts = receipts;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // جرب A5.Landscape أو A4 عند الحاجة
                page.Size(PageSizes.A5.Landscape());
                page.Margin(6);
                // استبدل "Cairo" باسم الخط الذي سجلته في تطبيقك (انظر ملاحظة أعلى)
                page.DefaultTextStyle(x => x.FontFamily("Cairo").FontSize(9));
                page.Content().ContentFromRightToLeft()

                .Column(col =>
                {
                    foreach (var r in _receipts)
                    {
                        col.Item().Element(c => ReceiptTemplate(c, r));
                        col.Item().PageBreak();
                    }
                });
            });
        }

        private void ReceiptTemplate(IContainer container, GetAllReceiptsDto d)
        {
            // ثوابت قابلة للتعديل لعمل معايرة لطباعة دقيقة
            const float RightBoxWidth = 190;    // العرض لصندوق بيانات الإيصال (اليمين)
            const float LogoWidth = 80;         // عرض مكان الشعار (اليسار)
            const float LabelColWidth = 85;     // عرض أعمدة التسميات في الجدول الرئيسي

            container
                .Padding(4)
                .Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(90);   // col 1
                            columns.ConstantColumn(70);   // col 2
                            columns.ConstantColumn(70);   // col 3
                            columns.RelativeColumn();     // col 4
                            columns.ConstantColumn(90);   // col 5
                            columns.ConstantColumn(80);   // col 6
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
                        //Cell("كود العميل", true);

                        table.Cell()
                            .Border(1) // 👈 رقم أكبر = خط أسمك
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("كود العميل : ")
                            .FontSize(14)
                            .Bold();

                        //Cell(d.CustomerNumber.ToString());
                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((d.CustomerNumber).ToString("N0", _ar))
                            .FontSize(14);



                        //Cell("1"); // رقم الإيصال
                        table.Cell()
    .Border(1)
    .Padding(5)
    .AlignCenter()
    .AlignMiddle()
    .Text((1).ToString("N0", _ar))
    .FontSize(14);


                        //Cell("شركة الرحمة", true);
                        table.Cell()
     .Border(1) // 👈 رقم أكبر = خط أسمك
     .AlignCenter()
     .AlignMiddle()
     .Text("شركة الرحمة")
     .FontSize(14)
     .Bold();



                        //Cell("الإجمالي", true);
                        table.Cell()
     .Border(1) // 👈 رقم أكبر = خط أسمك
     .AlignCenter()
     .AlignMiddle()
     .Text("الإجمالي ")
     .FontSize(14)
     .Bold();



                        //Cell(d.TotalPrice.ToString("N0", _ar), true);
                        table.Cell()
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((d.TotalPrice).ToString("N0", _ar))
                            .FontSize(14);


                        // ================= ROW 2 =================
                        //Cell("المنطقة", true);
                        table.Cell()
.Border(1) // 👈 رقم أكبر = خط أسمك
.AlignCenter()
.AlignMiddle()
.Text("المنطقة")
.FontSize(14)
.Bold();


                        Cell(d.AreaName);

                        //Cell("9");
                        table.Cell()
                           .Border(1)
                           .Padding(5)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text("عدد الايصالات" + (9).ToString("N0", _ar))
                           .FontSize(11);


                        //Cell("مفروشات", true);
                        table.Cell()
.Border(1) // 👈 رقم أكبر = خط أسمك
.AlignCenter()
.AlignMiddle()
.Text("مفروشات + اجهزه كهربيه+ ادوات منزليه")
.FontSize(14)
.Bold();



                        //Cell("المقدم", true);
                        table.Cell()
.Border(1) // 👈 رقم أكبر = خط أسمك
.AlignCenter()
.AlignMiddle()
.Text("المقدم")
.FontSize(14)
.Bold();



                        //Cell(d.Deposit.ToString("N0", _ar));
                        table.Cell()
                           .Border(1)
                           .Padding(5)
                           .AlignCenter()
                           .AlignMiddle()
                           .Text((d.Deposit).ToString("N0", _ar))
                           .FontSize(14);


                        // ================= ROW 3 =================
                        //Cell("اسم العميل", true);
                        //Cell(d.CustomerName);
                        //Cell("");
                        //Cell("");
                        //Cell("الباقي بعد هذا الإيصال", true);
                        //Cell((d.TotalPrice - d.Deposit).ToString("N0", _ar));

                        // اسم العميل
                        //Cell("اسم العميل", true);

                        table.Cell()
                             .Border(1) // 👈 رقم أكبر = خط أسمك
                             .AlignCenter()
                             .AlignMiddle()
                             .Text("اسم العميل : ")
                             .FontSize(14)
                             .Bold();

                        table.Cell()
                            .ColumnSpan(3) // 👈 ياخد 3 أعمدة بدل 1
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.CustomerName)
                            .FontSize(14);


                        // الباقي
                        //Cell("الباقي بعد هذا الإيصال", true);

                        table.Cell()
                           .Border(1) // 👈 رقم أكبر = خط أسمك
                           .AlignCenter()
                           .AlignMiddle()
                           .Text(" الباقي بعد هذا الإيصال ")
                           .FontSize(10);

                        //Cell((d.TotalPrice - d.Deposit).ToString("N0", _ar));

                        table.Cell()
                             .Border(1)
                             .Padding(5)
                             .AlignCenter()
                             .AlignMiddle()
                             .Text((d.TotalPrice - d.Deposit).ToString("N0", _ar))
                             .FontSize(14);
                        // ================= ROW 4 =================
                        //Cell("العنوان", true);
                        table.Cell().ColumnSpan(1)
                             .Border(1) // 👈 رقم أكبر = خط أسمك
                             .AlignCenter()
                             .AlignMiddle()
                             .Text("العنوان : ")
                             .FontSize(14)
                             .Bold();



                        //Cell(d.Address);
                        table.Cell()
                            .ColumnSpan(2) // 👈 ياخد 3 أعمدة بدل 1
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.Address)
                            .FontSize(14);


                        //Cell("هاتف", true);
                        table.Cell()
                            .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 
                           .Border(1) // 👈 رقم أكبر = خط أسمك
                           .AlignCenter()
                           .AlignMiddle()
                           .Text("الهاتف:")
                           .FontSize(11)
                           .Bold();


                        //Cell(d.InstallmentAmount.ToString("N0", _ar));
                        table.Cell()
                            .ColumnSpan(2) // 👈 ياخد 3 أعمدة بدل 1
                            .Border(1)
                            .Padding(5)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(d.MobileNumber)
                            .FontSize(14);

                        // ================= ROW 5 =================

                        table.Cell()
                           .ColumnSpan(2) // 👈 ياخد 3 أعمدة بدل 
                          .Border(1) // 👈 رقم أكبر = خط أسمك
                          .AlignCenter()
                          .AlignMiddle()
                          .Text("بموجب هذا الايصال اتعهد بدفع  مبلغ:")
                          .FontSize(10)
                          .Bold();


                        table.Cell()
                          .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 1
                          .Border(1)
                          .Padding(5)
                          .AlignCenter()
                          .AlignMiddle()
                          .Text(d.Receipts[0].InstallmentAmount.ToString())
                          .FontSize(15);


                        table.Cell()
                        .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 
                       .Border(1) // 👈 رقم أكبر = خط أسمك
                       .AlignCenter()
                       .AlignMiddle()
                       .Text(" فقط و قدره :")
                       .FontSize(13)
                       .Bold();

                        table.Cell()
                      .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 1
                      .Border(1)
                      .Padding(5)
                      .AlignCenter()
                      .AlignMiddle()
                      .Text(d.Receipts[0].InstallmentAmount.ToString())
                      .FontSize(13);


                        table.Cell()
                        .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 
                       .Border(1) // 👈 رقم أكبر = خط أسمك
                       .AlignCenter()
                       .AlignMiddle()
                       .Text("جنيها لاغير")
                       .FontSize(13)
                       .Bold();

                        //Cell("تاريخ الشراء", true);
                        //Cell(d.InvoiceDate.ToString("yyyy/MM/dd"));

                        // ================= ROW 6 =================


                        //Cell("تاريخ الدفع", true);
                        table.Cell()
                       .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 
                      .Border(1) // 👈 رقم أكبر = خط أسمك
                      .AlignCenter()
                      .AlignMiddle()
                      .Text("تاريخ الدفع")
                      .FontSize(13)
                      .Bold();



                        Cell(d.InstallmentDueDate.ToString("yyyy/MM/dd"));


                        //Cell("تاريخ الشراء", true);
                        table.Cell()
                            .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 
                            .Border(1) // 👈 رقم أكبر = خط أسمك
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("تاريخ الشراء")
                            .FontSize(13)
                            .Bold();


                        Cell(d.InstallmentDueDate.ToString("yyyy/MM/dd"));

                        //Cell("نظام القسط", true);
                        table.Cell()
                            .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 
                            .Border(1) // 👈 رقم أكبر = خط أسمك
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("نظام القسط")
                            .FontSize(13)
                            .Bold();


                        //Cell(d.Plans);
                        table.Cell()
                        .ColumnSpan(1) // 👈 ياخد 3 أعمدة بدل 1
                        .Border(1)
                        .Padding(5)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text(d.AreaName)
                        .FontSize(14);

                        // ================= ROW 7 =================
                        Cell("المنتج", true);


                        table.Cell()
                     .ColumnSpan(5) // 👈 ياخد 3 أعمدة بدل 1
                     .Border(1)
                     .Padding(5)
                     .AlignCenter()
                     .AlignMiddle()
                     .Text(d.CollectorName)
                     .FontSize(12);
                        // ================= ROW 8 =================
                        Cell("المندوب", true);

                        table.Cell()
                     .ColumnSpan(2) // 👈 ياخد 3 أعمدة بدل 1
                     .Border(1)
                     .Padding(5)
                     .AlignCenter()
                     .AlignMiddle()
                     .Text(d.CollectorName)
                     .FontSize(12);
                   

                        table.Cell()
                     .ColumnSpan(3) // 👈 ياخد 3 أعمدة بدل 1
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