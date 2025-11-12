using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.CreateCashCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.ReturnAllCashCustomerInvoices;
using ErpSystemBeniSouef.Core.GenericResponseModel;

namespace ErpSystemBeniSouef.Core.Contract.CashCustomerInvoiceServices
{
    public interface ICashCustomerInvoiceService
    {
        Task<ServiceResponse<bool>> AddCashCustomerInvoice(CreateCashCustomerInvoiceDTO dto);
        Task<ServiceResponse<List<ReturnAllCashCustomerInvoicesDTO>>> GetAllCashCustomerInvoices();
        Task<ServiceResponse<bool>> DeleteCashCustomerInvoiceAsync(int invoiceId);
    }
}
