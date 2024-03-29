﻿using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class ShopRepository : BaseRepository<Shop>, IShopRepository
    {
        public ShopRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Shop> AllOrdered() =>
            All().OrderBy(s => s.Code);

        public async Task<LazyShopsViewModel> LazyShopsViewModelAsync(string filter, int take, int skip)
        {
            var query = All()
                .Where(x => (x.Code + x.Description).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var shops = query.Select(s => new ShopViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyShops = new LazyShopsViewModel
            {
                Shops = await shops,
                Count = count
            };

            return lazyShops;
        }

        public async Task<List<ShopViewModel>> FindShopsViewModelAsync(string filter)
        {
            var query = All()
                .Where(x => (x.Code + x.Description).Contains(filter ?? string.Empty));

            return await query.Select(s => new ShopViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description
            })
            .ToListAsync();
        }

        public async Task<List<ShopViewModel>> AllByViewModelAsync()
        {
            return await All().Select(s => new ShopViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description
            }).ToListAsync();
        }
    }

    public interface IShopRepository : IBaseRepository<Shop>
    {
        Task<LazyShopsViewModel> LazyShopsViewModelAsync(string filter, int take, int skip);

        Task<List<ShopViewModel>> FindShopsViewModelAsync(string filter);

        Task<List<ShopViewModel>> AllByViewModelAsync();

    }
}
