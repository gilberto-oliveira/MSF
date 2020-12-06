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

        public async Task<List<SaleByUserViewModel>> GetSalesByUser()
        {
            var query = All()
                .Select(s => new
                {
                    s.UserId,
                    Sale = s.UnitPrice * s.Amount,
                }).GroupBy(g => g.UserId);

            return await query.Select(s => new SaleByUserViewModel
            {
                UserId = s.Key,
                Sale = s.Sum(s0 => s0.Sale)
            }).ToListAsync();
        }

        public async Task<List<SaleByCategoryViewModel>> GetSalesByCategory()
        {
            var query = await All()
                .Join(Context.Products, op => op.ProductId, p => p.Id, (op, p) => new { op, p })
                .Join(Context.Subcategories, j1 => j1.p.SubcategoryId, s => s.Id, (j1, s) => new { j1.op, j1.p, s })
                .Join(Context.Categories, j2 => j2.s.CategoryId, c => c.Id, (j2, c) => new { j2.op, j2.p, j2.s, c })
                .Select(s => new {
                    Category = s.c.Description + " -> " + s.s.Description,
                    Sale = s.op.UnitPrice * s.op.Amount,
                })
                .GroupBy(g => g.Category)
                .Select(s => new SaleByCategoryViewModel
                {
                    Category = s.Key,
                    Sale = s.Sum(s1 => s1.Sale)
                }).ToListAsync();

            return query;
        }
    }

    public interface IOperationRepository : IBaseRepository<Operation>
    {
        Task<LazyOperationViewModel> LazyViewModelAsync(int workCenterControlId, string type, string filter, int take, int skip);

        Task<List<Operation>>        FindByWorkCenterControlAndTypeAsync(int workCenterControlId, string type);

        Task<Operation> FindByProductAndProvider(int id, int productId, int providerId);

        Task<decimal> FindTotalPriceByWorkCenterControlAndTypeAsync(int workCenterControlId, string type);

        Task<List<SaleByUserViewModel>> GetSalesByUser();

        Task<List<SaleByCategoryViewModel>> GetSalesByCategory();
    }
}
