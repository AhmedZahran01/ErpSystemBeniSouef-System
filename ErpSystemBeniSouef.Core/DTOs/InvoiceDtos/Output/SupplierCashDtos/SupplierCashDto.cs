using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos
{
    public class SupplierCashDto
    {
        public int Id { get; set; }
        public int DisplayId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
