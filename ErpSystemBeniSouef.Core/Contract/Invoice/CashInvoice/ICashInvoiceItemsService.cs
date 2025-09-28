using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice
{
    public interface ICashInvoiceItemsService
    {  
        #region Add Invoice Items Region

        Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto);

        #endregion
 
        #region Get Invoice Items By Invoice Id Region

        Task<List<InvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

        #endregion
         


    }
}

