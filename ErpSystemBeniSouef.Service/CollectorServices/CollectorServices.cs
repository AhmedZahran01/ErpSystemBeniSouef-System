using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.Collector;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.CollectorServices
{
    public class CollectorServices : ICollectorService

    { 
        #region Constractor Region
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CollectorServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region  GetAll Region  

        public async Task<IReadOnlyList<CollectorDto>> GetAllAsync()
        {
            var supplier = await _unitOfWork.Repository<Collector>().GetAllAsync();
            var supplierDto = supplier.Select(sa => _mapper.Map<CollectorDto>(sa)).ToList();

            return supplierDto;
        }

        #endregion

        #region Create Region

        public CollectorDto Create(CreateCollectorDto createDto)
        {
            try
            {
                var collector = _mapper.Map<Collector>(createDto);
                _unitOfWork.Repository<Collector>().Add(collector);
                _unitOfWork.Complete();

                return _mapper.Map<CollectorDto>(collector);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Soft Delete Region

        public bool SoftDelete(int id)
        {
            Collector collector = _unitOfWork.Repository<Collector>().GetById(id);
            if (collector == null)
                return false;
            try { collector.IsDeleted = true; _unitOfWork.Complete(); return true; }
            catch { return false; }
        }

        #endregion

        #region Update Region

        public bool Update(UpdateCollectorDto updateDto)
        {
            Collector collector = _unitOfWork.Repository<Collector>().GetById(updateDto.Id);
            if (collector == null)
                return false;
            try
            {

                collector.Name = updateDto.Name;
                _unitOfWork.Complete(); return true;
            }
            catch { return false; }
        }

        #endregion

      
    }
}
