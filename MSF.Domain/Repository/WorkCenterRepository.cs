using Microsoft.EntityFrameworkCore;
using MSF.Common;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class WorkCenterRepository : BaseRepository<WorkCenter>, IWorkCenterRepository
    {
        public WorkCenterRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<WorkCenter> AllOrdered() => All().OrderBy(o => o.Description);

        public async Task<LazyWorkCentersViewModel> LazyWorkCentersViewModelAsync(string filter, int take, int skip)
        {
            var query = All().Include(c => c.Shop)
                .Where(x => (x.Code + x.Description + x.Shop.Code + x.Shop.Description).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var workCenters = query.Select(s => new WorkCenterViewModel
            {
                Id = s.Id,
                ShopId = s.ShopId,
                Code = s.Code,
                Description = s.Description,
                ShopName = s.Shop.Description
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyWorkCenters = new LazyWorkCentersViewModel
            {
                WorkCenters = await workCenters,
                Count = count
            };

            return lazyWorkCenters;
        }

        public async Task<List<WorkCenterViewModel>> FindByShopAsync(int shopId)
        {
            var openedWorkCenter = MSFEnumDefaults.OpenedWorkCenter.ToDescription();
            var closedWorkCenter = MSFEnumDefaults.ClosedWorkCenter.ToDescription();

            var query = All().Include(w => w.WorkCenterControls).Where(w => w.ShopId == shopId);

            var shops = query.Select(s => new WorkCenterViewModel
            {
                Id = s.Id,
                ShopId = s.ShopId,
                Code = s.Code,
                Description = s.Description,
                Status = s.WorkCenterControls.Any(w => w.FinalDate == null) ? openedWorkCenter : closedWorkCenter,
                Date = s.WorkCenterControls.Any(w => w.FinalDate == null) ? s.WorkCenterControls.FirstOrDefault(w => w.FinalDate == null).StartDate : s.WorkCenterControls.OrderByDescending(o => o.FinalDate).FirstOrDefault().FinalDate,
                UserId = s.WorkCenterControls.Any(w => w.FinalDate == null) ? s.WorkCenterControls.FirstOrDefault(w => w.FinalDate == null).UserId : s.WorkCenterControls.OrderByDescending(o => o.FinalDate).FirstOrDefault().UserId
            });

            return await shops.ToListAsync();
        }

        public async Task<List<WorkCenterStats>> GetWorkCenterStatsAsync()
        {
            return await Context.Operations
                .Join(Context.WorkCenterControls, op => op.WorkCenterControlId, wcc => wcc.Id, (op, wcc) => new { op, wcc })
                .Join(Context.WorkCenters, j1 => j1.wcc.WorkCenterId, wc => wc.Id, (j1, wc) => new { j1.op, j1.wcc, wc })
                .Select(s => new
                {
                    WorkCenter = s.wc.Code + " - " + s.wc.Description,
                    Sale = s.op.UnitPrice * s.op.Amount,
                }).GroupBy(g => g.WorkCenter)
                .Select(s => new WorkCenterStats
                {
                    WorkCenterName = s.Key,
                    Sale = s.Sum(s1 => s1.Sale)
                }).ToListAsync();
        }
    }

    public interface IWorkCenterRepository : IBaseRepository<WorkCenter>
    {
        Task<LazyWorkCentersViewModel> LazyWorkCentersViewModelAsync(string filter, int take, int skip);

        Task<List<WorkCenterViewModel>> FindByShopAsync(int shopId);

        Task<List<WorkCenterStats>> GetWorkCenterStatsAsync();
    }
}
