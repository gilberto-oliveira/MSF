using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class ProviderRepository : BaseRepository<Provider>, IProviderRepository
    {
        public ProviderRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Provider> AllOrdered() => All().OrderBy(p => p.Code);

        public async Task<LazyProvidersViewModel> LazyProvidersViewModelAsync(string filter, int take, int skip)
        {
            var query = All().Include(c => c.State)
                .Where(x => (x.Code + x.Name + x.State.Name + x.State.Initials).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var providers = query.Select(s => new ProviderViewModel
            {
                Id = s.Id,
                StateId = s.StateId,
                Code = s.Code,
                Name = s.Name,
                StateName = s.State.Name
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyProviders = new LazyProvidersViewModel
            {
                Providers = await providers,
                Count = count
            };

            return lazyProviders;
        }
    }

    public interface IProviderRepository: IBaseRepository<Provider>
    {
        Task<LazyProvidersViewModel> LazyProvidersViewModelAsync(string filter, int take, int skip);
    }
}
