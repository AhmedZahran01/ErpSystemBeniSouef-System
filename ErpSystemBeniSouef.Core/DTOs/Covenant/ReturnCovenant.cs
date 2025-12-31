using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.Covenant
{
    public class ReturnCovenant
    {
        public int DisplayUIId { get; set; }
        public int Id { get; set; }
        public string CovenantType { get; set; }
        public DateTime CovenantDate { get; set; }

    }
    public class ReturnCovenantItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }   
        public int CategoryId { get; set; }
        public int Amount { get; set; }
    }
}
