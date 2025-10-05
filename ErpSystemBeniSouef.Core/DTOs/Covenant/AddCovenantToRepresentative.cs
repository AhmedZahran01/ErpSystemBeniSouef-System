using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.Covenant
{
    public class AddCovenantToRepresentative
    {
        public DateTime CovenantDate { get; set; }
        public DateTime MonthDate { get; set; }
        public int RepresentativeId { get; set; }
        public string CovenantType { get; set; }

    }
   
}
