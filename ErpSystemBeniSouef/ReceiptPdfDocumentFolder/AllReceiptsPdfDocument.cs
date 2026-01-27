using ErpSystemBeniSouef.Core.DTOs.Receipt;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ErpSystemBeniSouef.ReceiptPdfDocumentFolder
{
    public class AllReceiptsPdfDocument : IDocument
    {

        private readonly List<GetAllReceiptsDto> _receipts;

        public AllReceiptsPdfDocument(List<GetAllReceiptsDto> receipts)
        {
            _receipts = receipts;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(10);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Content().Column(col =>
                {
                    foreach (var receipt in _receipts)
                    {
                        col.Item().Element(c => ReceiptTemplate(c, receipt));
                        col.Item().PageBreak(); // كل إيصال في صفحة
                    }
                });
            });
        }

        private void ReceiptTemplate(IContainer container, GetAllReceiptsDto d)
        {
            container
                .Border(1)
                .Padding(5)
                .Column(col =>
                {
                    // Header
                    col.Item().AlignCenter().Text("شركة مكة").Bold().FontSize(14);
                    col.Item().AlignCenter().Text("إيصال").Bold();

                    col.Item().PaddingVertical(5).LineHorizontal(1);

                    // جدول رئيسي زي الورقة
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(); // قيمة
                            columns.RelativeColumn(); // عنوان
                            columns.RelativeColumn(); // قيمة
                            columns.RelativeColumn(); // عنوان
                        });

                        void Cell(string text, bool bold = false)
                        {
                            table.Cell()
                                .Border(1)
                                .Padding(3)
                                .AlignRight()
                                .Text(t =>
                                {
                                    if (bold) t.Span(text).Bold();
                                    else t.Span(text);
                                });
                        }

                        // الصف 1
                        Cell(d.CustomerNumber.ToString());
                        Cell("رقم العميل", true);
                        Cell(d.AreaName);
                        Cell("المنطقة", true);

                        // الصف 2
                        Cell(d.CustomerName);
                        Cell("اسم العميل", true);
                        Cell(d.Address);
                        Cell("العنوان", true);

                        // الصف 3
                        Cell(d.MobileNumber);
                        Cell("هاتف", true);
                        Cell(d.TotalPrice.ToString("N2"));
                        Cell("الإجمالي", true);

                        // الصف 4
                        Cell(d.Deposite.ToString("N2"));
                        Cell("المقدم", true);
                        Cell(d.Plans);
                        Cell("نظام القسط", true);

                        // الصف 5
                        Cell(d.InvoiceDate.ToString("yyyy/MM/dd"));
                        Cell("تاريخ البيع", true);
                        Cell(d.FirstInvoiceDate.ToString("yyyy/MM/dd"));
                        Cell("تاريخ أول قسط", true);
                    });

                    col.Item().PaddingTop(5).LineHorizontal(1);

                    // الأصناف
                    col.Item()
                       .Border(1)
                       .Padding(5)
                       .AlignRight()
                       .Text($"الأصناف: {d.Items}");

                    col.Item().PaddingTop(10);

                    // توقيع
                    col.Item()
                       .BorderTop(1)
                       .PaddingTop(10)
                       .AlignRight()
                       .Text("التوقيع: __________________");
                });
        }


        //private void ReceiptTemplate(IContainer container, GetAllReceiptsDto d)
        //{
        //    container.Border(1).Padding(5).Column(col =>
        //    {
        //        col.Item().AlignCenter().Text("شركة مكة").Bold().FontSize(12);

        //        col.Item().Table(table =>
        //        {
        //            table.ColumnsDefinition(columns =>
        //            {
        //                columns.RelativeColumn();
        //                columns.RelativeColumn();
        //                columns.RelativeColumn();
        //                columns.RelativeColumn();
        //            });

        //            // الصف الأول
        //            table.Cell().Text("رقم العميل");
        //            table.Cell().Text(d.CustomerNumber.ToString());
        //            table.Cell().Text("المنطقة");
        //            table.Cell().Text(d.AreaName);

        //            // الصف الثاني
        //            table.Cell().Text("اسم العميل");
        //            table.Cell().ColumnSpan(3).Text(d.CustomerName);

        //            // الصف الثالث
        //            table.Cell().Text("العنوان");
        //            table.Cell().ColumnSpan(3).Text(d.Address);

        //            // الصف الرابع
        //            table.Cell().Text("الهاتف");
        //            table.Cell().Text(d.MobileNumber);
        //            table.Cell().Text("الإجمالي");
        //            table.Cell().Text(d.TotalPrice.ToString("N2"));

        //            // الصف الخامس
        //            table.Cell().Text("المقدم");
        //            table.Cell().Text(d.Deposite.ToString("N2"));
        //            table.Cell().Text("نظام القسط");
        //            table.Cell().Text(d.Plans);

        //            // الصف السادس
        //            table.Cell().Text("تاريخ البيع");
        //            table.Cell().Text(d.InvoiceDate.ToString("yyyy/MM/dd"));
        //            table.Cell().Text("أول قسط");
        //            table.Cell().Text(d.FirstInvoiceDate.ToString("yyyy/MM/dd"));
        //        });

        //        col.Item().PaddingTop(5).Text("التوقيع: __________________");
        //    });
        //}

    }
}
