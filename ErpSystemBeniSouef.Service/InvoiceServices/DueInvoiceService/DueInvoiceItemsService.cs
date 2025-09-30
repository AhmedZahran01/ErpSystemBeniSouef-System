using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice.DueInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
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
    public class DueInvoiceItemsService : IDueInvoiceItemService
    {

        #region Constractor Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DueInvoiceItemsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Add Due Invoice Items Region

        public async Task<bool> AddInvoiceItems(AddDueInvoiceItemsDto dto)
        { 
            var invoice = await _unitOfWork.Repository<Invoice>()
                .FindWithIncludesAsync(i => i.Id == dto.Id && i.invoiceType == InvoiceType.Due,
                i => i.Supplier);


            if (invoice == null)
                return false;
                //throw new Exception($"Invoice with Id {dto.Id} not found.");

            decimal? totalAmount = dto.InvoiceTotalPrice + invoice.TotalAmount;

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
                    ProductType = itemDto.ProductTypeName ?? "N/A",
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    Notes = itemDto.Note,
                    
                };

                if (invoice.Items == null)
                    invoice.Items = new List<InvoiceItem>();

                invoice.Items.Add(invoiceItem);

                //totalAmount += (itemDto.Quantity * itemDto.UnitPrice);
            }

            invoice.TotalAmount = totalAmount;

            _unitOfWork.Repository<Invoice>().Update(invoice);
            await _unitOfWork.CompleteAsync();

            return true;
        }
          
        #endregion

        #region Get Invoice By Id Region

        //public async Task<DueInvoiceDetailsDto> GetInvoiceById(int id)
        //{
        //    var invoice = await _unitOfWork.Repository<Invoice>()
        //        .FindWithIncludesAsync(i => i.Id == id && i.invoiceType == InvoiceType.Due && !i.IsDeleted,
        //       i => i.Supplier);

        //    if (invoice == null)
        //        throw new Exception($"Invoice with Id {id} not found.");

        //    return new DueInvoiceDetailsDto
        //    {
        //        Id = invoice.Id,
        //        InvoiceDate = invoice.InvoiceDate,
        //        TotalAmount = invoice.TotalAmount ?? 0,
        //        SupplierName = invoice.Supplier?.Name ?? "N/A",
        //        DueAmount = invoice.DueAmount,
        //        Notes = invoice.Notes
        //    };
        //}

        #endregion

        #region  Get All Invoice Items By Invoice Id Region

        public async Task<List<DueInvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId)
        {
            var Allitems = await _unitOfWork.Repository<InvoiceItem>()
              .GetAllAsync();
            var items = Allitems.Where(i => i.InvoiceId == invoiceId).ToList();
            return items.Select(i => new DueInvoiceItemDetailsDto
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

        #endregion
          
        #region Soft Delete Invoice Region

        public bool SoftDelete(int id, decimal _totalLine, int _invoiceId)
        {
            var invoice = _unitOfWork.Repository<Invoice>().GetById(_invoiceId);
            if (invoice == null)
                return false;
            invoice.TotalAmount -= _totalLine;
            _unitOfWork.Repository<Invoice>().Update(invoice);


            var invoiceItem = _unitOfWork.Repository<InvoiceItem>().GetById(id);
            if (invoiceItem == null)
                return false;

            invoiceItem.IsDeleted = true;
            _unitOfWork.Repository<InvoiceItem>().Update(invoiceItem);

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





    }
}
