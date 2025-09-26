using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface ISupplierService
    {
        #region sync Region
       // IReadOnlyList<SupplierDto> GetAll();

        IReadOnlyList<SupplierRDto> GetAll();

        //  SupplierDto GetById(int id);
        /*
        IReadOnlyList<SupplierDto> GetSubAreaDtoByMainAreaId(int SupplierId);
        */
        SupplierRDto Create(CreateSupplierDto createDto);

        Supplier Update(int updateSupplierId , string newName);

        bool SoftDelete(int id);
        #endregion


        #region Async Region
        //   Task<SupplierDto> GetByIdAsync(int id);

        Task<IReadOnlyList<SupplierRDto>> GetAllAsync();

        // Task<IReadOnlyList<SupplierDto>> GetSubAreaDtoByMainAreaIdAsync(int StorekeeperId);

        //Task<SupplierDto> CreateAsync(CreateSupplierDto createDto);

        // Task<SupplierDto> UpdateAsync(UpdateSupplierDto updateDto);

        // Task<bool> SoftDeleteAsync(int id);
        #endregion
    }
}
