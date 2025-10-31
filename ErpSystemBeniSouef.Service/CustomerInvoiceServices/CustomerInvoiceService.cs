using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.GenericResponseModel;
using Microsoft.EntityFrameworkCore;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.GetAllDetailsForCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;

namespace ErpSystemBeniSouef.Service.CustomerInvoiceServices
{
    public class CustomerInvoiceService : ICustomerInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Create Customer Invoice

        public async Task<ServiceResponse<bool>> CreateCustomerInvoiceAsync(CreateCustomerInvoiceDTO dto)
        {
            try
            {
                // Validate Collector, Representative, and SubArea
                var collector = await _unitOfWork.Repository<Collector>().GetByIdAsync(dto.CollectorId);
                if (collector == null)
                    return ServiceResponse<bool>.Failure("Collector not found.");

                var representative = await _unitOfWork.Repository<Representative>().GetByIdAsync(dto.RepresentativeId);
                if (representative == null)
                    return ServiceResponse<bool>.Failure("Representative not found.");

                var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(dto.SubAreaId);
                if (subArea == null)
                    return ServiceResponse<bool>.Failure("Sub-area not found.");

                //  Create the customer
                var customer = new Customer
                {
                    CustomerNumber = dto.CustomerNumber,
                    Name = dto.Name,
                    MobileNumber = dto.MobileNumber,
                    Address = dto.Address,
                    NationalNumber = dto.NationalNumber,
                    Deposit = dto.Deposit,
                    SaleDate = dto.SaleDate,
                    FirstInvoiceDate = dto.FirstInvoiceDate,
                    SubAreaId = dto.SubAreaId,
                    CollectorId = dto.CollectorId,
                    RepresentativeId = dto.RepresentativeId
                };

                _unitOfWork.Repository<Customer>().Add(customer);
                await _unitOfWork.CompleteAsync();

                // 3 Create the invoice
                var invoice = new CustomerInvoice
                {
                    CustomerId = customer.Id,
                    InvoiceDate = DateTime.UtcNow,
                    TotalAmount = dto.customerinvoicedtos.Sum(i => i.Total)
                };

                _unitOfWork.Repository<CustomerInvoice>().Add(invoice);
                await _unitOfWork.CompleteAsync();

                // Create invoice items
                foreach (var item in dto.customerinvoicedtos)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (product == null)
                        return ServiceResponse<bool>.Failure($"Product with ID {item.ProductId} not found.");

                    var invoiceItem = new CustomerInvoiceItems
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    _unitOfWork.Repository<CustomerInvoiceItems>().Add(invoiceItem);
                }

                //  Create installment plans
                foreach (var inst in dto.installmentsdtos)
                {
                    var installment = new InstallmentPlan
                    {
                        InvoiceId = invoice.Id,
                        NumberOfMonths = inst.NumberOfMonths,
                        Amount = inst.Amount
                    };

                    _unitOfWork.Repository<InstallmentPlan>().Add(installment);
                }

                await _unitOfWork.CompleteAsync();

                return ServiceResponse<bool>.SuccessResponse(true, "Customer invoice created successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.Failure($"An error occurred: {ex.Message}");
            }
        }

        #endregion


        #region Get All Customer Invoices

        public async Task<ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>> GetAllCustomerInvoicesAsync()
        {
            try
            {
               
                var query = _unitOfWork.Repository<Customer>()
            .GetAllQueryable(
                c => c.SubArea,
                c => c.Collector,
                c => c.Representative,
                c => c.Invoices
            );


                var customers = await query.ToListAsync();
                if (customers == null || customers.Count == 0)
                    return ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>.Failure("No customer invoices found.");

                int serial = 1;

                var response = customers.Select(c => new ReturnCustomerInvoiceListDTO
                {
                    serialNumber = serial++,
                    CustomerNumber = c.CustomerNumber,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    NationalNumber = c.NationalNumber,
                    Deposit = c.Deposit,
                    SaleDate = c.SaleDate,
                    FirstInvoiceDate = c.FirstInvoiceDate,
                    SubAreaName = c.SubArea?.Name ?? "N/A",
                    CollectorName = c.Collector?.Name ?? "N/A",
                    RepresentativeName = c.Representative?.Name ?? "N/A"
                }).ToList();

                return ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>
                    .SuccessResponse(response, "Customer invoices retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>.Failure($"Error: {ex.Message}");
            }
        }

        #endregion


        #region get customerinvoicebyid
        public async Task<ServiceResponse<ReturnCustomerInvoiceDetailsDTO>> GetCustomerInvoiceByIdAsync(int invoiceId)
        {
            try
            {
                var invoice = await _unitOfWork.Repository<CustomerInvoice>()
                    .GetAllQueryable(
                        i => i.Customer,
                        i => i.Items,
                        i => i.Items.Select(ii => ii.Product),
                        i => i.Installments
                    )
                    .FirstOrDefaultAsync(i => i.Id == invoiceId && !i.IsDeleted);

                if (invoice == null)
                    return ServiceResponse<ReturnCustomerInvoiceDetailsDTO>.Failure("Invoice not found.");

                var customer = invoice.Customer;

                var dto = new ReturnCustomerInvoiceDetailsDTO
                {
                    CustomerNumber = customer.CustomerNumber,
                    Name = customer.Name,
                    MobileNumber = customer.MobileNumber,
                    Address = customer.Address,
                    NationalNumber = customer.NationalNumber,
                    Deposit = customer.Deposit,
                    SaleDate = customer.SaleDate,
                    FirstInvoiceDate = customer.FirstInvoiceDate,
                    SubAreaId = customer.SubAreaId,
                    CollectorId = customer.CollectorId,
                    RepresentativeId = customer.RepresentativeId,

                    Installments = invoice.Installments?.Select(inst => new InstallmentDTO
                    {
                        NumberOfMonths = inst.NumberOfMonths,
                        Amount = inst.Amount
                    }).ToList() ?? new List<InstallmentDTO>(),

                    CustomerInvoiceItems = invoice.Items?.Select(item => new CustomerInvoiceItemsDTO
                    {
                        CategoryName = item.Product?.Category?.Name ?? "N/A",
                        ProductName = item.Product?.ProductName ?? "N/A",
                        Quantity = item.Quantity,
                        Price = item.Price,
                    }).ToList() ?? new List<CustomerInvoiceItemsDTO>()
                };

                return ServiceResponse<ReturnCustomerInvoiceDetailsDTO>
                    .SuccessResponse(dto, "Customer invoice details retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<ReturnCustomerInvoiceDetailsDTO>.Failure($"Error: {ex.Message}");
            }
        }

        #endregion







        #region delete customer invoice
        public async Task<ServiceResponse<bool>> DeleteCustomerAsync(int customerId)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var customer = await _unitOfWork.Repository<Customer>()
                    .GetAllQueryable()
                    .Include(c => c.Invoices)
                        .ThenInclude(i => i.Items)
                            .ThenInclude(it => it.Product)
                    .FirstOrDefaultAsync(c => c.Id == customerId);

                if (customer == null)
                    return ServiceResponse<bool>.Failure("Customer not found.");

                // Get Representative and current Covenant
                var representative = await _unitOfWork.Repository<Representative>().
                    GetByIdAsync(customer.RepresentativeId);
                if (representative == null)
                    return ServiceResponse<bool>.Failure("Representative not found.");

                var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var covenant = await _unitOfWork.Repository<Covenant>()
                    .GetAllQueryable()
                    .Include(c => c.CovenantProducts)
                    .FirstOrDefaultAsync(c =>
                        c.RepresentativeId == representative.Id &&
                        c.MonthDate == currentMonth);

                if (covenant == null)
                {
                    covenant = new Covenant
                    {
                        RepresentativeId = representative.Id,
                        CovenantDate = DateTime.Now,
                        MonthDate = currentMonth,
                        CovenantType = "Return",
                        CovenantProducts = new List<CovenantProduct>()
                    };
                     _unitOfWork.Repository<Covenant>().Add(covenant);
                }

                //  Return Products to Covenant
                foreach (var invoice in customer.Invoices)
                {
                    foreach (var item in invoice.Items)
                    {
                        var covenantProduct = covenant.CovenantProducts
                            .FirstOrDefault(p => p.ProductId == item.ProductId);

                        if (covenantProduct != null)
                            covenantProduct.Amount += item.Quantity;
                        else
                        {
                            covenant.CovenantProducts.Add(new CovenantProduct
                            {
                                ProductId = item.ProductId,
                                CategoryId = item.Product.CategoryId,
                                Amount = item.Quantity
                            });
                        }
                    }

                    // Reverse Discounts for Current Month
                    await ReverseDiscountsForInvoice( invoice);

                    //  Deduct Representative Commission
                    await DeductCommissionForInvoice( invoice, representative.Id);
                }

                //  Delete Customer and Invoices
                _unitOfWork.Repository<Customer>().Delete(customer);

                // Save all changes
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();

                return ServiceResponse<bool>.SuccessResponse(true,"Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResponse<bool>.Failure($"Error deleting customer: {ex.Message}");
            }
        }

        // ------------------------------------------
        // Helper Methods
        // ------------------------------------------
        private async Task ReverseDiscountsForInvoice(CustomerInvoice invoice)
        {
            var discountRepo = _unitOfWork.Repository<Discount>();
            var discounts = await discountRepo.GetAllAsync(d => d.Invoice);

            var invoiceDiscounts = discounts
                .Where(d => d.InvoiceId == invoice.Id && !d.IsDeleted)
                .ToList();

            foreach (var discount in invoiceDiscounts)
            {
                discount.IsReversed = true;
                discount.ReversedDate = DateTime.Now;
                discount.Reason = "Reversed automatically due to customer deletion.";
                discountRepo.Update(discount);
            }
        }

        private async Task DeductCommissionForInvoice(CustomerInvoice invoice, int representativeId)
        {
            var commissionRepo = _unitOfWork.Repository<Commission>();
            var commissions = await commissionRepo.GetAllAsync(c => c.Representative);

            var commission = commissions
                .FirstOrDefault(c => c.InvoiceId == invoice.Id && c.RepresentativeId == representativeId);

            if (commission != null && !commission.IsDeducted)
            {
                commission.DeductedAmount = commission.CommissionAmount * 0.10m; // Deduct 10% as penalty
                commission.CommissionAmount -= commission.DeductedAmount;
                commission.IsDeducted = true;
                commission.DeductedDate = DateTime.Now;
                commission.Note = "Commission deducted due to customer deletion.";
                commissionRepo.Update(commission);
            }
        }

        #endregion





    }
}
