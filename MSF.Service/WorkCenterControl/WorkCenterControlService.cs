using MSF.Common;
using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSF.Service.WorkCenterControl
{
    public class WorkCenterControlService : BaseService, IWorkCenterControlService
    {
        public WorkCenterControlService(IUnitOfWork unit) : base(unit) { }

        public async Task<WorkCenterControlViewModel> LazyOpenedByWorkCenterAsync(int workCenterID)
        {
            var workCenterControl = await _unit.WorkCenterControlRepository.GetLastByWorkCenterId(workCenterID);

            if (workCenterControl is null)
                return null;

            return new WorkCenterControlViewModel
            {
                Id = workCenterControl.Id,
                WorkCenterId = workCenterControl.WorkCenterId,
                UserId = workCenterControl.UserId,
                StartDate = workCenterControl.StartDate,
                FinalDate = workCenterControl.FinalDate
            };
        }

        public async Task<int> FinishSaleProcessAsync(int id)
        {
            var type = MSFEnum.GetEnumValue(MSFEnumOperationType.AddCart);
            var operations = await _unit.OperationRepository.FindByWorkCenterControlAndTypeAsync(id, type);
            operations.ForEach((o) => {
                o.Type = MSFEnum.GetEnumValue(MSFEnumOperationType.Sale);
                _unit.OperationRepository.Update(o.Id, o);
            });
            return await _unit.CommitChangesAsync();
        }
    }

    public interface IWorkCenterControlService: IBaseService
    {
        Task<WorkCenterControlViewModel> LazyOpenedByWorkCenterAsync(int workCenterID);

        Task<int> FinishSaleProcessAsync(int id);
    }
}
