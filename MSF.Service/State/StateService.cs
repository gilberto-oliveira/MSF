using MSF.Domain.UnitOfWork;
using MSF.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSF.Service.State
{
    public class StateService : BaseService, IStateService
    {
        public StateService(IUnitOfWork unit) : base(unit) { }

        public Task<List<Domain.Models.State>> AllAsync() =>
            _unit.StateRepository.AllAsync();

        public Task<Domain.Models.State> FindAsync(int? id) =>
             _unit.StateRepository.FindAsync(id);

        public Task<List<Domain.Models.State>> FindAsync(string filter) =>
            _unit.StateRepository.FindAsync(filter);
    }

    public interface IStateService : IBaseService
    {
        Task<List<Domain.Models.State>> AllAsync();

        Task<Domain.Models.State> FindAsync(int? id);

        Task<List<Domain.Models.State>> FindAsync(string filter);
    }
}
