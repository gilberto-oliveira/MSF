using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class ProductRepository: BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Product> AllOrdered() => All().OrderBy(p => p.Description);

        public async Task<LazyProductsViewModel> LazyProductsViewModelAsync(string filter, int take, int skip)
        {
            var query = All().Include(c => c.Subcategory)
                .Where(x => (x.Description + x.Profit.ToString() + x.Subcategory.Description).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var products = query.Select(s => new ProductViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Profit = s.Profit,
                SubcategoryId = s.SubcategoryId,
                SubcategoryName = s.Subcategory.Description
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyProducts = new LazyProductsViewModel
            {
                Products = await products,
                Count = count
            };

            return lazyProducts;
        }

        public async Task<IEnumerable<ProductViewModel>> FindByFilter(string filter)
        {
            var query = All().Include(c => c.Subcategory)
                .Where(x => (x.Description + x.Profit.ToString() + x.Subcategory.Description).Contains(filter ?? string.Empty));
            
            return await query.Select(s => new ProductViewModel {
                Id = s.Id,
                Description = s.Description,
                Profit = s.Profit,
                SubcategoryId = s.SubcategoryId,
                SubcategoryName = s.Subcategory.Description
            }).ToListAsync();
        }

        public async Task<LazyProductStatsViewModel> LazyProductStatsViewModelAsync(string filter, int take, int skip)
        {
            var query = Context.Operations
                    .Include(c => c.Product).Where(o => o.Product.Description.Contains(filter ?? string.Empty)).GroupBy(g => g.Product)
                .Select(s => new ProductStatsViewModel
                {
                    ProductName = s.Key.Description,
                    Sale = s.Sum(s1 => s1.Amount * s1.UnitPrice),
                    SalePercent = ((decimal) s.Sum(s1 => s1.Amount) /
                        (decimal) (Context.Stocks.Where(e => e.ProductId == s.Key.Id).Sum(s1 => s1.Amount) + s.Sum(s1 => s1.Amount))) * 100,
                    Profit = s.Sum(s1 => s1.Amount * s1.UnitPrice) - s.Sum(s1 => s1.Amount * Context.Stocks.FirstOrDefault(e => e.ProductId == s.Key.Id && e.ProviderId == s1.ProviderId).UnitPrice)
                });

            var count = await query.CountAsync();

            var lazyProducts = new LazyProductStatsViewModel
            {
                ProductStats = await query
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(),
                Count = count
            };

            return lazyProducts;
        }
    }

    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<LazyProductsViewModel> LazyProductsViewModelAsync(string filter, int take, int skip);

        Task<LazyProductStatsViewModel> LazyProductStatsViewModelAsync(string filter, int take, int skip);

        Task<IEnumerable<ProductViewModel>> FindByFilter(string filter);
    }
}
