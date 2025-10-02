using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.SupplierCash;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;

namespace ErpSystemBeniSouef.Core.Contract.Invoice
{
    public interface ISupplierCashService
    { 
        #region GetAllSupplierAccounts Region

        Task<List<ReturnSupplierCashDto>> GetAllSupplierAccounts();

        #endregion

        #region AddSupplierCash Region

        Task<ReturnSupplierCashDto> AddSupplierCash(AddSupplierCashDto dto);

        #endregion

        #region Soft Delete Region

        bool SoftDelete(int id);

        Task<bool> SoftDeleteAsync(int id);

        #endregion


        #region Update Region

        bool Update(UpdateSupplierCashDto updateDto);

        #endregion




    }
}
