using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.Invoice
{
    public interface ICashInvoiceService
    {
        Task<ReturnInvoiceDto> AddInvoice(AddInvoiceDto dto);
        Task<bool> AddInvoiceItems(AddInvoiceItemsDto dto);
        Task<IReadOnlyList<ReturnInvoiceDto>> GetAllAsync();

    }
}
