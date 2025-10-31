using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.GenericResponseModel;

namespace ErpSystemBeniSouef.Core.Contract.CustomerInvoice
{
    public interface ICustomerInvoiceService
    {
        Task<ServiceResponse<bool>> CreateCustomerInvoiceAsync(CreateCustomerInvoiceDTO dto);
        Task<ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>> GetAllCustomerInvoicesAsync();
        Task<ServiceResponse<bool>> DeleteCustomerAsync(int customerId);
    }
}
