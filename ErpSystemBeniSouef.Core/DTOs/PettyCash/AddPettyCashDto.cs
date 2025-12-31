using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.PettyCash
{
    public class AddPettyCashDto
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
    }

    public class ReturnPettyCashDto
    {
        public int Id { get; set; }
        public int DipslayUIId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
    }
}
