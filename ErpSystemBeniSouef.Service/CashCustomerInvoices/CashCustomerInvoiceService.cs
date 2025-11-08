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

namespace ErpSystemBeniSouef.Service.CashCustomerInvoices
{
    public class CashCustomerInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CashCustomerInvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddCashCustomerInvoice(CreateCashCustomerInvoiceDTO dto)
        {
            try
            {
                var totalAmount = dto.cashcustomerinvoicedtos.Sum(i => i.Total);

                var invoice = new CashCstomerInvoice
                {
                    SubAreaId = dto.SubAreaId,
                    RepresentativeId=dto.RepresentativeId,
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

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating cash invoice: {ex.Message}");
                return false;
            }
        }
        public async Task<List<ReturnAllCashCustomerInvoicesDTO>> GetAllCashCustomerInvoices()
        {
            var repo = _unitOfWork.Repository<CashCstomerInvoice>();

            var invoices = await repo
                .GetAllQueryable(
                c => c.SubArea,
                  c=>c.SubArea.mainRegions,
                c => c.Representative
                ).ToListAsync() ;

            var result = invoices.Select((invoice, index) => new ReturnAllCashCustomerInvoicesDTO
            {
                serialNumber = index + 1,
                SaleDate = invoice.InvoiceDate,
                MainAreaName = invoice.SubArea?.mainRegions?.Name ?? "",
                SubAreaName = invoice.SubArea?.Name ?? "",
                RepresentativeName = invoice.Representative?.Name ?? "",
                total = invoice.TotalAmount ?? 0
            }).ToList();

            return result;
        }
    }
}


