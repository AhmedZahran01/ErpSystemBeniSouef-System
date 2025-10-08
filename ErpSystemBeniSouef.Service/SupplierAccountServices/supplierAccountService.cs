using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.SupplierAccountServices
{
    public class supplierAccountService : ISupplierAccountService
    {

        #region Constractor Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public supplierAccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion
         
        #region  Get Supplier Account Region

        public async Task<SupplierAccountReportDto> GetSupplierAccount(int supplierId, DateTime? startDate, DateTime? endDate)
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierId);
            if (supplier == null)
                throw new Exception($"Supplier with Id {supplierId} not found.");

            var invoices = await _unitOfWork.Repository<Invoice>()
                .GetAllAsync();

            invoices = invoices.Where(i => i.SupplierId == supplierId &&
                                 (!startDate.HasValue || i.InvoiceDate >= startDate.Value) &&
                                 (!endDate.HasValue || i.InvoiceDate <= endDate.Value)).ToList();

            var payments = await _unitOfWork.Repository<SupplierAccount>() .GetAllAsync();
            var paymentsNew =  payments.Where(c => c.SupplierId == supplierId &&
                                 (!startDate.HasValue || c.TransactionDate >= startDate.Value) &&
                                 (!endDate.HasValue || c.TransactionDate <= endDate.Value)).ToList();

            return new SupplierAccountReportDto
            {
                SupplierName = supplier.Name,
                Invoices = invoices.Select(i => new SupplierInvoiceDto
                {
                    Id = i.Id,
                    InvoiceDate = i.InvoiceDate,
                    TotalAmount = i.TotalAmount ?? 0,
                    DueAmount = i.DueAmount ?? 0,
                    Notes = i.Notes,
                    invoiceType = i.invoiceType
                }).ToList(),
                Payments = paymentsNew.Select(p => new SupplierCashDto
                {
                    Id = p.Id,
                    PaymentDate = p.TransactionDate,
                    Amount = p.Amount,
                    Notes = p.Notes
                }).ToList()
            };
        }

        #endregion

    }
}
