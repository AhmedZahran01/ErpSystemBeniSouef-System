using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input
{
    public class UpdateCustomerInvoiceDTO
    {
       
        public int? CustomerNumber { get; set; }
        public string? Name { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? NationalNumber { get; set; }
        public decimal? Deposit { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime? FirstInvoiceDate { get; set; }
        public int? SubAreaId { get; set; }
        public int? CollectorId { get; set; }
        public int? RepresentativeId { get; set; }

        public List<UpdateInvoiceItemDTO>? UpdatedItems { get; set; }

        public List<UpdateInstallmentDTO>? UpdatedInstallments { get; set; }
    }

    public class UpdateInvoiceItemDTO
    {
        public int Id { get; set; } 
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateInstallmentDTO
    {
        public int NumberOfMonths { get; set; }
        public decimal Amount { get; set; }
    }
}
