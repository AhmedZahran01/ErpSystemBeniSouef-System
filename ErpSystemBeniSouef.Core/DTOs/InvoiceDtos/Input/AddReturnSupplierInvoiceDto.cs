using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input
{
    public class AddReturnSupplierInvoiceDto
    {
        public DateTime InvoiceDate { get; set; }
        public int SupplierId { get; set; }
    }
    public class AddReturnSupplierInvoiceItemsDto
    {
        public int Id { get; set; }
        public List<ReturnSupplierInvoiceItemDto> invoiceItemDtos { get; set; }
    }
    public class ReturnSupplierInvoiceItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }

    }
}
