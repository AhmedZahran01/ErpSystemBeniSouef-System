using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.Entities;

namespace ErpSystemBeniSouef.Service.SubAreaServices
{
    public class SubAreaService : ISubAreaService
    {
        #region Constrctor and properties Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubAreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region  Get All Async Region
        public async Task<IReadOnlyList<SubAreaDto>> GetAllAsync()
        {
            var subAreas = await _unitOfWork.Repository<SubArea>().GetAllAsync(s => s.mainRegions);
            var subAreasDto = subAreas.Select(sa => _mapper.Map<SubAreaDto>(sa)).ToList();

            return subAreasDto;
        }

        #endregion

        #region  GetByIdAsync Region
        public async Task<SubAreaDto> GetByIdAsync(int id)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(id);
            if (subArea == null)
                return null;

            var subAreaDto = _mapper.Map<SubAreaDto>(subArea);
            return subAreaDto;
        }

        #endregion

        #region Get SubAreaDto By MainArea Id Async Region

        public async Task<IReadOnlyList<SubAreaDto>> GetSubAreaDtoByMainAreaIdAsync(int mainAreaId)
        {
            var subAreas = await _unitOfWork.Repository<SubArea>().GetAllAsync(P => P.MainAreaId == mainAreaId);
            var subAreasDto = subAreas.Select(sa => _mapper.Map<SubAreaDto>(sa)).ToList();
            return subAreasDto;
        }

        #endregion

        #region  Create Region

        public async Task<SubAreaDto> CreateAsync(CreateSubAreaDto createDto)
        {
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(createDto.MainAreaId);
            if (mainArea == null)
                return null;

            var subArea = _mapper.Map<SubArea>(createDto);
            _unitOfWork.Repository<SubArea>().Add(subArea);
            await _unitOfWork.CompleteAsync();

            var subAreaDto = _mapper.Map<SubAreaDto>(subArea);
            subAreaDto.MainAreaName = mainArea.Name;
            return subAreaDto;
        }

        #endregion

        #region Update Region Region

        public async Task<SubAreaDto> UpdateAsync(UpdateSubAreaDto updateDto)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(updateDto.Id);
            if (subArea == null)
                return null;

            if (subArea.MainAreaId != updateDto.MainAreaId)
            {
                var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(updateDto.MainAreaId);
                if (mainArea == null)
                    return null;
            }

            _mapper.Map(updateDto, subArea);
            subArea.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<SubArea>().Update(subArea);
            await _unitOfWork.CompleteAsync();

            var updatedSubArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(subArea.Id);
            var subAreaDto = _mapper.Map<SubAreaDto>(updatedSubArea);

            return subAreaDto;
        }

        #endregion

        #region Soft Delete Region Region

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(id);
            if (subArea == null)
                return false;
            _unitOfWork.Repository<SubArea>().Delete(subArea);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        #endregion

        //=====================================================


        #region  Get All Async Region
        public IReadOnlyList<SubAreaDto> GetAll()
        {
            var subAreas = _unitOfWork.Repository<SubArea>().GetAll(s =>s.mainRegions );
            var subAreasDto = subAreas.Select(sa => _mapper.Map<SubAreaDto>(sa)).ToList();

            return subAreasDto;
        }

        #endregion

        #region  GetById Region
        public SubAreaDto GetById(int id)
        {
            var subArea = _unitOfWork.Repository<SubArea>().GetByIdAsync(id);

            if (subArea == null)
                return null;

            var subAreaDto = _mapper.Map<SubAreaDto>(subArea);
            return subAreaDto;
        }

        #endregion

        #region Get SubAreaDto By MainArea Id Async Region

        public IReadOnlyList<SubAreaDto> GetSubAreaDtoByMainAreaId(int mainAreaId)
        {
            var subAreas = _unitOfWork.Repository<SubArea>().GetAll(P => P.MainAreaId == mainAreaId);

            var subAreasDto = subAreas.Select(sa => _mapper.Map<SubAreaDto>(sa)).ToList();
            return subAreasDto;
        }

        #endregion

        #region  Create Region

        public SubAreaDto Create(CreateSubAreaDto createDto)
        {
            var mainArea = _unitOfWork.Repository<MainArea>().GetById(createDto.MainAreaId);
            if (mainArea == null)
                return null;

            var subArea = _mapper.Map<SubArea>(createDto);
            _unitOfWork.Repository<SubArea>().Add(subArea);
            _unitOfWork.Complete();

            var subAreaDto = _mapper.Map<SubAreaDto>(subArea);
            subAreaDto.MainAreaName = mainArea.Name;

            return subAreaDto;
        }

        #endregion

        #region Update Region Region

        public SubAreaDto Update(UpdateSubAreaDto updateDto)
        {
            var subArea = _unitOfWork.Repository<SubArea>().GetById(updateDto.Id);
            if (subArea == null)
                return null;

            if (subArea.MainAreaId != updateDto.MainAreaId)
            {
                var mainArea = _unitOfWork.Repository<MainArea>().GetById(updateDto.MainAreaId);
                if (mainArea == null)
                    return null;
            }

            _mapper.Map(updateDto, subArea);
            subArea.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<SubArea>().Update(subArea);
            _unitOfWork.Complete();

            var updatedSubArea = _unitOfWork.Repository<SubArea>().GetById(subArea.Id);
            var subAreaDto = _mapper.Map<SubAreaDto>(updatedSubArea);

            return subAreaDto;
        }

        #endregion

        #region Soft Delete Region Region

        public bool SoftDelete(int id)
        {
            var subArea = _unitOfWork.Repository<SubArea>().GetById(id);
            if (subArea == null)
                return false;

            _unitOfWork.Repository<SubArea>().Delete(subArea);
            _unitOfWork.Complete();

            return true;
        }

        #endregion


    }
}
