using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract
{
    public interface IMainAreaService
    { 
        IReadOnlyList<MainAreaDto> GetAll();
        Task<IReadOnlyList<MainAreaDto>> GetAllAsync();

        int Create(CreateMainAreaDto createDto);

        bool SoftDelete(int id);

        // Task<MainAreaResponseDto> GetByIdAsync(int id);

        //int Update(MainArea updateDto);

    }
}
