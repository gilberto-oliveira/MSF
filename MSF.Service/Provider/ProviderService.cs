using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSF.Service.Provider
{
    public class ProviderService : BaseService, IProviderService
    {
        public ProviderService(IUnitOfWork unit) : base(unit) { }

        public Task<LazyProvidersViewModel> LazyProvidersViewModelAsync(string filter, int take, int skip) => 
            _unit.ProviderRepository.LazyProvidersViewModelAsync(filter, take, skip);

        public async Task<int> AddAsync(Domain.Models.Provider provider)
        {
            _unit.ProviderRepository.Add(provider);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(Domain.Models.Provider provider)
        {
            _unit.ProviderRepository.Delete(provider);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> UpdateAsync(Domain.Models.Provider provider)
        {
            _unit.ProviderRepository.Update(provider.Id, provider);
            return await _unit.CommitChangesAsync();
        }

        public Task<Domain.Models.Provider> FindAsync(int? id) =>
            _unit.ProviderRepository.FindAsync(id);

        public Task<IEnumerable<ProviderViewModel>> FindByFilter(string filter) =>
            _unit.ProviderRepository.FindByFilter(filter);
    }

    public interface IProviderService
    {
        Task<LazyProvidersViewModel> LazyProvidersViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Provider provider);

        Task<int> DeleteAsync(Domain.Models.Provider provider);

        Task<int> UpdateAsync(Domain.Models.Provider provider);

        Task<Domain.Models.Provider> FindAsync(int? id);

        Task<IEnumerable<ProviderViewModel>> FindByFilter(string filter);
    }
}
