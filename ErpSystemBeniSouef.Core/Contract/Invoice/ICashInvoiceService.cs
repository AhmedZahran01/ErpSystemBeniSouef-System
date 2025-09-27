using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
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
        #region Get All Cash Invoice Dto Async Region

        Task<IReadOnlyList<ReturnCashInvoiceDto>> GetAllAsync();

        #endregion

        #region Add Cash Invoice Dto Region

        ReturnCashInvoiceDto AddInvoice(AddCashInvoiceDto dto);

        #endregion

        #region Add Invoice Items Region

        Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto);

        #endregion

        #region Get Invoice By Id Region

        Task<InvoiceDetailsDto> GetInvoiceById(int id);

        #endregion

        #region Get Invoice Items By Invoice Id Region

        Task<List<InvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

        #endregion
         
        #region Update Region

        bool Update(UpdateInvoiceDto updateDto);
 
        #endregion

        #region Soft Delete Region

        bool SoftDelete(int id);
        
        Task<bool> SoftDeleteAsync(int id);
        
        #endregion




    }
}
