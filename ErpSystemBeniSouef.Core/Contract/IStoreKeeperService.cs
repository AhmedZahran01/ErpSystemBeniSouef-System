using ErpSystemBeniSouef.Core.DTOs.RepresentativeDto;
using ErpSystemBeniSouef.Core.DTOs.StorekeeperResponseDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface IStoreKeeperService
    {
        #region sync Region
       Task <IReadOnlyList<StorekeeperResponseDto>> GetAllAsync();

       // StorekeeperResponseDto GetById(int id);
        /*
        IReadOnlyList<StorekeeperResponseDto> GetSubAreaDtoByMainAreaId(int StorekeeperId);
        */
        StorekeeperResponseDto Create(CreateStorekeeperDto createDto);

        bool Update(UpdateStorekeeperDto updateDto);

        bool SoftDelete(int id);
        #endregion


        #region Async Region
      //  Task<StorekeeperResponseDto> GetByIdAsync(int id);

       // Task<IReadOnlyList<StorekeeperResponseDto>> GetAllAsync();

       // Task<IReadOnlyList<StorekeeperResponseDto>> GetSubAreaDtoByMainAreaIdAsync(int StorekeeperId);

       // Task<StorekeeperResponseDto> CreateAsync(CreateStorekeeperDto createDto);

       // Task<StorekeeperResponseDto> UpdateAsync(UpdateStorekeeperDto updateDto);

       // Task<bool> SoftDeleteAsync(int id);
        #endregion

    }
}
