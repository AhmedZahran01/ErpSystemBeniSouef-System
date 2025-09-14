using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Customer : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }                      
        public string MobileNumber { get; set; }              
        public string OccupationOrResidence { get; set; }     
        public DateTime SaleDate { get; set; }                
        public DateTime FirstInvoiceDate { get; set; }        
        public string NationalId { get; set; }                

        public int MainAreaId { get; set; }
        public MainArea MainArea { get; set; }

        public int SubAreaId { get; set; }
        public SubArea SubArea { get; set; }

        public string RepresentativeOrCollector { get; set; } 

        public string ProductType { get; set; }                 
        public string ProductName { get; set; }                
        public int Quantity { get; set; }                     
        public decimal Price { get; set; }
    }
}
