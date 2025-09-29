using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }  
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
       
        
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public ICollection<CovenantProduct> CovenantProducts { get; set; }
    }
}
