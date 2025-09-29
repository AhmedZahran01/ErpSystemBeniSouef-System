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
    public interface ICashInvoiceService
    {
        #region Get All Cash Invoice Dto Async Region

        Task<IReadOnlyList<ReturnCashInvoiceDto>> GetAllAsync();

        #endregion

        #region Add Cash Invoice Dto Region

        ReturnCashInvoiceDto AddInvoice(AddCashInvoiceDto dto);

        #endregion
         
        #region Get Invoice By Id Region

        Task<CashInvoiceDetailsDto> GetInvoiceById(int id);

        #endregion
         
        #region Update Region

        bool Update(UpdateCashInvoiceDto updateDto);

        #endregion

        #region Soft Delete Region

        bool SoftDelete(int id);

        Task<bool> SoftDeleteAsync(int id);

        #endregion




    }
}
