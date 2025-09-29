using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.Invoice.DueInvoice
{
    public interface IDueInvoiceItemService
    {
        #region Add Invoice Items Region

        Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto);

        #endregion

        #region Get Invoice By Id Region

        Task<DueInvoiceDetailsDto> GetInvoiceById(int id);

        #endregion

        #region Get Invoice Items By Invoice Id Region

        Task<List<DueInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

        #endregion
 
    }
}
