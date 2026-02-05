using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.RepresentativeWithdrawalDtos
{
    public class AddRepresentativeWithdrawalDto
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }

        public int representativeId { get; set; }
    }

    public class ReturnRepresentativeWithdrawalDto
    {
        public int Id { get; set; }
        public int DisplayUIId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }

        public int representativeId { get; set; }
        public Representative representative { get; set; }
        public string representativeName { get; set; }

    }
}
