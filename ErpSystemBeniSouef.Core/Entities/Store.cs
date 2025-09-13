using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        //public string ManagerName { get; set; }
        //public string ContactNumber { get; set; }
        //public string Email { get; set; }
        //public DateTime EstablishedDate { get; set; }
        //public string Description { get; set; }

        // Navigation property for related Categories
        public ICollection<Category> Categories { get; set; }
    }
}
