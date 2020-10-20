using Microsoft.AspNetCore.Identity;
using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Identity.Models;
using MSF.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Service.WorkCenter
{
    public class WorkCenterService : BaseService, IWorkCenterService
    {
        private readonly UserManager<User> _userManager;

        public WorkCenterService(UserManager<User> userManager, IUnitOfWork unit) : base(unit)
        {
            _userManager = userManager;
        }

        public async Task<int> StartAsync(int id)
        {
            var currentUserID = _unit.UserId;

            if (currentUserID.HasValue)
            {
                var workCenterControl = new Domain.Models.WorkCenterControl { WorkCenterId = id, UserId = currentUserID.Value, StartDate = DateTime.Now };
                _unit.WorkCenterControlRepository.Add(workCenterControl);
            }

            return await _unit.CommitChangesAsync();
        }

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

        public async Task<List<WorkCenterViewModel>> FindByShopAsync(int shopId)
        {
            var workCenters = await _unit.WorkCenterRepository.FindByShopAsync(shopId);

            return workCenters.Select(s => new WorkCenterViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description,
                Date = s.Date,
                ShopId = s.ShopId,
                Status = s.Status,
                UserId = s.UserId,
                UserName = _userManager.Users.FirstOrDefault(u => u.Id == s.UserId)?.FirstName + " " + _userManager.Users.FirstOrDefault(u => u.Id == s.UserId)?.LastName
            }).ToList();
        }

        public async Task<int> CloseAsync(int id)
        {
            var currentUserID  = _unit.UserId;
            var lastWorkCenter = await _unit.WorkCenterControlRepository.GetLastByWorkCenterId(id);

            if (currentUserID != lastWorkCenter.UserId)
            {
                throw new Exception("Não é possível fechar o caixa aberto por outro usuário.");
            }

            if (lastWorkCenter != null)
            {
                lastWorkCenter.FinalDate = DateTime.Now;
                _unit.WorkCenterControlRepository.Update(lastWorkCenter.Id, lastWorkCenter);
            }

            return await _unit.CommitChangesAsync();
        }

        
    }

    public interface IWorkCenterService: IBaseService
    {
        Task<LazyWorkCentersViewModel> LazyWorkCentersViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.WorkCenter workCenter);

        Task<int> StartAsync(int id);

        Task<int> DeleteAsync(Domain.Models.WorkCenter workCenter);

        Task<int> UpdateAsync(Domain.Models.WorkCenter workCenter);

        Task<Domain.Models.WorkCenter> FindAsync(int? id);

        Task<List<WorkCenterViewModel>> FindByShopAsync(int shopId);

        Task<int> CloseAsync(int id);
    }
}
