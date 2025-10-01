using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier
{
    public class AddReturnSupplierInvoiceDto
    {
        public DateTime? InvoiceDate { get; set; }
        public int SupplierId { get; set; }
    }
    
}
