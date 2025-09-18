using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }
        
        //public int CompanyId { get; set; }
        //public Company Company { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
        // Navigation property for Items
        public virtual ICollection<Product>? Products { get; set; }
    }
}
