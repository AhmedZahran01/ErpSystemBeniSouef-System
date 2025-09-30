using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Representative : BaseEntity
    {
        public string Name { get; set; }              
        public int UserNumber { get; set; }          
        public string Password { get; set; }
        public ICollection<Covenant> Covenants { get; set; }
    }
}
