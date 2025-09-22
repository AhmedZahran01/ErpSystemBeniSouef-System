using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input
{
    public class AddCashInvoiceItemsDto
    {
        public int Id { get; set; }
        public List<CashInvoiceItemDto> invoiceItemDtos { get; set; }
    }
    public class CashInvoiceItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int TotalAmount { get; set; }
        public string Note { get; set; }

    }
}
