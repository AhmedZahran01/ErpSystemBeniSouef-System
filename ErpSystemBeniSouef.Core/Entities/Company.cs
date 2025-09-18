using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        // Navigation property for Items
        public virtual ICollection<Product>? Products { get; set; }

    }
}

