using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Enum;

namespace ErpSystemBeniSouef.Service.InvoiceServices
{
    public class CashInvoiceService : ICashInvoiceService
    {

        #region Constractor Region
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CashInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region  Add Invoice Region

        public ReturnCashInvoiceDto AddInvoice(AddCashInvoiceDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            // Make sure supplier exists
            var supplier = _unitOfWork.Repository<Supplier>().GetById(dto.SupplierId);
            if (supplier == null)
                return null;

            invoice.SupplierId = dto.SupplierId;
            invoice.Supplier = supplier;
            invoice.invoiceType = InvoiceType.cash;
            invoice.TotalAmount = 0;
            invoice.CreatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Invoice>().Add(invoice);
            _unitOfWork.Complete();

            // Map Entity → Return DTO
            var returnDto = new ReturnCashInvoiceDto
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = (decimal)invoice.TotalAmount,
                SupplierName = supplier.Name,
                 SupplierId = supplier.Id
            };

            return returnDto;
        }

        #endregion

        #region Add Invoice Items Region

        public async Task<bool> AddInvoiceItems(AddCashInvoiceItemsDto dto)
        {
            Invoice invoice = await _unitOfWork.Repository<Invoice>()
                .FindWithIncludesAsync(i => i.Id == dto.Id && i.invoiceType == InvoiceType.cash, i => i.Supplier);
 
            if (invoice == null)
                return false;

            decimal totalAmount = invoice.TotalAmount ?? 0;

            foreach (var itemDto in dto.invoiceItemDtos)
            {
                var product =  _unitOfWork.Repository<Product>().GetById(itemDto.ProductId);
                if (product == null)
                    return false;

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

        #endregion

        #region Get Invoice By Id Region

        public async Task<InvoiceDetailsDto> GetInvoiceById(int id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>()
               .FindWithIncludesAsync(i => i.Id == id && i.invoiceType == InvoiceType.cash,
               i => i.Supplier, id => id.Items);

            if (invoice == null)
                return null;

            var response = _mapper.Map<InvoiceDetailsDto>(invoice);

            return response;

        }
        #endregion

        #region  Get Invoice Items By Invoice Id Region

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
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Notes = i.Notes
            }).ToList();
        }

        #endregion

        #region Get All Async Region
        public async Task<IReadOnlyList<ReturnCashInvoiceDto>> GetAllAsync()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsync(i => i.Supplier);
            var CahInvoice = invoices.Where(I => I.invoiceType == InvoiceType.cash).ToList();

            var response = _mapper.Map<IReadOnlyList<ReturnCashInvoiceDto>>(CahInvoice);

            return response;
        }

        #endregion


        #region Update Region

        public bool Update(UpdateInvoiceDto updateDto)
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

            _unitOfWork.Repository<Invoice>().Update(invoice);
            _unitOfWork.CompleteAsync();
            return true;
        }
         
        #endregion


        #region Soft Delete Region

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


    }
}


