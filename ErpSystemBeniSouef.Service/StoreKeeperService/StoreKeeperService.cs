using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.StorekeeperResponseDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.StoreKeeperService
{
    public class StoreKeeperService : IStoreKeeperService
    {
        #region Properties Region

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StoreKeeperService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region  Create Region

        public StorekeeperResponseDto Create(CreateStorekeeperDto createDto)
        {
            try
            {
                var storekeeper = _mapper.Map<Storekeeper>(createDto);
                _unitOfWork.Repository<Storekeeper>().Add(storekeeper);
                _unitOfWork.Complete();


                return _mapper.Map<StorekeeperResponseDto>(storekeeper);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region  GetAllAsync Region

        public async Task<IReadOnlyList<StorekeeperResponseDto>> GetAllAsync()
        {
            var storekeeper = await _unitOfWork.Repository<Storekeeper>().GetAllAsync();
            var storekeeperDto = storekeeper.Select(sa => _mapper.Map<StorekeeperResponseDto>(sa)).ToList();

            return storekeeperDto;
        }

        #endregion

        #region  Soft Delete Region

        public bool SoftDelete(int id)
        {
            Storekeeper storekeeper = _unitOfWork.Repository<Storekeeper>().GetById(id);
            if (storekeeper == null)
                return false;
            try { storekeeper.IsDeleted = true; _unitOfWork.Complete(); return true; }
            catch { return false; }
        }

        #endregion

        #region Update Region

        public bool Update(UpdateStorekeeperDto updateDto)
        {
            Storekeeper storekeeper = _unitOfWork.Repository<Storekeeper>().GetById(updateDto.Id);
            if (storekeeper == null)
                return false;
            try
            {

                storekeeper.Name = updateDto.Name;
                storekeeper.Username = updateDto.Username;
                storekeeper.Password = updateDto.Password;
                _unitOfWork.Complete(); return true;
            }
            catch { return false; }
        }

        #endregion

    }
}
