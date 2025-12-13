using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.CreateCashCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.ReturnAllCashCustomerInvoices;
using Microsoft.EntityFrameworkCore;
using ErpSystemBeniSouef.Core.GenericResponseModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ErpSystemBeniSouef.Core.Contract.CashCustomerInvoiceServices;

namespace ErpSystemBeniSouef.Service.CashCustomerInvoices
{
    public class CashCustomerInvoiceService : ICashCustomerInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constractor Region

        public CashCustomerInvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Add Cash Customer Invoice Region

        public async Task<ServiceResponse<bool>> AddCashCustomerInvoice(CreateCashCustomerInvoiceDTO dto)
        {
            try
            {
                var totalAmount = dto.cashcustomerinvoicedtos.Sum(i => i.Total);

                var invoice = new CashCstomerInvoice
                {
                    SubAreaId = dto.SubAreaId,
                    RepresentativeId = dto.RepresentativeId,
                    InvoiceDate = dto.SaleDate,
                    TotalAmount = totalAmount,
                    Items = new List<CashCustomerInvoiceItems>()
                };

                foreach (var itemDto in dto.cashcustomerinvoicedtos)
                {
                    var item = new CashCustomerInvoiceItems
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = itemDto.Price,
                        // CashCustomerInvoiceId = invoice.Id, 
                        cashCstomerInvoice = invoice
                    };
                    invoice.Items.Add(item);
                }

                // Step 4: Save
                _unitOfWork.Repository<CashCstomerInvoice>().Add(invoice);
                await _unitOfWork.CompleteAsync();

                return ServiceResponse<bool>.SuccessResponse(true, " created cash invoice.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating cash invoice: {ex.Message}");
                return ServiceResponse<bool>.Failure("Error creating cash invoice.");
            }
        }

        #endregion

        #region Get All Cash Customer Invoices Region

        public async Task<ServiceResponse<List<ReturnAllCashCustomerInvoicesDTO>>> GetAllCashCustomerInvoices()
        {
            var repo = _unitOfWork.Repository<CashCstomerInvoice>();

            var invoices = await repo
                .GetAllQueryable(
                c => c.SubArea,
                  c => c.SubArea.mainRegions,
                c => c.Representative
                ).ToListAsync();

            var result = invoices.Select((invoice, index) => new ReturnAllCashCustomerInvoicesDTO
            {
                serialNumber = index + 1,
                serialNumberIdFromDB = invoice.Id ,
                SaleDate = invoice.InvoiceDate,
                MainAreaName = invoice.SubArea?.mainRegions?.Name ?? "",
                SubAreaName = invoice.SubArea?.Name ?? "",
                RepresentativeName = invoice.Representative?.Name ?? "",
                total = invoice.TotalAmount ?? 0
            }).ToList();

            return ServiceResponse<List<ReturnAllCashCustomerInvoicesDTO>>.SuccessResponse(result, "Fetched all cash invoices.");
        }

        #endregion

        #region Delete Cash Customer Invoice Region

        public async Task<ServiceResponse<bool>> DeleteCashCustomerInvoiceAsync(int invoiceId)
        {
            try
            {
                var invoiceRepo = _unitOfWork.Repository<CashCstomerInvoice>();
                var itemRepo = _unitOfWork.Repository<CashCustomerInvoiceItems>();

                var invoice = await invoiceRepo
                    .GetAllQueryable(i => i.Items)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice == null)
                    return ServiceResponse<bool>.Failure("Cash invoice not found.");

                if (invoice.Items != null && invoice.Items.Any())
                {
                    foreach (var item in invoice.Items)
                    {
                        itemRepo.Delete(item);
                    }
                }

                invoiceRepo.Delete(invoice);

                await _unitOfWork.CompleteAsync();

                return ServiceResponse<bool>.SuccessResponse(true, "Created sussessfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting cash invoice: {ex.Message}");
                return ServiceResponse<bool>.Failure("Error deleting cash invoice.");
            }
        }

        #endregion

    }
}



