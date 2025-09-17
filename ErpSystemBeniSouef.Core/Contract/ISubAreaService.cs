using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface ISubAreaService
    {

        #region sync Region
        IReadOnlyList<SubAreaDto> GetAll();

        SubAreaDto GetById(int id);

        IReadOnlyList<SubAreaDto> GetSubAreaDtoByMainAreaId(int mainAreaId);

        SubAreaDto Create(CreateSubAreaDto createDto);

        SubAreaDto Update(UpdateSubAreaDto updateDto);

        bool SoftDelete(int id);
        #endregion


        #region Async Region
        Task<SubAreaDto> GetByIdAsync(int id);

        Task<IReadOnlyList<SubAreaDto>> GetAllAsync();

        Task<IReadOnlyList<SubAreaDto>> GetSubAreaDtoByMainAreaIdAsync(int mainAreaId);

        Task<SubAreaDto> CreateAsync(CreateSubAreaDto createDto);

        Task<SubAreaDto> UpdateAsync(UpdateSubAreaDto updateDto);

        Task<bool> SoftDeleteAsync(int id);
        #endregion


    }
}
