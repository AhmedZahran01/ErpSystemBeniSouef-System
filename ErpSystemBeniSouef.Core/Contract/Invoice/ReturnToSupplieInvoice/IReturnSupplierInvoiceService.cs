using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.Invoice.ReturnToSupplieInvoice
{
    public interface IReturnSupplierInvoiceService
    {
        Task<IReadOnlyList<DtoForReturnSupplierInvoice>> GetAllAsync();

        DtoForReturnSupplierInvoice AddInvoice(AddReturnSupplierInvoiceDto dto);
        Task<ReturnSupplierInvoiceDetailsDto> GetInvoiceById(int id);


        #region Update Region
        bool Update(UpdateInvoiceDto updateDto);

        #endregion

        #region Soft Delete Region

        bool SoftDelete(int id);

        Task<bool> SoftDeleteAsync(int id);

        #endregion



    }
}
