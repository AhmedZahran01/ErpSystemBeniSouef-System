using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto
{
    public class AddCashInvoiceDto
    {
        public DateTime? InvoiceDate { get; set; }
        public int SupplierId { get; set; }

    }
}
