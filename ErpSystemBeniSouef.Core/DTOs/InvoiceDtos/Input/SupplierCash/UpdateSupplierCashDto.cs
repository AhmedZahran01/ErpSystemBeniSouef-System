using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.SupplierCash
{
    public class UpdateSupplierCashDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int SupplierId { get; set; }
        public string? Notes { get; set; }
          

    }
}
