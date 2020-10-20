using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class OperationRepository : BaseRepository<Operation>, IOperationRepository
    {
        public OperationRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Operation> AllOrdered() => All().OrderBy(o => o.Date);

        public async Task<LazyOperationViewModel> LazyViewModelAsync(int workCenterControlId, string type, string filter, int take, int skip)
        {
            var query = All()
                .Include(c => c.Product)
                .Include(o => o.Provider)
                .Where(x => x.WorkCenterControlId == workCenterControlId &&
                    x.Type.Equals(type) &&
                    (x.Product.Description + x.Provider.Name + x.Provider.Code + x.Amount.ToString() + x.UnitPrice.ToString())
                    .Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var operations = query.Select(s => new OperationViewModel
            {
                Id = s.Id,
                WorkCenterControlId = s.WorkCenterControlId,
                ProductId = s.ProductId,
                ProductName = s.Product.Description,
                ProviderName = s.Provider.Name,
                ProviderId = s.ProviderId,
                Amount = s.Amount,
                UnitPrice = s.UnitPrice,
                Type = s.Type,
                Date = s.Date
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyOperation = new LazyOperationViewModel
            {
                Operations = await operations,
                Count = count
            };

            return lazyOperation;
        }

        public Task<List<Operation>> FindByWorkCenterControlAndTypeAsync(int workCenterControlId, string type) => 
            All().Where(o => o.WorkCenterControlId == workCenterControlId && o.Type.Equals(type)).ToListAsync();

        public Task<Operation> FindByProductAndProvider(int id, int productId, int providerId)
            => All().FirstOrDefaultAsync(o => o.ProductId == productId && o.ProviderId == providerId && o.Id == id);

        public async Task<decimal> FindTotalPriceByWorkCenterControlAndTypeAsync(int workCenterControlId, string type)
        {
            var query = await All()
                .Where(x => x.WorkCenterControlId == workCenterControlId && x.Type.Equals(type))
                .Select(s => s.UnitPrice * s.Amount).SumAsync();
            return Math.Round(query, 2);
        }
    }

    public interface IOperationRepository : IBaseRepository<Operation>
    {
        Task<LazyOperationViewModel> LazyViewModelAsync(int workCenterControlId, string type, string filter, int take, int skip);

        Task<List<Operation>>        FindByWorkCenterControlAndTypeAsync(int workCenterControlId, string type);

        Task<Operation> FindByProductAndProvider(int id, int productId, int providerId);

        Task<decimal> FindTotalPriceByWorkCenterControlAndTypeAsync(int workCenterControlId, string type);
    }
}
