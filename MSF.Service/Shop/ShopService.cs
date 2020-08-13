using Microsoft.EntityFrameworkCore;
using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Identity.Models;
using MSF.Service.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Service.Shop
{
    public class ShopService : BaseService, IShopService
    {
        private MSF.Identity.Context.MSFIdentityDbContext identityContext;

        public ShopService(IUnitOfWork unit, MSF.Identity.Context.MSFIdentityDbContext _identityContext) : base(unit) 
        {
            identityContext = _identityContext;
        }

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

        public async Task<List<ShopViewModel>> FindShopsByUserRoleAsync(int userId, int roleId)
        {
            var shops = await _unit.ShopRepository.AllByViewModelAsync();

            var userRoleShops = await identityContext.UserRoleShops.Where(urs => urs.UserId == userId && urs.RoleId == roleId).ToListAsync();

            return shops.Select(s => new ShopViewModel
            {
                Code = s.Code,
                Description = s.Description,
                Id = s.Id,
                Used = userRoleShops.Any(u => u.ShopId == s.Id)
            }).ToList();
        }

        public async Task<List<ShopViewModel>> FindShopsByCurrentUserAsync()
        {
            var currentUserId = _unit.UserId;

            if (currentUserId != null)
            {
                var shopIds = await identityContext.UserRoleShops.Where(urs => urs.UserId == currentUserId).Select(s => s.ShopId).ToListAsync();

                var shops = await _unit.ShopRepository.AllByViewModelAsync();

                return shops.Where(s => shopIds.Contains(s.Id)).Select(s => new ShopViewModel
                {
                    Code = s.Code,
                    Description = s.Description,
                    Id = s.Id
                }).ToList();
            }

            return new List<ShopViewModel> { };
        }

    }

    public interface IShopService
    {
        Task<LazyShopsViewModel> LazyShopsViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Shop shop);

        Task<int> DeleteAsync(Domain.Models.Shop shop);

        Task<int> UpdateAsync(Domain.Models.Shop shop);

        Task<Domain.Models.Shop> FindAsync(int? id);

        Task<List<ShopViewModel>> FindShopsViewModelAsync(string filter);

        Task<List<ShopViewModel>> FindShopsByUserRoleAsync(int userId, int roleId);

        Task<List<ShopViewModel>> FindShopsByCurrentUserAsync();
    }
}
