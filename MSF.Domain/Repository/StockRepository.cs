using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
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
    }

    public interface IStockRepository : IBaseRepository<Stock>
    {
        Task<LazyStocksViewModel> LazyStocksViewModelAsync(string filter, int take, int skip);
    }
}
