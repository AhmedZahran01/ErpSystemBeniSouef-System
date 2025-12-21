using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.GetAllDetailsForCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.CustomerInvoice
{
    public interface ICustomerInvoiceService
    {
        Task<ServiceResponse<bool>> CreateCustomerInvoiceAsync(CreateCustomerInvoiceDTO dto);
        Task<ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>> GetAllCustomerInvoicesAsync();
        Task<ServiceResponse<ReturnCustomerInvoiceDetailsDTO>> GetCustomerInvoiceByIdAsync(int invoiceId);
        Task<ServiceResponse<bool>> DeleteCustomerAsync(int customerId);
        Task<ServiceResponse<bool>> UpdateCustomerInvoiceAsync(int invoiceId, UpdateCustomerInvoiceDTO dto);

    }
}
