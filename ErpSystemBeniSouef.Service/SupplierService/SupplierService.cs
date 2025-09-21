using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.SupplierService
{
    public class SupplierService : ISupplierService
    {

        #region Constractor Region
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region  GetAll Supplier  
        public IReadOnlyList<SupplierDto> GetAll()
        {
            var supplier =  _unitOfWork.Repository<Supplier>().GetAll();
            var supplierDto = supplier.Select(sa => _mapper.Map<SupplierDto>(sa)).ToList();

            return supplierDto;
        }
        
        public async Task<IReadOnlyList<SupplierDto>> GetAllAsync()
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetAllAsync();
            var supplierDto = supplier.Select(sa => _mapper.Map<SupplierDto>(sa)).ToList();

            return supplierDto;
        }

        #endregion

        #region  Create Region

        public SupplierDto Create(CreateSupplierDto createDto)
        {
            try
            {
                var supplier = _mapper.Map<Supplier>(createDto);
                _unitOfWork.Repository<Supplier>().Add(supplier);
                _unitOfWork.Complete();


                return _mapper.Map<SupplierDto>(supplier);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region My  softDeleted
        public bool SoftDelete(int id)
        {
            Supplier supplier = _unitOfWork.Repository<Supplier>().GetById(id);
            if (supplier == null)
                return false;
            try { supplier.IsDeleted = true; _unitOfWork.Complete(); return true; }
            catch { return false; }
        }
        #endregion







        #region Update Region

        public Supplier Update(int updateSupplierId , string newName)
        {
            Supplier supplier = _unitOfWork.Repository<Supplier>().GetById(updateSupplierId);
            if (supplier == null)
                return null;
            try
            { 
                supplier.Name = newName;
                _unitOfWork.Complete(); return supplier;
            }
            catch { return null; }
        }

        #endregion






      



         

        /*
         public  IReadOnlyList<SupplierDto> GetAll()
         {
             var supplier = _unitOfWork.Repository<Supplier>().GetAll();
             IReadOnlyList<SupplierDto> mainAreaDto = _mapper.Map<IReadOnlyList<SupplierDto>>(supplier);

             return mainAreaDto;
         }
         */

        /*
        public SupplierDto GetById(int id)
        {
            throw new NotImplementedException();
        }
        */
        /*
        public IReadOnlyList<SupplierDto> GetSubAreaDtoByMainAreaId(int SupplierId)
        {
            throw new NotImplementedException();
        }
        */
    }
}
