using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSF.Service.Shop
{
    public class ShopService : BaseService, IShopService
    {
        public ShopService(IUnitOfWork unit) : base(unit) { }

        public async Task<int> AddAsync(Domain.Models.Shop shop)
        {
            _unit.ShopRepository.Add(shop);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(Domain.Models.Shop shop)
        {
            _unit.ShopRepository.Delete(shop);
            return await _unit.CommitChangesAsync();
        }

        public Task<LazyShopsViewModel> LazyShopsViewModelAsync(string filter, int take, int skip) =>
            _unit.ShopRepository.LazyShopsViewModelAsync(filter, take, skip);

        public async Task<int> UpdateAsync(Domain.Models.Shop shop)
        {
            _unit.ShopRepository.Update(shop.Id, shop);
            return await _unit.CommitChangesAsync();
        }

        public Task<List<ShopViewModel>> FindShopsViewModelAsync(string filter) =>
            _unit.ShopRepository.FindShopsViewModelAsync(filter);

        public Task<Domain.Models.Shop> FindAsync(int? id) =>
            _unit.ShopRepository.FindAsync(id);
    }

    public interface IShopService
    {
        Task<LazyShopsViewModel> LazyShopsViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Shop shop);

        Task<int> DeleteAsync(Domain.Models.Shop shop);

        Task<int> UpdateAsync(Domain.Models.Shop shop);

        Task<Domain.Models.Shop> FindAsync(int? id);

        Task<List<ShopViewModel>> FindShopsViewModelAsync(string filter);
    }
}
