using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.Reports.MonthlyCollectingDtos
{
    public class SubmitMonthlyCollectionRequestDto
    {
        public int CollectorId { get; set; }
        public DateTime Month { get; set; }     
        
    }

    public class MonthlyCollectionItemDto
    {
        public int InstallmentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string WorkPlace { get; set; }
        public string Area { get; set; }
        public decimal InstallmentAmount { get; set; }
    }

    public class MonthlyCollectionResponseDto
    {
        public int MonthlyCollectionId { get; set; }
        public int CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string MonthName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<MonthlyCollectionItemDto> Items { get; set; }
    }
}
