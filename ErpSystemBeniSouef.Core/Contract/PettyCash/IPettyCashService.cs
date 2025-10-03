using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.PettyCash;

namespace ErpSystemBeniSouef.Core.Contract.PettyCash
{
    public interface IPettyCashService
    {
        Task<bool> AddPettyCash(AddPettyCashDto dto);
        Task<bool> DeletePettyCash(int id);
        Task<List<ReturnPettyCashDto>> GetAllPettyCash();
    }
}
