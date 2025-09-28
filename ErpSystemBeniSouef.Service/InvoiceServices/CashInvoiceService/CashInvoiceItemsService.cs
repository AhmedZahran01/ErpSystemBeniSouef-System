using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.InvoiceServices.CashInvoiceService
{
    public class CashInvoiceItemsService : ICashInvoiceItemsService
    {

        #region Constractor Region
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CashInvoiceItemsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region  Get All Invoice Items By Invoice Id Region

        public async Task<List<InvoiceItemDetailsDto>> GetInvoiceItemsByInvoiceId(int invoiceId)
        {
            var items = await _unitOfWork.Repository<InvoiceItem>()
                .GetAllAsync();
            items = items.Where(i => i.InvoiceId == invoiceId).ToList();
            return items.Select(i => new InvoiceItemDetailsDto
            {
                Id = i.Id,
                InvoiceId = i.InvoiceId,
                ProductName = i.ProductName,
                ProductId = i.Id,
                ProductType = i.ProductType,
                ProductTypeName = i.ProductType,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Notes = i.Notes
            }).ToList();
        }

        #endregion

        #region Add Cash Invoice Items Dto Region

        public async Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto)
        {
            Invoice invoice = await _unitOfWork.Repository<Invoice>()
                .FindWithIncludesAsync(i => i.Id == dto.Id && i.invoiceType == InvoiceType.cash, i => i.Supplier);

            if (invoice == null)
                return false;

            //decimal totalAmount = dto.InvoiceTotalPrice;
            decimal totalAmount = invoice.TotalAmount ?? 0;
            totalAmount += dto.InvoiceTotalPrice;

            foreach (var itemDto in dto.invoiceItemDtos)
            {
                var product = _unitOfWork.Repository<Product>().GetById(itemDto.ProductId);
                if (product == null)
                    return false;

                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    ProductType = itemDto.ProductTypeName ?? "N/A",
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                };

                if (invoice.Items == null)
                    invoice.Items = new List<InvoiceItem>();
                invoice.Items.Add(invoiceItem);

            }

            invoice.TotalAmount = totalAmount;

            _unitOfWork.Repository<Invoice>().Update(invoice);
            await _unitOfWork.CompleteAsync();

            return true;

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
