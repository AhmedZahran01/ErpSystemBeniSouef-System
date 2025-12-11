using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;

namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.CreateCashCustomerInvoiceDtos
{
    public class CreateCashCustomerInvoiceDTO
    {
        public int SubAreaId { get; set; }
        public int RepresentativeId { get; set; }
        public DateTime SaleDate { get; set; }
        public List<Cashcustomerinvoicedtos>? cashcustomerinvoicedtos { get; set; }
    }
    public class Cashcustomerinvoicedtos
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;

    }
}
