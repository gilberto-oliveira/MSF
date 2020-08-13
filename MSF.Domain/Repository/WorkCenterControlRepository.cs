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
    public class WorkCenterControlRepository : BaseRepository<WorkCenterControl>, IWorkCenterControlRepository
    {
        public WorkCenterControlRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<WorkCenterControl> AllOrdered() => All().OrderBy(o => o.StartDate);

        public async Task<WorkCenterControl> GetLastByWorkCenterId(int id)
        {
            var query = await AllAsync();

            return query
                    .Where(wc => wc.WorkCenterId == id && wc.FinalDate is null)
                    .OrderByDescending(wc => wc.StartDate)
                    .FirstOrDefault();
        }
    }

    public interface IWorkCenterControlRepository : IBaseRepository<WorkCenterControl>
    {
        Task<WorkCenterControl> GetLastByWorkCenterId(int id);
    }
}
