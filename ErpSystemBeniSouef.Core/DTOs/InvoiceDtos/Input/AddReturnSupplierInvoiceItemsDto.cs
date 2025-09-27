using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input
{
    public class AddReturnSupplierInvoiceItemsDto
    {
        public int Id { get; set; }
        public List<ReturnSupplierInvoiceItemDto> invoiceItemDtos { get; set; }
    }
}
