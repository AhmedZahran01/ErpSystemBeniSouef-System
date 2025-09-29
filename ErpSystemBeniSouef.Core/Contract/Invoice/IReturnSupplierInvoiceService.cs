using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;

namespace ErpSystemBeniSouef.Core.Contract.Invoice
{
    public interface IReturnSupplierInvoiceService
    {
        Task<IReadOnlyList<DtoForReturnSupplierInvoice>> GetAllAsync();
        DtoForReturnSupplierInvoice AddInvoice(AddReturnSupplierInvoiceDto dto);
        Task<bool> AddInvoiceItems(AddReturnSupplierInvoiceItemsDto dto);
        Task<ReturnSupplierInvoiceDetailsDto> GetInvoiceById(int id);
        Task<List<ReturnSupplierInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

    }
}
