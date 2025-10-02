using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface ISupplierAccountService
    {
        Task<SupplierAccountReportDto> GetSupplierAccount(int supplierId, DateTime? startDate, DateTime? endDate);

    }
}
