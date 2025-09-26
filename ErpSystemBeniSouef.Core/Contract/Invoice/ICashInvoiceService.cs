using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
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
        Task<IReadOnlyList<ReturnCashInvoiceDto>> GetAllAsync();

        ReturnCashInvoiceDto AddInvoice(AddCashInvoiceDto dto);
        
        Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto);
       
        Task<InvoiceDetailsDto> GetInvoiceById(int id);
       
        Task<List<InvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

        #region Update Region
        bool Update(UpdateInvoiceDto updateDto);
 
        #endregion

        #region Soft Delete Region

        bool SoftDelete(int id);
        
        Task<bool> SoftDeleteAsync(int id);
        
        #endregion




    }
}
