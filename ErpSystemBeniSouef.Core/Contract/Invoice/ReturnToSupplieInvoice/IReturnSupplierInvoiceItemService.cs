using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.Invoice.ReturnToSupplieInvoice
{
    public interface IReturnSupplierInvoiceItemService
    {
        #region Get Invoice Items By Invoice Id Region

        Task<List<ReturnSupplierInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

        #endregion


        #region Add Invoice Items Region

        Task<bool> AddInvoiceItems(AddReturnSupplierInvoiceItemsDto dto);

        #endregion

      
        #region Soft Delete Region

        bool SoftDelete(int id, decimal totalLine, int invoiceId);

        Task<bool> SoftDeleteAsync(int id);

        #endregion


        #region  Comment mostafa  Region
     
        //#region add Region
        //Task<bool> AddInvoiceItems(AddReturnSupplierInvoiceItemsDto dto);

        //#endregion

        //#region Get Invoice Items By InvoiceId Region
        //Task<List<ReturnSupplierInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId);

        //#endregion

        //#region Soft Delete Region

        //bool SoftDelete(int id, decimal totalLine, int invoiceId);

        //Task<bool> SoftDeleteAsync(int id);

        //#endregion 
        #endregion

    }
}
