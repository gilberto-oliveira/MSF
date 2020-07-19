using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
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
    }

    public interface IWorkCenterRepository: IBaseRepository<WorkCenter>
    {
        Task<LazyWorkCentersViewModel> LazyWorkCentersViewModelAsync(string filter, int take, int skip);
    }
}
