using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.Invoice.DueInvoice
{
    public interface IDueInvoiceService
    {
        #region Get All Cash Invoice Dto Async Region

        Task<IReadOnlyList<DueInvoiceDetailsDto>> GetAllAsync();

        #endregion

        #region Add Due Invoice Region

        Task<DueInvoiceDetailsDto> AddDueInvoice(AddDueInvoiceDto dto);

        #endregion

        #region Soft Delete Region

        bool SoftDelete(int id);

        Task<bool> SoftDeleteAsync(int id);

        #endregion

        #region Update Region

        bool Update(UpdateDueInvoiceDto updateDto);

        #endregion

        

    }
}
