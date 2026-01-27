using ErpSystemBeniSouef.Core.DTOs.Receipt;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ErpSystemBeniSouef.ReceiptPdfDocumentFolder
{
    public class ReceiptPdfDocument : IDocument
    {
        private readonly GetAllReceiptsDto _data;

        public ReceiptPdfDocument(GetAllReceiptsDto data)
        {
            _data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(15);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Content().Column(col =>
                {
                    col.Item().AlignCenter().Text("شركة مكة").Bold().FontSize(14);
                    col.Item().AlignCenter().Text("إيصال استلام");

                    col.Item().LineHorizontal(1);

                    col.Item().Text($"رقم العميل: {_data.CustomerNumber}");
                    col.Item().Text($"اسم العميل: {_data.CustomerName}");
                    col.Item().Text($"المنطقة: {_data.AreaName}");
                    col.Item().Text($"العنوان: {_data.Address}");
                    col.Item().Text($"الهاتف: {_data.MobileNumber}");

                    col.Item().LineHorizontal(1);

                    col.Item().Text($"تاريخ الفاتورة: {_data.InvoiceDate:yyyy/MM/dd}");
                    col.Item().Text($"أول قسط: {_data.FirstInvoiceDate:yyyy/MM/dd}");

                    col.Item().LineHorizontal(1);

                    col.Item().Text($"الإجمالي: {_data.TotalPrice:N2}");
                    col.Item().Text($"المقدم: {_data.Deposite:N2}");
                    col.Item().Text($"نظام القسط: {_data.Plans}");

                    col.Item().LineHorizontal(1);

                    col.Item().Text("الأصناف:");
                    col.Item().Text(_data.Items);

                    col.Item().PaddingTop(10);
                    col.Item().AlignCenter().Text("التوقيع: ______________");
                });
            });
        }
    }


}
