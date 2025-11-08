using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class CashCstomerInvoice:BaseEntity
    {
        public int SubAreaId { get; set; }
        public SubArea SubArea { get; set; }

      
        public int RepresentativeId { get; set; }
        public Representative Representative { get; set; }
        public DateTime InvoiceDate { get; set; }

        public decimal? TotalAmount { get; set; }

        // Navigation
        public ICollection<CashCustomerInvoiceItems> Items { get; set; }
    }
}
