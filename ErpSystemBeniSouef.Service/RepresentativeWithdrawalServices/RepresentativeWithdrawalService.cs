using ErpSystemBeniSouef.Core.Contract.PettyCash;
using ErpSystemBeniSouef.Core.Contract.RepresentativeWithdrawal;
using ErpSystemBeniSouef.Core.DTOs.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeWithdrawalDtos;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.RepresentativeWithdrawalServices
{
    public class RepresentativeWithdrawalService : IRepresentativeWithdrawalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepresentativeWithdrawalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddRepresentativeWithdrawal(AddRepresentativeWithdrawalDto dto)
        {
            try
            {
                var pettyCash = new RepresentativeWithdrawal
                {
                    Date = dto.Date,
                    Reason = dto.Reason,
                    Amount = dto.Amount,
                    representativeId = dto.representativeId,
                };

                _unitOfWork.Repository<RepresentativeWithdrawal>().Add(pettyCash);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRepresentativeWithdrawal(int id)
        {
            try
            {
                var pettyCash = await _unitOfWork.Repository<RepresentativeWithdrawal>().GetByIdAsync(id);
                if (pettyCash == null) return false;

                _unitOfWork.Repository<RepresentativeWithdrawal>().Delete(pettyCash);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ReturnRepresentativeWithdrawalDto>> GetAllRepresentativeWithdrawal()
        {
            var representativeWithdrawals = await _unitOfWork.Repository<RepresentativeWithdrawal>().GetAllAsync(r => r.representative);

            return representativeWithdrawals.Select(pc => new ReturnRepresentativeWithdrawalDto
            {
                Id = pc.Id,
                Date = pc.Date,
                Reason = pc.Reason,
                Amount = pc.Amount,
                representative = pc.representative,
                representativeName = pc.representative.Name,
            }).ToList();
        }
    
    }
}
