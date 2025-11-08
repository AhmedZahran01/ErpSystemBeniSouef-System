using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class CashCustomerInvoiceItems:BaseEntity
    {
       
        public int? CashCustomerInvoiceId { get; set; }
        public CashCstomerInvoice? cashCstomerInvoice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }
}
