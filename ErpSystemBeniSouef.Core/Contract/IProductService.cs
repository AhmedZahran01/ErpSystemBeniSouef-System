using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto; 
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface IProductService
    {
        #region Get All Region

        IReadOnlyList<ProductDto> GetAll(); 

        Task<IReadOnlyList<ProductDto>> GetAllAsync();

        #endregion

        #region Get By Id Region

        Task<ProductDto> GetByIdAsync(int id);

        ProductDto GetById(int id);
       
        #endregion

        #region Get Products By Ctegory Id Region
         
        IReadOnlyList<ProductDto> GetProductsByCategoryId(int categoryId);
        Task<IReadOnlyList<ProductDto>> GetProductsByCategoryIdAsync(int categoryId);
        #endregion

        #region Create Product Region
        bool Create(CreateProductDto createDto); 
        Task<ProductDto> CreateAsync(CreateProductDto createDto);

        #endregion

        #region Update Region
        ProductDto Update(UpdateProductDto updateDto);
        Task<ProductDto> UpdateAsync(UpdateProductDto updateDto);

        #endregion
         
        #region Soft Delete Region
        bool SoftDelete(int id);
        Task<bool> SoftDeleteAsync(int id);
        #endregion

        #region Calculate Profit Margin Region
        decimal CalculateProfitMargin(int id);
        Task<decimal> CalculateProfitMarginAsync(int id);

        #endregion
         
        #region Get All Categories Region

        IReadOnlyList<CategoryDto> GetAllCategories();

        Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync();

        #endregion

    }
}
