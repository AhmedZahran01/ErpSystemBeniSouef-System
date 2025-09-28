using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;

namespace ErpSystemBeniSouef.Core.Contract.Invoice
{
    public interface ISupplierCashService
    {
        Task<DTOs.InvoiceDtos.Input.SupplierCashDto> AddSupplierCash(AddSupplierCashDto dto);

        Task<SupplierAccountReportDto> GetSupplierAccount(int supplierId, DateTime? startDate, DateTime? endDate);
    }
}
