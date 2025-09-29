using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DamageInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DamageDtos;

namespace ErpSystemBeniSouef.Core.Contract.Invoice
{
    public interface IDamageInvoiceService
    {
        Task<ReturnDamageInvoiceDto> AddInvoice(AddDamageInvoiceDto dto);
        Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto);
        Task<DamageInvoiceDetailsDto> GetInvoiceById(int id);
        Task<List<DamageInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);
    }
}
