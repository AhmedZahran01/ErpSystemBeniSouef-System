using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Enum;

namespace ErpSystemBeniSouef.Service.InvoiceServices
{
    public class ReturnSupplierInvoiceService : IReturnSupplierInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReturnSupplierInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public DtoForReturnSupplierInvoice AddInvoice(AddReturnSupplierInvoiceDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            // Make sure supplier exists
            var supplier = _unitOfWork.Repository<Supplier>().GetById(dto.SupplierId);
            if (supplier == null)
                return null;

            invoice.SupplierId = dto.SupplierId;
            invoice.Supplier = supplier;
            invoice.invoiceType = InvoiceType.SupplierReturn;
            invoice.TotalAmount = 0;
            invoice.CreatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Invoice>().Add(invoice);
            _unitOfWork.Complete();

            // Map Entity → Return DTO
            var returnDto = new DtoForReturnSupplierInvoice
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = (decimal)invoice.TotalAmount,
                SupplierName = supplier.Name
            };

            return returnDto;
        }
        

        public async Task<bool> AddInvoiceItems(AddReturnSupplierInvoiceItemsDto dto)
        {
            Invoice invoice = new Invoice();
            try
            {
                invoice = await _unitOfWork.Repository<Invoice>()
              .FindWithIncludesAsync(i => i.Id == dto.Id && i.invoiceType == InvoiceType.SupplierReturn, i => i.Supplier);

            }
            catch (Exception ex)
            {

            }

            if (invoice == null)
                return false;

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
                    UnitPrice = itemDto.UnitPrice
                };

                if (invoice.Items == null)
                    invoice.Items = new List<InvoiceItem>();
                invoice.Items.Add(invoiceItem);

                totalAmount += (itemDto.Quantity * itemDto.UnitPrice);
            }

            invoice.TotalAmount = totalAmount;

            _unitOfWork.Repository<Invoice>().Update(invoice);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IReadOnlyList<DtoForReturnSupplierInvoice>> GetAllAsync()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsync(i => i.Supplier);
            var CahInvoice = invoices.Where(I => I.invoiceType == InvoiceType.SupplierReturn).ToList();

            var response = _mapper.Map<IReadOnlyList<DtoForReturnSupplierInvoice>>(CahInvoice);

            return response;
        }

        public async Task<ReturnSupplierInvoiceDetailsDto> GetInvoiceById(int id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>()
               .FindWithIncludesAsync(i => i.Id == id && i.invoiceType == InvoiceType.SupplierReturn,
               i => i.Supplier, id => id.Items);

            if (invoice == null)
                return null;

            var response = _mapper.Map<ReturnSupplierInvoiceDetailsDto>(invoice);

            return response;
        }

        public async Task<List<ReturnSupplierInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId)
        {
            var items = await _unitOfWork.Repository<InvoiceItem>()
                .GetAllAsync(i => i.InvoiceId == invoiceId);

            return items.Select(i => new ReturnSupplierInvoiceItemDetailsDto
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
