using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        public StateRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<State> AllOrdered() => All().OrderBy(p => p.Name);

        public async Task<List<State>> FindAsync(string filter)
        {
            var query = All()
                .Where(x => (x.Name + x.Initials).Contains(filter ?? string.Empty));
            return await query.ToListAsync();
        }
    }

    public interface IStateRepository : IBaseRepository<State> 
    {
        Task<List<State>> FindAsync(string filter);
    }
}