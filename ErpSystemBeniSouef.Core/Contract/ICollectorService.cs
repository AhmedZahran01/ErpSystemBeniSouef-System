using ErpSystemBeniSouef.Core.DTOs.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface ICollectorService
    {

        #region sync Region
        Task<IReadOnlyList<CollectorDto>> GetAllAsync();

        //CollectorDto GetById(int id);

        /*IReadOnlyList<CollectorDto> GetSubAreaDtoByMainAreaId(int CollectorId);*/

        CollectorDto Create(CreateCollectorDto createDto);

        bool Update(UpdateCollectorDto updateDto);

        bool SoftDelete(int id);
        #endregion


        #region Async Region
        // Task<CollectorDto> GetByIdAsync(int id);

        // Task<IReadOnlyList<CollectorDto>> GetAllAsync();

        // Task<IReadOnlyList<CollectorDto>> GetSubAreaDtoByMainAreaIdAsync(int CollectorId);

        // Task<CollectorDto> CreateAsync(CreateCollectorDto createDto);

        // Task<CollectorDto> UpdateAsync(UpdateCollectorDto updateDto);

        // Task<bool> SoftDeleteAsync(int id);
        #endregion
    }
}
