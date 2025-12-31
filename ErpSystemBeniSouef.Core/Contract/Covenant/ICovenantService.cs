using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.Covenant;

namespace ErpSystemBeniSouef.Core.Contract.Covenant
{
    public interface ICovenantService
    {
        Task<bool> addCovenant(AddCovenantToRepresentative dto);
        Task<List<ReturnCovenant>> GetAllCovenants();
        Task<ReturnCovenant> GetCovenantById(int Id);
        Task<List<ReturnCovenantItem>> GetCovenantItemsByCovenantId(int covenantId);
        bool addCovenantItems(AddCovenantItemsDto dto);
        Task<bool> DeleteCovenant(int covenantId);
        Task<bool> DeleteCovenantItem(int covenantItemId);


    }
}
