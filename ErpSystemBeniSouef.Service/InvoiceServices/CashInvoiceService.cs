using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.Entities;

namespace ErpSystemBeniSouef.Service.InvoiceServices
{
    public class CashInvoiceService:ICashInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CashInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReturnInvoiceDto> AddInvoice(AddInvoiceDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            // Make sure supplier exists
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                throw new Exception($"Supplier with Id {dto.SupplierId} not found.");

            invoice.SupplierId = dto.SupplierId;
            invoice.Supplier = supplier;

            // Default values
            invoice.TotalAmount = 0; // Later updated when items are added
            invoice.CreatedDate = DateTime.UtcNow;

            // Save to DB
            _unitOfWork.Repository<Invoice>().Add(invoice);
            await _unitOfWork.CompleteAsync();

            // Map Entity → Return DTO
            var returnDto = new ReturnInvoiceDto
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = (decimal)invoice.TotalAmount,
                SupplierName = supplier.Name
            };

            return returnDto;
        }
       public async  Task<bool> AddInvoiceItems(AddInvoiceItemsDto dto)
        {
            // 1. Get the invoice
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(dto.Id);
            if (invoice == null)
                throw new Exception($"Invoice with Id {dto.Id} not found.");

            decimal totalAmount = invoice.TotalAmount ?? 0;

            // 2. Loop through items
            foreach (var itemDto in dto.invoiceItemDtos)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(itemDto.Product);
                if (product == null)
                    throw new Exception($"Product with Id {itemDto.Product} not found.");

                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    ProductType = product.Category?.Name ?? "N/A",  // Example: get category name as type
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                };

                // Add to invoice collection
                if (invoice.Items == null)
                    invoice.Items = new List<InvoiceItem>();
                invoice.Items.Add(invoiceItem);

                // Update total
                totalAmount += (itemDto.Quantity * itemDto.UnitPrice);
            }

            // 3. Update invoice totals
            invoice.TotalAmount = totalAmount;

            // 4. Save
            _unitOfWork.Repository<Invoice>().Update(invoice);
            await _unitOfWork.CompleteAsync();

            return true;

        }

    }

       
    }
