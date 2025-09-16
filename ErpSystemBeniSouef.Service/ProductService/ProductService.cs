using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.Entities;

namespace ErpSystemBeniSouef.Service.ProductService
{
    public class ProductService : IProductService
    {
        #region Constractor Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get All Region 

        public IReadOnlyList<ProductDto> GetAll()
        {
            var products = _unitOfWork.Repository<Product>().GetAll();

            var response = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            foreach (var productDto in response)
            {
                productDto.ProfitMargin = CalculateProfitMargin(productDto.Id);
            }

            return response;
        }

        public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync(m => m.Category);

            var response = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            foreach (var productDto in response)
            {
                productDto.ProfitMargin = await CalculateProfitMarginAsync(productDto.Id);
            }

            return response;
        }

        #endregion

        #region Get By Id  Region

        public ProductDto GetById(int id)
        {
            var product = _unitOfWork.Repository<Product>().GetById(id);

            if (product == null)
                return null;

            var response = _mapper.Map<ProductDto>(product);
            response.ProfitMargin = CalculateProfitMargin(id);
            return response;

        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            if (product == null)
                return null;
            var response = _mapper.Map<ProductDto>(product);
            response.ProfitMargin = await CalculateProfitMarginAsync(id);
            return response;
        }

        #endregion

        #region GetByCategoryIdAsync Region

        public IReadOnlyList<ProductDto> GetProductsByCategoryId(int categoryId)
        {
            var category = _unitOfWork.Repository<Category>().GetById(categoryId);
            if (category == null)
                return null;
            ;
            var products = _unitOfWork.Repository<Product>().GetAll().Where(p => p.CategoryId == categoryId);

            var response = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return response;

        }

        public async Task<IReadOnlyList<ProductDto>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
            if (category == null)
                return null;

            var products = await _unitOfWork.Repository<Product>().GetAllAsync(p => p.CategoryId == categoryId);

            var response = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return response;
        }

        #endregion

        #region Create Region

        public bool Create(CreateProductDto createDto)
        {
            var category = _unitOfWork.Repository<Category>().GetById(createDto.CategoryId);
            if (category == null)
                return false;


            var product = _mapper.Map<Product>(createDto);
            var productD = _mapper.Map<ProductDto>(createDto);
            _unitOfWork.Repository<Product>().Add(product);
            _unitOfWork.CompleteAsync();


            return true;
        }
        public async Task<ProductDto> CreateAsync(CreateProductDto createDto)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(createDto.CategoryId);
            if (category == null)
                return null;

            if (createDto.SalePrice <= createDto.PurchasePrice)
                return null;

            var product = _mapper.Map<Product>(createDto);
            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<ProductDto>(product);
            response.CategoryName = category.Name;
            response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);

            return response;
        }

        #endregion

        #region Update Region

        public bool Update(UpdateProductDto updateDto)
        {
            var product = _unitOfWork.Repository<Product>().GetById(updateDto.Id);
            if (product == null)
                return false;

            if (product.CategoryId != updateDto.CategoryId)
            {
                var category = _unitOfWork.Repository<Category>().GetById(updateDto.CategoryId);
                if (category == null)
                    return false;
            }

            _mapper.Map(updateDto, product);
            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Product>().Update(product);
            _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<ProductDto> UpdateAsync(UpdateProductDto updateDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updateDto.Id);
            if (product == null)
                return null;

            if (product.CategoryId != updateDto.CategoryId)
            {
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateDto.CategoryId);
                if (category == null)
                    return null;
            }

            if (updateDto.SalePrice <= updateDto.PurchasePrice)
                return null;

            _mapper.Map(updateDto, product);
            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();

            var updatedProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(product.Id);
            var response = _mapper.Map<ProductDto>(updatedProduct);
            response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);

            return response;
        }

        #endregion

        #region Soft Delete Region

        public bool SoftDelete(int id)
        {
            var product = _unitOfWork.Repository<Product>().GetById(id);
            if (product == null)
                return false;
            product.IsDeleted = true;
            _unitOfWork.Repository<Product>().Update(product);
            _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
                return false;

            product.IsDeleted = true;
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        #endregion

        #region CalculateProfitMarginAsync Region

        public decimal CalculateProfitMargin(int id)
        {
            var product = _unitOfWork.Repository<Product>().GetById(id);
            if (product == null)
                return 0;

            var profit = product.SalePrice - product.PurchasePrice;
            var profitMargin = (profit / product.SalePrice) * 100;
            return Math.Round(profitMargin, 2);
        }

        public async Task<decimal> CalculateProfitMarginAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
                return 0;

            var profit = product.SalePrice - product.PurchasePrice;
            var profitMargin = (profit / product.SalePrice) * 100;
            return Math.Round(profitMargin, 2);
        }

        #endregion


        #region Get All Region 

        public IReadOnlyList<CategoryDto> GetAllCategories()
        {
            var categories = _unitOfWork.Repository<Category>().GetAll();

            var response = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);

            return response;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();

            var response = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);

            return response;
        }

        #endregion



        //=================== Comment ASYNC METHIDS=========================

        #region Comment Async Region


        //#region Get By Id  Region

        //public async Task<ProductDto> GetById(int id)
        //{
        //    var product = _unitOfWork.Repository<Product>().GetByIdAsync(id);

        //    if (product == null)
        //        return null;

        //    var response = _mapper.Map<ProductDto>(product);
        //    response.ProfitMargin = await CalculateProfitMarginAsync(id);
        //    return response;

        //}

        //#endregion

        //#region Get All Region

        //public async Task<IReadOnlyList<ProductDto>> GetAll()
        //{
        //    var products = _unitOfWork.Repository<Product>().GetAll();

        //    var response = _mapper.Map<IReadOnlyList<ProductDto>>(products);

        //    foreach (var productDto in response)
        //    {
        //        productDto.ProfitMargin = await CalculateProfitMarginAsync(productDto.Id);
        //    }

        //    return response;
        //}

        //#endregion

        //#region GetByCategoryIdAsync Region

        //public async Task<IReadOnlyList<ProductDto>> GetByCategoryId(int categoryId)
        //{
        //    var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
        //    if (category == null)
        //        return null;
        //    ;
        //    var products = _unitOfWork.Repository<Product>().GetAll();

        //    var response = _mapper.Map<IReadOnlyList<ProductDto>>(products);
        //    return response;

        //}

        //#endregion

        //#region Create Region

        //public async Task<ProductDto> Create(CreateProductDto createDto)
        //{
        //    var category = await _unitOfWork.Repository<Category>().GetByIdAsync(createDto.CategoryId);
        //    if (category == null)
        //        return null;

        //    if (createDto.SalePrice <= createDto.PurchasePrice)
        //        return null;

        //    var product = _mapper.Map<Product>(createDto);
        //    _unitOfWork.Repository<Product>().Add(product);
        //    await _unitOfWork.CompleteAsync();

        //    var response = _mapper.Map<ProductDto>(product);
        //    response.CategoryName = category.Name;
        //    response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);

        //    return response;
        //}

        //#endregion

        //#region Update Region

        //public async Task<ProductDto> Update(UpdateProductDto updateDto)
        //{
        //    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updateDto.Id);
        //    if (product == null)
        //        return null;

        //    if (product.CategoryId != updateDto.CategoryId)
        //    {
        //        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateDto.CategoryId);
        //        if (category == null)
        //            return null;
        //    }

        //    if (updateDto.SalePrice <= updateDto.PurchasePrice)
        //        return null;

        //    _mapper.Map(updateDto, product);
        //    product.UpdatedDate = DateTime.UtcNow;

        //    _unitOfWork.Repository<Product>().Update(product);
        //    await _unitOfWork.CompleteAsync();

        //    var updatedProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(product.Id);
        //    var response = _mapper.Map<ProductDto>(updatedProduct);
        //    response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);

        //    return response;
        //}

        //#endregion


        //#region CalculateProfitMarginAsync Region

        //public async Task<decimal> CalculateProfitMarginAsync(int id)
        //{
        //    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        //    if (product == null)
        //        return 0;

        //    var profit = product.SalePrice - product.PurchasePrice;
        //    var profitMargin = (profit / product.SalePrice) * 100;
        //    return Math.Round(profitMargin, 2);
        //}

        //#endregion 
        #endregion

        #region MyRegion

        //public ProductDto GetByIdWithDetailsAsync(int id)
        //{
        //    var spec = new ProductSpecs(id);
        //    var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

        //    if (product == null)
        //        return ApiResponse<ProductDetailDto>.ErrorResponse($"Product with Id {id} not found.");

        //    var response = _mapper.Map<ProductDetailDto>(product);
        //    response.ProfitMargin = await CalculateProfitMarginAsync(id);
        //    return ApiResponse<ProductDetailDto>.SuccessResponse(response, "Product with details retrieved successfully.");
        //}

        #endregion

    }
}
