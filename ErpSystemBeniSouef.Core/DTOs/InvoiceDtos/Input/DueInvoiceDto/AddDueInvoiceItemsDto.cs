using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto
{
    public class AddDueInvoiceItemsDto
    {
        public int Id { get; set; }
        public decimal InvoiceTotalPrice { get; set; } = 0;
        public List<DueInvoiceItemDto> invoiceItemDtos { get; set; }
 
    }
}
