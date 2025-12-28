using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract.Covenant;
using ErpSystemBeniSouef.Core.DTOs.Covenant;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;

namespace ErpSystemBeniSouef.Service.CovenantServices
{
    public class CovenantService : ICovenantService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CovenantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  bool addCovenant(AddCovenantToRepresentative dto)
        {
            try
            {
                var covenant = new Covenant
                {
                    CovenantDate = dto.CovenantDate,
                    MonthDate = dto.MonthDate,
                    RepresentativeId = dto.RepresentativeId,
                    CovenantType = dto.CovenantType
                };

                _unitOfWork.Repository<Covenant>().Add(covenant);
                _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public  bool addCovenantItems(AddCovenantItemsDto dto)
        {
            try
            {
                var covenant =  _unitOfWork.Repository<Covenant>().GetByIdAsync(dto.CovenantId);
                if (covenant == null) 
                    return false;

                foreach (var item in dto.CovenantItems)
                {
                    var covenantItem = new CovenantProduct
                    {
                        CovenantId = dto.CovenantId,
                        ProductId = item.ProductId,
                        CategoryId = item.CategoryId,
                        Amount = item.Amount
                    };

                    _unitOfWork.Repository<CovenantProduct>().Add(covenantItem);
                }

               _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ReturnCovenant>> GetAllCovenants()
        {
            var covenants = await _unitOfWork.Repository<Covenant>().GetAllAsync();

            var result = covenants.Select(c => new ReturnCovenant
            {
                Id = c.Id,
                CovenantType = c.CovenantType,
                CovenantDate = c.CovenantDate
            }).ToList();

            return result;
        }
        public async Task<List<ReturnCovenantItem>> GetCovenantItemsByCovenantId(int covenantId)
        {
            var covenant = await _unitOfWork.Repository<Covenant>()
                .GetByIdWithIncludesAsync(covenantId, c => c.CovenantProducts, c => c.CovenantProducts.Select(p => p.Product));

            if (covenant == null) return new List<ReturnCovenantItem>();

            var result = covenant.CovenantProducts.Select(item => new ReturnCovenantItem
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.ProductName,  
                CategoryId = item.CategoryId,
                Amount = item.Amount
            }).ToList();

            return result;
        }
        public async Task<bool> DeleteCovenant(int covenantId)
        {
            try
            {
                var covenant = await _unitOfWork.Repository<Covenant>()
                    .GetByIdWithIncludesAsync(covenantId, c => c.CovenantProducts);

                if (covenant == null) return false;

                foreach (var item in covenant.CovenantProducts.ToList())
                {
                    _unitOfWork.Repository<CovenantProduct>().Delete(item);
                }

                _unitOfWork.Repository<Covenant>().Delete(covenant);

                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> DeleteCovenantItem(int covenantItemId)
        {
            try
            {
                var covenantItem = await _unitOfWork.Repository<CovenantProduct>().GetByIdAsync(covenantItemId);
                if (covenantItem == null) return false;

                _unitOfWork.Repository<CovenantProduct>().Delete(covenantItem);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<ReturnCovenant> GetCovenantById(int Id)
        {
            var covenant = await _unitOfWork.Repository<Covenant>().GetByIdAsync(Id);
            if (covenant == null) return null;

            return new ReturnCovenant
            {
                Id = covenant.Id,
                CovenantType = covenant.CovenantType,
                CovenantDate = covenant.CovenantDate
            };
        }
    }
}
