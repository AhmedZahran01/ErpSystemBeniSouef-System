using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core; 
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Enum;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;

namespace ErpSystemBeniSouef.Service.InvoiceServices
{
    public class SupplierCashService : ISupplierCashService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierCashService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Core.DTOs.InvoiceDtos.Input.SupplierCashDto> AddSupplierCash(AddSupplierCashDto dto)
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                throw new Exception($"Supplier with Id {dto.SupplierId} not found.");

            var entity = new SupplierAccount
            {
                SupplierId = dto.SupplierId,
                Amount = dto.Amount,
                Description = dto.Notes,
                TransactionDate = dto.PaymentDate
            };

            _unitOfWork.Repository<SupplierAccount>().Add(entity);
            await _unitOfWork.CompleteAsync();

            return new Core.DTOs.InvoiceDtos.Input.SupplierCashDto
            {
                Id = entity.Id,
                SupplierName = supplier.Name,
                Amount = entity.Amount,
                Notes = entity.Description,
                PaymentDate = entity.TransactionDate
            };
        }
        public async Task<List<Core.DTOs.InvoiceDtos.Input.SupplierCashDto>> GetAllSupplierAccounts()
        {
            var accounts = await _unitOfWork.Repository<SupplierAccount>()
                .GetAllAsync(a => a.Supplier); 

            return accounts.Select(a => new Core.DTOs.InvoiceDtos.Input.SupplierCashDto
            {
                Id = a.Id,
                SupplierName = a.Supplier?.Name ?? "N/A",
                Amount = a.Amount,
                Notes = a.Description,
                PaymentDate = a.TransactionDate
            }).ToList();
        }

        public async Task<SupplierAccountReportDto> GetSupplierAccount(int supplierId, DateTime? startDate, DateTime? endDate)
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierId);
            if (supplier == null)
                throw new Exception($"Supplier with Id {supplierId} not found.");

            var invoices = await _unitOfWork.Repository<Invoice>()
                .GetAllAsync(i => i.SupplierId == supplierId &&
                                 (!startDate.HasValue || i.InvoiceDate >= startDate.Value) &&
                                 (!endDate.HasValue || i.InvoiceDate <= endDate.Value));

            var payments = await _unitOfWork.Repository<SupplierAccount>()
                .GetAllAsync(c => c.SupplierId == supplierId &&
                                 (!startDate.HasValue || c.TransactionDate >= startDate.Value) &&
                                 (!endDate.HasValue || c.TransactionDate <= endDate.Value));

            return new SupplierAccountReportDto
            {
                SupplierName = supplier.Name,
                Invoices = invoices.Select(i => new SupplierInvoiceDto
                {
                    Id = i.Id,
                    InvoiceDate = i.InvoiceDate,
                    TotalAmount = i.TotalAmount ?? 0,
                    DueAmount = i.DueAmount ?? 0,
                    Notes = i.Notes
                }).ToList(),
                Payments = payments.Select(p => new Core.DTOs.InvoiceDtos.Output.SupplierCashDto
                {
                    Id = p.Id,
                    PaymentDate = p.TransactionDate,
                    Amount = p.Amount,
                    Notes = p.Description
                }).ToList()
            };
        }
    }


}
