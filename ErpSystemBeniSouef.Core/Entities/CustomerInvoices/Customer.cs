using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class Customer : BaseEntity
    {
        public int CustomerNumber { get; set; }
        public string Name { get; set; }                      
        public string MobileNumber { get; set; }              
        public string Address { get; set; }     
        public DateTime SaleDate { get; set; }                
        public DateTime FirstInvoiceDate { get; set; }        
        public string NationalNumber { get; set; }
        public decimal Deposit { get; set; }


        public int SubAreaId { get; set; }
        public SubArea SubArea { get; set; }

        public int CollectorId { get; set; }
        public Collector Collector { get; set; }
        public int RepresentativeId { get; set; } 
        public Representative Representative { get; set; }


        public ICollection<CustomerInvoice> Invoices { get; set; }
        
    }
}
