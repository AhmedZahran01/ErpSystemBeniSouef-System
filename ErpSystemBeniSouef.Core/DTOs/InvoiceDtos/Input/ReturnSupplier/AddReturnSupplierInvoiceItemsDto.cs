using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier
{
    public class AddReturnSupplierInvoiceItemsDto
    {
        public int Id { get; set; }
        public decimal InvoiceTotalPrice { get; set; }
        public List<ReturnSupplierInvoiceItemDto> invoiceItemDtos { get; set; }
    }
}
