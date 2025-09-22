using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.MainAreaServices
{
    public class MainAreaService : IMainAreaService
    {
        #region Constrctor and properties Region
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MainAreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region  Get All Region
        public IReadOnlyList<MainAreaDto> GetAll()
        {
            var mainAreas = _unitOfWork.Repository<MainArea>().GetAll();
            IReadOnlyList<MainAreaDto> mainAreaDto = _mapper.Map<IReadOnlyList<MainAreaDto>>(mainAreas);

            return mainAreaDto;
        }
        public async Task<IReadOnlyList<MainAreaDto>> GetAllAsync()
        {
            var mainAreas = await _unitOfWork.Repository<MainArea>().GetAllAsync();
            IReadOnlyList<MainAreaDto> mainAreaDto = _mapper.Map<IReadOnlyList<MainAreaDto>>(mainAreas);

            return mainAreaDto;
        }

        #endregion

        #region  Create Region
        public int Create(CreateMainAreaDto createDto)
        {
            try
            {
                var mainArea = _mapper.Map<MainArea>(createDto);
                _unitOfWork.Repository<MainArea>().Add(mainArea);
                _unitOfWork.CompleteAsync();
                return 1;
            }
            catch
            {
                return 0;
            }

        }
        #endregion

        #region Soft Delete Region Region

        public bool SoftDelete(int id)
        {
            MainArea mainArea = _unitOfWork.Repository<MainArea>().GetById(id);
            if (mainArea == null)
                return false;
            try { mainArea.IsDeleted = true; _unitOfWork.Complete(); return true; }
            catch { return false; }

        }

        #endregion

        #region Update Region Region

        public bool Update(UpdateMainAreaDto updateMainAreaDto)
        {
            MainArea mainArea = _unitOfWork.Repository<MainArea>().GetById(updateMainAreaDto.Id);
            if (mainArea == null)
                return false;
            try
            {
                mainArea.StartNumbering = updateMainAreaDto.StartNumbering;
                mainArea.Name = updateMainAreaDto.Name;
                _unitOfWork.Complete(); return true;
            }
            catch { return false; }

        }

        #endregion
          
    }
}
