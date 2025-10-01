using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos
{
    public class DtoForReturnSupplierInvoice
    
    {
        public int Id { get; set; }
        public int DisplayId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string SupplierName { get; set; }
         
        public int SupplierId { get; set; }
        public SupplierRDto Supplier { get; set; } 

    }
 
}
