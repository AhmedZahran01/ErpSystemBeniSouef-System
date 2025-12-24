using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.GetAllDetailsForCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;
using ErpSystemBeniSouef.Core.GenericResponseModel;
using ErpSystemBeniSouef.Infrastructer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.CustomerInvoiceServices
{
    public class CustomerInvoiceService
    {
        #region Properies Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constractor Region
        public CustomerInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion
        #region MyRegion

        //        #region Create Customer Invoice
        //        public async Task<ServiceResponse<bool>> CreateCustomerInvoiceAsync(CreateCustomerInvoiceDTO dto)
        //        {
        //            using var transaction = await _unitOfWork.BeginTransactionAsync();
        //            try
        //            {
        //                // Validate Collector, Representative, and SubArea
        //                var collector = await _unitOfWork.Repository<Collector>().GetByIdAsync(dto.CollectorId);
        //                if (collector == null)
        //                    return ServiceResponse<bool>.Failure("Collector not found.");

        //                var representative = await _unitOfWork.Repository<Representative>().GetByIdAsync(dto.RepresentativeId);
        //                if (representative == null)
        //                    return ServiceResponse<bool>.Failure("Representative not found.");

        //                var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(dto.SubAreaId);
        //                if (subArea == null)
        //                    return ServiceResponse<bool>.Failure("Sub-area not found.");

        //                //  Create the customer
        //                var customer = new Customer
        //                {
        //                    CustomerNumber = dto.CustomerNumber,
        //                    Name = dto.Name,
        //                    MobileNumber = dto.MobileNumber,
        //                    Address = dto.Address,
        //                    NationalNumber = dto.NationalNumber,
        //                    Deposit = dto.Deposit,
        //                    SaleDate = dto.SaleDate,
        //                    FirstInvoiceDate = dto.FirstInvoiceDate,
        //                    SubAreaId = dto.SubAreaId,
        //                    CollectorId = dto.CollectorId,
        //                    RepresentativeId = dto.RepresentativeId
        //                };

        //                _unitOfWork.Repository<Customer>().Add(customer);
        //                await _unitOfWork.CompleteAsync();

        //                var totalAmount = dto.customerinvoicedtos.Sum(i => i.Total);
        //                // 3 Create the invoice
        //                var invoice = new CustomerInvoice
        //                {
        //                    CustomerId = customer.Id,
        //                    InvoiceDate = DateTime.UtcNow,
        //                    TotalAmount = totalAmount
        //                };
        //                decimal deposit = dto.Deposit;
        //                decimal netAmount = totalAmount - deposit;
        //                _unitOfWork.Repository<CustomerInvoice>().Add(invoice);
        //                await _unitOfWork.CompleteAsync();

        //                _unitOfWork.Repository<CustomerInvoiceItems>().Add(invoiceItem);
        //                await _unitOfWork.CompleteAsync();
        //                //var covenantResult = await DeductFromCovenantAsync(representative.Id, product.Id, item.Quantity);
        //                //if (!covenantResult.Success)
        //                //    return ServiceResponse<bool>.Failure(covenantResult.Message);

        //                //Create commission record(snapshot commissionPerUnit and total)
        //                await CreateCommissionRecordAsync(representative.Id, invoice.Id, product, item.Quantity, invoice.InvoiceDate, invoiceItem.Id);
        //            }

        //                //  Create installment plans
        //                foreach (var inst in dto.installmentsdtos)
        //            {
        //                var installment = new InstallmentPlan
        //                {
        //                    InvoiceId = invoice.Id,
        //                    NumberOfMonths = inst.NumberOfMonths,
        //                    Amount = inst.Amount
        //                };

        //                _unitOfWork.Repository<InstallmentPlan>().Add(installment);

        //                var lastInstallment = _unitOfWork.Repository<MonthlyInstallment>()
        //                    .GetAllQueryable()
        //                    .Where(m => m.InvoiceId == invoice.Id)
        //                    .OrderByDescending(m => m.MonthDate)
        //                    .FirstOrDefault();

        //                var startDate = lastInstallment is null
        //                    ? invoice.InvoiceDate.AddMonths(1)
        //                    : lastInstallment.MonthDate.AddMonths(1);

        //                await GenerateMonthlyInstallmentsAsync(invoice, inst.NumberOfMonths, inst.Amount, startDate);
        //            }

        //            await _unitOfWork.CompleteAsync();
        //            await transaction.CommitAsync();
        //            return ServiceResponse<bool>.SuccessResponse(true, "Customer invoice created successfully.");
        //        }

        //            catch (Exception ex)
        //            {
        //                await transaction.RollbackAsync();
        //                return ServiceResponse<bool>.Failure($"An error occurred: {ex.Message}");
        //            }
        //}

        //#endregion

        //#region Get All Customer Invoices

        //public async Task<ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>> GetAllCustomerInvoicesAsync()
        //{
        //    try
        //    {

        //        var query = _unitOfWork.Repository<Customer>()
        //          .GetAllQueryable(
        //        c => c.SubArea,
        //        c => c.Collector,
        //        c => c.Representative,
        //        c => c.Invoices
        //                     );


        //        var customers = await query.ToListAsync();
        //        if (customers == null || customers.Count == 0)
        //            return ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>.Failure("No customer invoices found.");

        //        int serial = 1;

        //        var response = customers.Select(c => new ReturnCustomerInvoiceListDTO
        //        {
        //            serialNumber = serial++,
        //            CustomerNumber = c.CustomerNumber,
        //            Name = c.Name,
        //            MobileNumber = c.MobileNumber,
        //            Address = c.Address,
        //            NationalNumber = c.NationalNumber,
        //            Deposit = c.Deposit,
        //            SaleDate = c.SaleDate,
        //            FirstInvoiceDate = c.FirstInvoiceDate,
        //            SubAreaName = c.SubArea?.Name ?? "N/A",
        //            CollectorName = c.Collector?.Name ?? "N/A",
        //            RepresentativeName = c.Representative?.Name ?? "N/A",
        //            MainAreaId = c.SubArea.MainAreaId,
        //            CollectorId = c.CollectorId,
        //            RepresentativeId = c.RepresentativeId,
        //            SubAreaId = c.SubAreaId,
        //            Id = c.Id,
        //        }).ToList();

        //        return ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>
        //            .SuccessResponse(response, "Customer invoices retrieved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>.Failure($"Error: {ex.Message}");
        //    }
        //}

        //#endregion

        //#region get customerinvoicebyid
        //public async Task<ServiceResponse<ReturnCustomerInvoiceDetailsDTO>> GetCustomerInvoiceByIdAsync(int invoiceId)
        //{
        //    try
        //    {
        //        var invoice = await _unitOfWork.Repository<CustomerInvoice>()
        //            .GetAllQueryable(
        //                i => i.Customer,
        //                i => i.Items,
        //                //i => i.Items.Select(ii => ii.Product),
        //                i => i.Installments
        //            )
        //            .FirstOrDefaultAsync(i => i.CustomerId == invoiceId && !i.IsDeleted);

        //        if (invoice == null)
        //            return ServiceResponse<ReturnCustomerInvoiceDetailsDTO>.Failure("Invoice not found.");

        //        var customer = invoice.Customer;

        //        var dto = new ReturnCustomerInvoiceDetailsDTO
        //        {
        //            CustomerNumber = customer.CustomerNumber,
        //            Name = customer.Name,
        //            MobileNumber = customer.MobileNumber,
        //            Address = customer.Address,
        //            NationalNumber = customer.NationalNumber,
        //            Deposit = customer.Deposit,
        //            SaleDate = customer.SaleDate,
        //            FirstInvoiceDate = customer.FirstInvoiceDate,
        //            SubAreaId = customer.SubAreaId,
        //            CollectorId = customer.CollectorId,
        //            RepresentativeId = customer.RepresentativeId,

        //            Installments = invoice.Installments?.Select(inst => new InstallmentDTO
        //            {
        //                NumberOfMonths = inst.NumberOfMonths,
        //                Amount = inst.Amount
        //            }).ToList() ?? new List<InstallmentDTO>(),

        //            CustomerInvoiceItems = invoice.Items?.Select(item => new CustomerInvoiceItemsDTO
        //            {
        //                CategoryName = item.Product?.Category?.Name ?? "N/A",
        //                ProductName = item.Product?.ProductName ?? "N/A",
        //                Quantity = item.Quantity,
        //                Price = item.Price,
        //            }).ToList() ?? new List<CustomerInvoiceItemsDTO>()
        //        };

        //        return ServiceResponse<ReturnCustomerInvoiceDetailsDTO>
        //            .SuccessResponse(dto, "Customer invoice details retrieved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ServiceResponse<ReturnCustomerInvoiceDetailsDTO>.Failure($"Error: {ex.Message}");
        //    }
        //}

        //#endregion

        //#region delete customer invoice
        //public async Task<ServiceResponse<bool>> DeleteCustomerAsync(int customerId)
        //{
        //    using var transaction = await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var customer = await _unitOfWork.Repository<Customer>()
        //            .GetAllQueryable()
        //            .Include(c => c.Invoices)
        //                .ThenInclude(i => i.Items)
        //                    .ThenInclude(it => it.Product)
        //            .FirstOrDefaultAsync(c => c.Id == customerId);

        //        if (customer == null)
        //            return ServiceResponse<bool>.Failure("Customer not found.");

        //        // Get Representative and current Covenant
        //        var representative = await _unitOfWork.Repository<Representative>().
        //            GetByIdAsync(customer.RepresentativeId);
        //        if (representative == null)
        //            return ServiceResponse<bool>.Failure("Representative not found.");

        //        var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //        var covenant = await _unitOfWork.Repository<Covenant>()
        //            .GetAllQueryable()
        //            .Include(c => c.CovenantProducts)
        //            .FirstOrDefaultAsync(c =>
        //                c.RepresentativeId == representative.Id &&
        //                c.MonthDate == currentMonth);

        //        if (covenant == null)
        //        {
        //            covenant = new Covenant
        //            {
        //                RepresentativeId = representative.Id,
        //                CovenantDate = DateTime.Now,
        //                MonthDate = currentMonth,
        //                CovenantType = "Return",
        //                CovenantProducts = new List<CovenantProduct>(),
        //                CustomerId = customer.Id
        //            };
        //            _unitOfWork.Repository<Covenant>().Add(covenant);
        //        }

        //        //  Return Products to Covenant
        //        foreach (var invoice in customer.Invoices)
        //        {
        //            foreach (var item in invoice.Items)
        //            {
        //                var covenantProduct = covenant.CovenantProducts
        //                    .FirstOrDefault(p => p.ProductId == item.ProductId);

        //                if (covenantProduct != null)
        //                    covenantProduct.Amount += item.Quantity;
        //                else
        //                {
        //                    covenant.CovenantProducts.Add(new CovenantProduct
        //                    {
        //                        ProductId = item.ProductId,
        //                        CategoryId = item.Product.CategoryId,
        //                        Amount = item.Quantity
        //                    });
        //                }
        //            }

        //            // Reverse Discounts for Current Month
        //            await ReverseDiscountsForInvoice(invoice);

        //            //  Deduct Representative Commission
        //            await DeductCommissionForInvoice(invoice, representative.Id);
        //            //  Remove monthly installments and plans related to this invoice
        //            var monthlyInstallments = await _unitOfWork.Repository<MonthlyInstallment>()
        //                .GetAllAsync(m => m.InvoiceId == invoice.Id);

        //            if (monthlyInstallments != null && monthlyInstallments.Any())
        //            {
        //                foreach (var mi in monthlyInstallments)
        //                    _unitOfWork.Repository<MonthlyInstallment>().Delete(mi);
        //            }

        //            var installmentPlans = await _unitOfWork.Repository<InstallmentPlan>()
        //                .GetAllAsync(p => p.InvoiceId == invoice.Id);

        //            if (installmentPlans != null && installmentPlans.Any())
        //            {
        //                foreach (var p in installmentPlans)
        //                    _unitOfWork.Repository<InstallmentPlan>().Delete(p);
        //            }
        //        }

        //        //  Delete Customer and Invoices
        //        _unitOfWork.Repository<Customer>().Delete(customer);

        //        // Save all changes
        //        await _unitOfWork.CompleteAsync();
        //        await transaction.CommitAsync();

        //        return ServiceResponse<bool>.SuccessResponse(true, "Customer deleted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return ServiceResponse<bool>.Failure($"Error deleting customer: {ex.Message}");
        //    }
        //}

        //// ------------------------------------------
        //// Helper Methods
        //// ------------------------------------------

        //private async Task<(bool Success, string Message)> DeductFromCovenantAsync(int representativeId, int productId, int quantity)
        //{
        //    // find current covenant for representative (current month)
        //    var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        //    var covenant = await _unitOfWork.Repository<Covenant>()
        //        .GetAllQueryable()
        //        .Include(c => c.CovenantProducts)
        //        .FirstOrDefaultAsync(c => c.RepresentativeId == representativeId && c.MonthDate == currentMonth);

        //    if (covenant == null)
        //    {
        //        // if no covenant exists, you may want to create one OR return error
        //        return (false, "Representative covenant for current month not found.");
        //    }

        //    var covenantProduct = covenant.CovenantProducts.FirstOrDefault(cp => cp.ProductId == productId);
        //    if (covenantProduct == null)
        //        return (false, "Product is not available in representative covenant.");

        //    if (covenantProduct.Amount < quantity)
        //        return (false, "Not enough quantity in representative covenant.");

        //    covenantProduct.Amount -= quantity;
        //    _unitOfWork.Repository<CovenantProduct>().Update(covenantProduct);

        //    await _unitOfWork.CompleteAsync();
        //    return (true, "OK");
        //}

        //private async Task CreateCommissionRecordAsync(int representativeId, int invoiceId, Product product, int quantity, DateTime invoiceDate)
        //{
        //    // commission per unit is product.CommissionRate (snapshot)
        //    var perUnit = product.CommissionRate;
        //    var total = Math.Round(perUnit * quantity, 2);

        //    var commission = new Commission
        //    {
        //        RepresentativeId = representativeId,
        //        InvoiceId = invoiceId,
        //        ProductId = product.Id,
        //        TotalCommission = total,
        //        CommissionAmount = total, // snapshot of gross before any deductions
        //        MonthDate = new DateTime(invoiceDate.Year, invoiceDate.Month, 1),
        //        DeductedAmount = 0m,
        //        IsDeducted = false,
        //        Note = "Created on invoice creation"
        //    };

        //    _unitOfWork.Repository<Commission>().Add(commission);
        //    await _unitOfWork.CompleteAsync();
        //}

        //private async Task GenerateMonthlyInstallmentsAsync(CustomerInvoice invoice, int months, decimal planAmount, DateTime startDate)
        //{
        //    if (months <= 0) return;

        //    // distribute planAmount across months: round to 2 decimals, put remainder in last month
        //    //decimal monthly = Math.Floor((planAmount / months) * 100) / 100m; // floor to 2 decimals
        //    //decimal sum = monthly * months;
        //    //decimal remainder = Math.Round(planAmount - sum, 2);

        //    for (int i = 0; i < months; i++)
        //    {
        //        //decimal amountThisMonth = monthly;
        //        //if (i == months - 1) amountThisMonth += remainder; // adjust last month
        //        var monthlyInstallment = new MonthlyInstallment
        //        {
        //            InvoiceId = invoice.Id,
        //            CustomerId = invoice.CustomerId,
        //            CollectorId = invoice.Customer.CollectorId,
        //            MonthDate = DateTime.Now.AddMonths(i),
        //            Amount = planAmount,
        //            CollectedAmount = 0m,
        //            IsDelayed = false
        //        };

        //        _unitOfWork.Repository<MonthlyInstallment>().Add(monthlyInstallment);
        //    }

        //    await _unitOfWork.CompleteAsync();
        //}
        //private async Task ReverseDiscountsForInvoice(CustomerInvoice invoice)
        //{
        //    var discountRepo = _unitOfWork.Repository<Discount>();
        //    var discounts = await discountRepo.GetAllAsync(d => d.Invoice);

        //    var invoiceDiscounts = discounts
        //        .Where(d => d.InvoiceId == invoice.Id && !d.IsDeleted)
        //        .ToList();

        //    await _unitOfWork.CompleteAsync();
        //    return (true, "OK");
        //}

        //private async Task CreateCommissionRecordAsync(int representativeId, int invoiceId, Product product, int quantity, DateTime invoiceDate, int customerInvoiceItemId)
        //{
        //    // commission per unit is product.CommissionRate (snapshot)
        //    var perUnit = product.CommissionRate;
        //    var total = Math.Round(perUnit * quantity, 2);

        //        private async Task DeductCommissionForInvoice(CustomerInvoice invoice, int representativeId)
        //{
        //    RepresentativeId = representativeId,
        //            InvoiceId = invoiceId,
        //            ProductId = product.Id,
        //            TotalCommission = total,
        //            CommissionAmount = total, // snapshot of gross before any deductions
        //            MonthDate = new DateTime(invoiceDate.Year, invoiceDate.Month, 1),
        //            DeductedAmount = 0m,
        //            InvoiceItemId = customerInvoiceItemId,
        //            IsDeducted = false,
        //            Note = "Created on invoice creation"
        //        }
        //;

        //_unitOfWork.Repository<Commission>().Add(commission);
        //await _unitOfWork.CompleteAsync();
        //    }

        //            var commission = commissions
        //                .FirstOrDefault(c => c.InvoiceId == invoice.Id && c.RepresentativeId == representativeId);

        //if (commission != null && !commission.IsDeducted)
        //{
        //    commission.DeductedAmount = commission.CommissionAmount * 0.10m; // Deduct 10% as penalty
        //    commission.CommissionAmount -= commission.DeductedAmount;
        //    commission.IsDeducted = true;
        //    commission.DeductedDate = DateTime.Now;
        //    commission.Note = "Commission deducted due to customer deletion.";
        //    commissionRepo.Update(commission);
        //}
        //        }

        //        #endregion

        //        #region Update Customer Invoice

        //        public async Task<ServiceResponse<bool>> UpdateCustomerInvoiceAsync(int invoiceId, UpdateCustomerInvoiceDTO dto)
        //{
        //    using var transaction = await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        // 1. Get existing invoice with all related data
        //        var existingInvoice = await _unitOfWork.Repository<CustomerInvoice>()
        //            .GetAllQueryable(
        //                i => i.Customer,
        //                i => i.Items,
        //                i => i.Installments
        //            //,
        //            //i => i.Items.Select(ii => ii.Product)
        //            )
        //            .FirstOrDefaultAsync(i => i.CustomerId == invoiceId && !i.IsDeleted);

        //        if (existingInvoice == null)
        //            return ServiceResponse<bool>.Failure("Invoice not found.");

        //        // 2. Validate related entities if they're being updated
        //        if (dto.CollectorId.HasValue)
        //        {
        //            var collector = await _unitOfWork.Repository<Collector>().GetByIdAsync(dto.CollectorId.Value);
        //            if (collector == null)
        //                return ServiceResponse<bool>.Failure("Collector not found.");
        //        }

        //        if (dto.RepresentativeId.HasValue)
        //        {
        //            var representative = await _unitOfWork.Repository<Representative>().GetByIdAsync(dto.RepresentativeId.Value);
        //            if (representative == null)
        //                return ServiceResponse<bool>.Failure("Representative not found.");
        //        }

        //        if (dto.SubAreaId.HasValue)
        //        {
        //            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(dto.SubAreaId.Value);
        //            if (subArea == null)
        //                return ServiceResponse<bool>.Failure("Sub-area not found.");
        //        }

        //        var customer = existingInvoice.Customer;
        //        if (customer != null)
        //        {
        //            if (dto.CustomerNumber != null)
        //                customer.CustomerNumber = (int)dto.CustomerNumber;

        //            if (!string.IsNullOrEmpty(dto.Name))
        //                customer.Name = dto.Name;

        //            if (!string.IsNullOrEmpty(dto.MobileNumber))
        //                customer.MobileNumber = dto.MobileNumber;

        //            if (!string.IsNullOrEmpty(dto.Address))
        //                customer.Address = dto.Address;

        //            if (!string.IsNullOrEmpty(dto.NationalNumber))
        //                customer.NationalNumber = dto.NationalNumber;

        //            if (dto.Deposit.HasValue)
        //                customer.Deposit = dto.Deposit.Value;

        //            if (dto.SaleDate.HasValue)
        //                customer.SaleDate = dto.SaleDate.Value;

        //            if (dto.FirstInvoiceDate.HasValue)
        //                customer.FirstInvoiceDate = dto.FirstInvoiceDate.Value;

        //            if (dto.SubAreaId.HasValue)
        //                customer.SubAreaId = dto.SubAreaId.Value;

        //            if (dto.CollectorId.HasValue)
        //                customer.CollectorId = dto.CollectorId.Value;

        //            if (dto.RepresentativeId.HasValue)
        //                customer.RepresentativeId = dto.RepresentativeId.Value;

        //            _unitOfWork.Repository<Customer>().Update(customer);
        //        }

        //        if (dto.UpdatedItems != null && dto.UpdatedItems.Any())
        //        {
        //            await UpdateInvoiceItemsAsync(existingInvoice, dto.UpdatedItems, dto.RepresentativeId ?? customer.RepresentativeId);
        //        }

        //        if (dto.UpdatedInstallments != null && dto.UpdatedInstallments.Any())
        //        {
        //            await UpdateInstallmentPlansAsync(existingInvoice, dto.UpdatedInstallments);
        //        }

        //        await RecalculateInvoiceTotalAsync(existingInvoice);

        //        await _unitOfWork.CompleteAsync();
        //        await transaction.CommitAsync();

        //        return ServiceResponse<bool>.SuccessResponse(true, "Customer invoice updated successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return ServiceResponse<bool>.Failure($"An error occurred while updating invoice: {ex.Message}");
        //    }
        //}

        //// Helper method to update invoice items
        //private async Task UpdateInvoiceItemsAsync(CustomerInvoice invoice, List<UpdateInvoiceItemDTO> updatedItems, int representativeId)
        //{
        //    var existingItems = invoice.Items?.ToList() ?? new List<CustomerInvoiceItems>();

        //    // Handle items to be removed
        //    var itemsToRemove = existingItems.Where(ei => !updatedItems.Any(ui => ui.Id == ei.Id)).ToList();
        //    foreach (var item in itemsToRemove)
        //    {
        //        await ReturnToCovenantAsync(representativeId, item.ProductId, item.Quantity);
        //        _unitOfWork.Repository<CustomerInvoiceItems>().Delete(item);

        //        // Remove related commission
        //        await RemoveCommissionRecordAsync(invoice.Id, item.ProductId);
        //    }

        //    // Handle items to be updated or added
        //    foreach (var updatedItem in updatedItems)
        //    {
        //        var existingItem = existingItems.FirstOrDefault(ei => ei.Id == updatedItem.Id);

        //        if (existingItem != null)
        //        {
        //            // Update existing item
        //            var quantityDifference = updatedItem.Quantity - existingItem.Quantity;

        //            if (quantityDifference != 0)
        //            {
        //                await AdjustCovenantAsync(representativeId, existingItem.ProductId, quantityDifference);
        //            }

        //            existingItem.Quantity = updatedItem.Quantity;
        //            existingItem.Price = updatedItem.Price;

        //            _unitOfWork.Repository<CustomerInvoiceItems>().Update(existingItem);

        //            // Update commission
        //            await UpdateCommissionRecordAsync(invoice.Id, existingItem.ProductId, updatedItem.Quantity);
        //        }
        //        else
        //        {
        //            // Add new item
        //            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updatedItem.ProductId);
        //            if (product == null)
        //                throw new Exception($"Product with ID {updatedItem.ProductId} not found.");

        //            var newItem = new CustomerInvoiceItems
        //            {
        //                InvoiceId = invoice.Id,
        //                ProductId = updatedItem.ProductId,
        //                Quantity = updatedItem.Quantity,
        //                Price = updatedItem.Price
        //            };

        //            _unitOfWork.Repository<CustomerInvoiceItems>().Add(newItem);

        //            // Deduct from covenant
        //            var covenantResult = await DeductFromCovenantAsync(representativeId, product.Id, updatedItem.Quantity);
        //            if (!covenantResult.Success)
        //                throw new Exception(covenantResult.Message);

        //            // Create commission record
        //            await CreateCommissionRecordAsync(representativeId, invoice.Id, product, updatedItem.Quantity, invoice.InvoiceDate);
        //        }
        //    }
        //}

        //// Helper method to update installment plans
        //private async Task UpdateInstallmentPlansAsync(CustomerInvoice invoice, List<UpdateInstallmentDTO> updatedInstallments)
        //{
        //    var existingPlans = invoice.Installments?.ToList() ?? new List<InstallmentPlan>();

        //    // Remove existing plans and monthly installments
        //    foreach (var plan in existingPlans)
        //    {
        //        // Remove related monthly installments
        //        var monthlyInstallments = await _unitOfWork.Repository<MonthlyInstallment>()
        //            .GetAllAsync(m => m.InvoiceId == invoice.Id);

        //        foreach (var monthly in monthlyInstallments)
        //        {
        //            _unitOfWork.Repository<MonthlyInstallment>().Delete(monthly);
        //        }

        //        _unitOfWork.Repository<InstallmentPlan>().Delete(plan);
        //    }

        //    // Add new plans and generate monthly installments
        //    foreach (var inst in updatedInstallments)
        //    {
        //        var newPlan = new InstallmentPlan
        //        {
        //            InvoiceId = invoice.Id,
        //            NumberOfMonths = inst.NumberOfMonths,
        //            Amount = inst.Amount
        //        };

        //        _unitOfWork.Repository<CustomerInvoiceItems>().Add(newItem);

        //        // Deduct from covenant
        //        var covenantResult = await DeductFromCovenantAsync(representativeId, product.Id, updatedItem.Quantity);
        //        if (!covenantResult.Success)
        //            throw new Exception(covenantResult.Message);

        //        // Create commission record
        //        await CreateCommissionRecordAsync(representativeId, invoice.Id, product, updatedItem.Quantity, invoice.InvoiceDate, newItem.Id);
        //    }
        //}

        //// Helper method to recalculate invoice total
        //private async Task RecalculateInvoiceTotalAsync(CustomerInvoice invoice)
        //{
        //    var items = await _unitOfWork.Repository<CustomerInvoiceItems>()
        //        .GetAllAsync(i => i.InvoiceId == invoice.Id);

        //    decimal itemsTotal = items.Sum(i => i.Quantity * i.Price);
        //    decimal deposit = invoice.Customer?.Deposit ?? 0;

        //    invoice.TotalAmount = itemsTotal;
        //    // Note: Net amount would be itemsTotal - deposit if you need it

        //    _unitOfWork.Repository<CustomerInvoice>().Update(invoice);
        //}

        //// Helper method to adjust covenant
        //private async Task AdjustCovenantAsync(int representativeId, int productId, int quantityDifference)
        //{
        //    if (quantityDifference > 0)
        //    {
        //        // Need to deduct more from covenant
        //        var result = await DeductFromCovenantAsync(representativeId, productId, quantityDifference);
        //        if (!result.Success)
        //            throw new Exception(result.Message);
        //    }
        //    else if (quantityDifference < 0)
        //    {
        //        // Return to covenant
        //        await ReturnToCovenantAsync(representativeId, productId, Math.Abs(quantityDifference));
        //    }
        //}

        //// Helper method to return products to covenant
        //private async Task ReturnToCovenantAsync(int representativeId, int productId, int quantity)
        //{
        //    var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        //    var covenant = await _unitOfWork.Repository<Covenant>()
        //        .GetAllQueryable()
        //        .Include(c => c.CovenantProducts)
        //        .FirstOrDefaultAsync(c => c.RepresentativeId == representativeId && c.MonthDate == currentMonth);

        //    if (covenant == null)
        //    {
        //        // Create new covenant if doesn't exist
        //        covenant = new Covenant
        //        {
        //            RepresentativeId = representativeId,
        //            CovenantDate = DateTime.UtcNow,
        //            MonthDate = currentMonth,
        //            CovenantType = "Return",
        //            CovenantProducts = new List<CovenantProduct>()
        //        };
        //        _unitOfWork.Repository<Covenant>().Add(covenant);
        //    }

        //    var covenantProduct = covenant.CovenantProducts.FirstOrDefault(cp => cp.ProductId == productId);
        //    if (covenantProduct != null)
        //    {
        //        covenantProduct.Amount += quantity;
        //        _unitOfWork.Repository<CovenantProduct>().Update(covenantProduct);
        //    }
        //    else
        //    {
        //        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        //        var newCovenantProduct = new CovenantProduct
        //        {
        //            CovenantId = covenant.Id,
        //            ProductId = productId,
        //            CategoryId = product?.CategoryId ?? 0,
        //            Amount = quantity
        //        };
        //        _unitOfWork.Repository<CovenantProduct>().Add(newCovenantProduct);
        //    }
        //}

        //// Helper method to update commission record
        //private async Task UpdateCommissionRecordAsync(int invoiceId, int productId, int newQuantity)
        //{
        //    var commission = await _unitOfWork.Repository<Commission>()
        //        .GetAllQueryable()
        //        .FirstOrDefaultAsync(c => c.InvoiceId == invoiceId && c.ProductId == productId);

        //    if (commission != null)
        //    {
        //        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        //        if (product != null)
        //        {
        //            commission.TotalCommission = Math.Round(product.CommissionRate * newQuantity, 2);
        //            commission.CommissionAmount = commission.TotalCommission;
        //            _unitOfWork.Repository<Commission>().Update(commission);
        //        }
        //    }
        //}

        //// Helper method to remove commission record
        //private async Task RemoveCommissionRecordAsync(int invoiceId, int productId)
        //{
        //    var commission = await _unitOfWork.Repository<Commission>()
        //        .GetAllQueryable()
        //        .FirstOrDefaultAsync(c => c.InvoiceId == invoiceId && c.ProductId == productId);

        //    if (commission != null)
        //    {
        //        _unitOfWork.Repository<Commission>().Delete(commission);
        //    }
        //}

        //        #endregion

        #endregion
    }
}