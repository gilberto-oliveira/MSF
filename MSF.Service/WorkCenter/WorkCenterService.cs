using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Threading.Tasks;

namespace MSF.Service.WorkCenter
{
    public class WorkCenterService : BaseService, IWorkCenterService
    {
        public WorkCenterService(IUnitOfWork unit) : base(unit) { }

        public Task<LazyWorkCentersViewModel> LazyWorkCentersViewModelAsync(string filter, int take, int skip) =>
            _unit.WorkCenterRepository.LazyWorkCentersViewModelAsync(filter, take, skip);

        public async Task<int> AddAsync(Domain.Models.WorkCenter workCenter)
        {
            _unit.WorkCenterRepository.Add(workCenter);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(Domain.Models.WorkCenter workCenter)
        {
            _unit.WorkCenterRepository.Delete(workCenter);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> UpdateAsync(Domain.Models.WorkCenter workCenter)
        {
            _unit.WorkCenterRepository.Update(workCenter.Id, workCenter);
            return await _unit.CommitChangesAsync();
        }

        public Task<Domain.Models.WorkCenter> FindAsync(int? id) =>
            _unit.WorkCenterRepository.FindAsync(id);

    }

    public interface IWorkCenterService
    {
        Task<LazyWorkCentersViewModel> LazyWorkCentersViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.WorkCenter workCenter);

        Task<int> DeleteAsync(Domain.Models.WorkCenter workCenter);

        Task<int> UpdateAsync(Domain.Models.WorkCenter workCenter);

        Task<Domain.Models.WorkCenter> FindAsync(int? id);
    }
}
