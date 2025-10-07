using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.SupplierCash;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.supplierCashService
{
    public class SupplierCashService : ISupplierCashService
    {

        #region Constractor Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierCashService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region GetAllSupplierAccounts Region

        public async Task<List<ReturnSupplierCashDto>> GetAllSupplierAccounts()
        {
            var accounts = await _unitOfWork.Repository<SupplierAccount>()
                .GetAllAsync(a => a.Supplier);

            return accounts.Select(a => new ReturnSupplierCashDto
            {
                Id = a.Id,
                SupplierName = a.Supplier?.Name ?? "N/A",
                Amount = a.Amount,
                Notes = a.Notes,
                PaymentDate = a.TransactionDate,
                SupplierId = a.SupplierId
            }).ToList();
        }


        #endregion

        #region Add Supplier Cash  Region

        public async Task<ReturnSupplierCashDto> AddSupplierCash(AddSupplierCashDto dto)
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                throw new Exception($"Supplier with Id {dto.SupplierId} not found.");

            var entity = new SupplierAccount
            {
                SupplierId = dto.SupplierId,
                Amount = dto.Amount,
                Notes = dto.Notes,
                TransactionDate = dto.PaymentDate
            };

            _unitOfWork.Repository<SupplierAccount>().Add(entity);
            await _unitOfWork.CompleteAsync();

            return new ReturnSupplierCashDto
            {
                Id = entity.Id,
                SupplierName = supplier.Name,
                Amount = entity.Amount,
                Notes = entity.Notes,
                PaymentDate = entity.TransactionDate
            };
        }

        #endregion
         
        #region Soft Delete Invoice Region

        public bool SoftDelete(int SupplierAccountid )
        {
            var invoice =  _unitOfWork.Repository<SupplierAccount>().GetById(SupplierAccountid);
            if (invoice == null)
                return false;
            try
            {
                invoice.IsDeleted = true;
                _unitOfWork.Repository<SupplierAccount>().Update(invoice);
                 _unitOfWork.Complete();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SoftDeleteAsync(int SupplierAccountid)
        {
            var invoice = await _unitOfWork.Repository<SupplierAccount>().GetByIdAsync(SupplierAccountid);
            if (invoice == null)
                return false;
            try
            {
                invoice.IsDeleted = true;
                _unitOfWork.Repository<SupplierAccount>().Update(invoice);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion
         
        #region Update Invoice Dto Region

        public bool Update(UpdateSupplierCashDto updateDto)
        {
            var supplierAccount = _unitOfWork.Repository<SupplierAccount>().GetById(updateDto.Id);
            if (supplierAccount == null)
                return false;

            if (supplierAccount.SupplierId != updateDto.SupplierId)
            {
                var supplier = _unitOfWork.Repository<Supplier>().GetById(updateDto.SupplierId);
                if (supplier == null)
                    return false;
            }

            _mapper.Map(updateDto, supplierAccount);
            supplierAccount.UpdatedDate = DateTime.UtcNow; 

            _unitOfWork.Repository<SupplierAccount>().Update(supplierAccount);
            _unitOfWork.CompleteAsync();
            return true;
        }

        #endregion
         
    }
}
