using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto; 
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);
        ProductDto GetById(int id);


        Task<IReadOnlyList<ProductDto>> GetAllAsync();
        IReadOnlyList<ProductDto> GetAll();

        Task<IReadOnlyList<ProductDto>> GetByCategoryId(int categoryId);

        Task<ProductDto> Create(CreateProductDto createDto);
         
        Task<ProductDto> Update(UpdateProductDto updateDto);
      
        Task<bool> SoftDelete(int id);

        decimal CalculateProfitMargin(int id);
        Task<decimal> CalculateProfitMarginAsync(int id);


        //Task<ApiResponse<ProductDetailDto>> GetByIdWithDetailsAsync(int id);
    }
}
