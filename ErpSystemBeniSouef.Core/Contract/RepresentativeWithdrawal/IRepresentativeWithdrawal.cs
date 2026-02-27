using ErpSystemBeniSouef.Core.DTOs.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeWithdrawalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Contract.RepresentativeWithdrawal
{
    public interface IRepresentativeWithdrawalService
    {
        Task<bool> AddRepresentativeWithdrawal(AddRepresentativeWithdrawalDto dto);
        Task<bool> DeleteRepresentativeWithdrawal(int id);
        Task<List<ReturnRepresentativeWithdrawalDto>> GetAllRepresentativeWithdrawal();
    }
}
