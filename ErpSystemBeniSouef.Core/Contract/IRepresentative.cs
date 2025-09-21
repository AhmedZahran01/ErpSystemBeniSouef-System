using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface IRepresentativeService
    {
        #region sync Region
      Task<IReadOnlyList<RepresentativeDto>> GetAllAsync();

       // RepresentativeDto GetById(int id);

       // IReadOnlyList<RepresentativeDto> GetSubAreaDtoByMainAreaId(int RepresentativeId);

        RepresentativeDto Create(CreateRepresentativeDto createDto);

        bool Update(UpdateRepresentativeDto updateDto);

        bool SoftDelete(int id);
        #endregion


        #region Async Region
       // Task<RepresentativeDto> GetByIdAsync(int id);

      //  Task<IReadOnlyList<RepresentativeDto>> GetAllAsync();

       // Task<IReadOnlyList<RepresentativeDto>> GetSubAreaDtoByMainAreaIdAsync(int mainAreaId);

       // Task<RepresentativeDto> CreateAsync(CreateRepresentativeDto createDto);

      //  Task<RepresentativeDto> UpdateAsync(UpdateRepresentativeDto updateDto);

       // Task<bool> SoftDeleteAsync(int id);
        #endregion

    }
}
