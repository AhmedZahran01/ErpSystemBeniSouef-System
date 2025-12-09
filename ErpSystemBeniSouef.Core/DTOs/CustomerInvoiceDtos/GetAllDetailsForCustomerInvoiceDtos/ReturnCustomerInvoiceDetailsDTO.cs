using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.GetAllDetailsForCustomerInvoiceDtos
{
    public class ReturnCustomerInvoiceDetailsDTO
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
        public List<InstallmentDTO> Installments { get; set; }
        public List<CustomerInvoiceItemsDTO> CustomerInvoiceItems { get; set; }

    }
    public class InstallmentDTO
    {
        public int NumberOfMonths { get; set; }
        public decimal Amount { get; set; }
    }
    public class CustomerInvoiceItemsDTO
    {
        public int ProductIdDto { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }


}
