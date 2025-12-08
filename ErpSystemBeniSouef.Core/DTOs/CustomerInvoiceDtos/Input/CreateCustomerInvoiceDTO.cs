using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Entities;

namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input
{
    public class CreateCustomerInvoiceDTO
    {
        public int CustomerNumber { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string NationalNumber { get; set; }
        public decimal Deposit { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime FirstInvoiceDate { get; set; }
        public int SubAreaId { get; set; }

        public int CollectorId { get; set; }
        public int RepresentativeId { get; set; }
        public List<Customerinvoicedtos> customerinvoicedtos { get; set; }

        public List<Installmentsdtos> installmentsdtos { get; set; }
    }
    public class Installmentsdtos
    {
        public int NumberOfMonths { get; set; }
        public decimal Amount { get; set; }
    }
    public class Customerinvoicedtos
    {
        //public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;

    }

    public class DisplayForUiCustomerinvoicedtos
    {
        //public int InvoiceId { get; set; }
        public int DisplayId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
        public string ProductCategoryName { get; set; }
        public string ProductName { get; set; } 

    }

}
