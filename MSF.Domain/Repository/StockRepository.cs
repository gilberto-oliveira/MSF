using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Stock> AllOrdered() => All().OrderBy(e => e.Date);

        public async Task<LazyStocksViewModel> LazyStocksViewModelAsync(string filter, int take, int skip)
        {
            var query = All().Include(e => e.Product).Include(e => e.Provider)
                .Where(e => (e.Date.ToString() + e.Amount.ToString() + e.Product.Description + e.Provider.Code + e.Provider.Name + e.UnitPrice.ToString() + e.Product.Profit.ToString()).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var stocks = query.Select(s => new StockViewModel
            {
                Id = s.Id,
                ProductId = s.Product.Id,
                ProviderId = s.Provider.Id,
                ProductName = s.Product.Description,
                ProviderName = s.Provider.Name,
                UnitPrice = s.UnitPrice,
                Amount = s.Amount,
                Date = s.Date
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyStock = new LazyStocksViewModel
            {
                Stocks = await stocks,
                Count = count
            };

            return lazyStock;
        }

        public async Task<IEnumerable<ProductViewModel>> FindProductByFilter(string filter)
        {
            var query = All().Include(e => e.Product)
                .Where(e => (e.Date.ToString() + e.Amount.ToString() + e.Product.Description + e.UnitPrice.ToString() + e.Product.Profit.ToString()).Contains(filter ?? string.Empty) && e.Amount > 0)
                .Select(s => s.Product).Distinct();

            return await query.Select(s => new ProductViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Profit = s.Profit,
                SubcategoryId = s.SubcategoryId,
                SubcategoryName = s.Subcategory.Description
            }).ToListAsync();
        }

        public async Task<IEnumerable<ProviderViewModel>> FindProviderByFilterAndProduct(string filter, int productId)
        {
            var query = All().Include(e => e.Provider).ThenInclude(c => c.State)
                .Where(x => (x.Provider.Code + x.Provider.Name + x.Provider.State.Name + x.Provider.State.Initials).Contains(filter ?? string.Empty) && x.Amount > 0 && x.ProductId == productId)
                .Select(s => s.Provider).Distinct();

            return await query.Select(s => new ProviderViewModel
            {
                Id = s.Id,
                StateId = s.StateId,
                Code = s.Code,
                Name = s.Name,
                StateName = s.State.Name
            }).ToListAsync();
        }

        public Task<bool> HasAvailable(int productId, int providerId)
            => All().AnyAsync(e => e.ProductId == productId && e.ProviderId == providerId);

        public Task<Stock> FindAvailableByProductAndProvider(int productId, int providerId)
            => All().FirstOrDefaultAsync(e => e.ProductId == productId && e.ProviderId == providerId);
    }

    public interface IStockRepository : IBaseRepository<Stock>
    {
        Task<LazyStocksViewModel> LazyStocksViewModelAsync(string filter, int take, int skip);

        Task<IEnumerable<ProductViewModel>> FindProductByFilter(string filter);

        Task<IEnumerable<ProviderViewModel>> FindProviderByFilterAndProduct(string filter, int productId);

        Task<bool> HasAvailable(int productId, int providerId);

        Task<Stock> FindAvailableByProductAndProvider(int productId, int providerId);
    }
}
