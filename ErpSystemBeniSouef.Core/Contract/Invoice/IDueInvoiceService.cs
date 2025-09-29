using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos;

namespace ErpSystemBeniSouef.Core.Contract.Invoice
{
    public interface IDueInvoiceService
    {
        Task<DueInvoiceDto> AddInvoice(AddDueInvoiceDto dto);
        Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto); 
        Task<DueInvoiceDetailsDto> GetInvoiceById(int id);
        Task<List<DueInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);
    }
}
