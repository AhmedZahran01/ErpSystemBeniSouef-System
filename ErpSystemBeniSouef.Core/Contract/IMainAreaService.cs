using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface IMainAreaService
    { 
        Task<IReadOnlyList<MainAreaResponseDto>> GetAllAsync();

        // Task<MainAreaResponseDto> GetByIdAsync(int id);

        //Task<ApiResponse<MainAreaResponseDto>> CreateAsync(CreateMainAreaDto createDto);

        //Task<MainAreaResponseDto> UpdateAsync(UpdateMainAreaDto updateDto);

        //Task<bool> SoftDeleteAsync(int id);
    }
}
