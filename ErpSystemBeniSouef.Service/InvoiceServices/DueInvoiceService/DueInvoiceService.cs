using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.DueInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.InvoiceServices.DueInvoiceService
{
    public class DueInvoiceService : IDueInvoiceService
    {

        #region Constractor Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DueInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Get All Async Due Invoice Dto Region
        public async Task<IReadOnlyList<DueInvoiceDetailsDto>> GetAllAsync()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsync(i => i.Supplier);
            var CahInvoice = invoices.Where(I => I.invoiceType == InvoiceType.Due).ToList();

            var response = _mapper.Map<IReadOnlyList<DueInvoiceDetailsDto>>(CahInvoice);

            return response;
        }

        #endregion

        #region Soft Delete Invoice Region

        public bool SoftDelete(int id)
        {
            var product = _unitOfWork.Repository<Invoice>().GetById(id);
            if (product == null)
                return false;
            product.IsDeleted = true;
            _unitOfWork.Repository<Invoice>().Update(product);
            _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);
            if (invoice == null)
                return false;
            try
            {
                invoice.IsDeleted = true;
                _unitOfWork.Repository<Invoice>().Update(invoice);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region Add Due Invoice Region

        public async Task<DueInvoiceDetailsDto> AddDueInvoice(AddDueInvoiceDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                throw new Exception($"Supplier with Id {dto.SupplierId} not found.");

            invoice.SupplierId = dto.SupplierId;
            invoice.Supplier = supplier;
            invoice.invoiceType = InvoiceType.Due;
            invoice.TotalAmount = 0;
            invoice.DueAmount = dto.DueAmount;
            invoice.CreatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Invoice>().Add(invoice);
            await _unitOfWork.CompleteAsync();

            return new DueInvoiceDetailsDto
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount ?? 0,
                SupplierName = supplier.Name,
                DueAmount = invoice.DueAmount
            };
        }

        #endregion

        #region Update Invoice Dto Region

        public bool Update(UpdateDueInvoiceDto updateDto)
        {
            var invoice = _unitOfWork.Repository<Invoice>().GetById(updateDto.Id);
            if (invoice == null)
                return false;

            if (invoice.SupplierId != updateDto.SupplierId)
            {
                var supplier = _unitOfWork.Repository<Supplier>().GetById(updateDto.SupplierId);
                if (supplier == null)
                    return false;
            }

            _mapper.Map(updateDto, invoice);
            invoice.UpdatedDate = DateTime.UtcNow;
            invoice.DueAmount = updateDto.DueAmount;

            _unitOfWork.Repository<Invoice>().Update(invoice);
            _unitOfWork.CompleteAsync();
            return true;
        }

        #endregion

    }
}
