using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Contract.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.PettyCash;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core;

namespace ErpSystemBeniSouef.Service.PettyCashServices
{
    public class PettyCashService : IPettyCashService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PettyCashService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddPettyCash(AddPettyCashDto dto)
        {
            try
            {
                var pettyCash = new PettyCash
                {
                    Date = dto.Date,
                    Reason = dto.Reason,
                    Amount = dto.Amount
                };

                _unitOfWork.Repository<PettyCash>().Add(pettyCash);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeletePettyCash(int id)
        {
            try
            {
                var pettyCash = await _unitOfWork.Repository<PettyCash>().GetByIdAsync(id);
                if (pettyCash == null) return false;

                _unitOfWork.Repository<PettyCash>().Delete(pettyCash);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ReturnPettyCashDto>> GetAllPettyCash()
        {
            var pettyCashList = await _unitOfWork.Repository<PettyCash>().GetAllAsync();

            return pettyCashList.Select(pc => new ReturnPettyCashDto
            {
                Id = pc.Id,
                Date = pc.Date,
                Reason = pc.Reason,
                Amount = pc.Amount
            }).ToList();
        }
    }
}
