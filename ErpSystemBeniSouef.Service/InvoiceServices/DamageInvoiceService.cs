using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Enum;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DamageInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DamageDtos;

namespace ErpSystemBeniSouef.Service.InvoiceServices
{
    public class DamageInvoiceService : IDamageInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DamageInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReturnDamageInvoiceDto> AddInvoice(AddDamageInvoiceDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            //var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(dto.SupplierId);
            //if (supplier == null)
            //    throw new Exception($"Supplier with Id {dto.SupplierId} not found.");

            //invoice.SupplierId = dto.SupplierId;
           // invoice.Supplier = supplier;
            invoice.invoiceType = InvoiceType.Damage;
            invoice.TotalAmount = 0;
            invoice.CreatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Invoice>().Add(invoice);
            await _unitOfWork.CompleteAsync();

            return new ReturnDamageInvoiceDto
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount ?? 0,
                SupplierName ="" //supplier.Name
            };
        }

        public async Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto)
        {
            var invoice = await _unitOfWork.Repository<Invoice>()
                .FindWithIncludesAsync(i => i.Id == dto.Id && i.invoiceType == InvoiceType.Damage, i => i.Supplier);

            if (invoice == null)
                throw new Exception($"Damage Invoice with Id {dto.Id} not found.");

            decimal totalAmount = invoice.TotalAmount ?? 0;

            foreach (var itemDto in dto.invoiceItemDtos)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product with Id {itemDto.ProductId} not found.");

                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    ProductType = product.Category?.Name ?? "N/A",
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    Notes = itemDto.Note
                };

                invoice.Items ??= new List<InvoiceItem>();
                invoice.Items.Add(invoiceItem);

                totalAmount += (itemDto.Quantity * itemDto.UnitPrice);
            }

            invoice.TotalAmount = totalAmount;

            _unitOfWork.Repository<Invoice>().Update(invoice);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<DamageInvoiceDetailsDto> GetInvoiceById(int id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>()
                .FindWithIncludesAsync(i => i.Id == id && i.invoiceType == InvoiceType.Damage && !i.IsDeleted, i => i.Supplier);

            if (invoice == null)
                throw new Exception($"Damage Invoice with Id {id} not found.");

            return new DamageInvoiceDetailsDto
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount ?? 0,
                SupplierName = invoice.Supplier?.Name ?? "N/A",
                Notes = invoice.Notes
            };
        }

        public async Task<List<DamageInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId)
        {
            var items = await _unitOfWork.Repository<InvoiceItem>().GetAllAsync(i => i.InvoiceId == invoiceId);

            return items.Select(i => new DamageInvoiceItemDetailsDto
            {
                Id = i.Id,
                InvoiceId = i.InvoiceId,
                ProductName = i.ProductName,
                ProductType = i.ProductType,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Notes = i.Notes
            }).ToList();
        }
    }
}

