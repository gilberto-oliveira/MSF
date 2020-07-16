using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Threading.Tasks;

namespace MSF.Service.Provider
{
    public class ProviderService : BaseService, IProviderService
    {
        public ProviderService(IUnitOfWork unit) : base(unit) { }

        public Task<LazyProvidersViewModel> LazyProvidersViewModelAsync(string filter, int take, int skip) => 
            _unit.ProviderRepository.LazyProvidersViewModelAsync(filter, take, skip);
    }

    public interface IProviderService
    {
        Task<LazyProvidersViewModel> LazyProvidersViewModelAsync(string filter, int take, int skip);
    }
}
