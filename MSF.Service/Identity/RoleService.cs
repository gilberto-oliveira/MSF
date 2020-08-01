using Microsoft.EntityFrameworkCore;
using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Identity.Context;
using MSF.Identity.Models;
using MSF.Service.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Service.Identity
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly MSFIdentityDbContext _identityContext;
        public RoleService(IUnitOfWork unit
                         , MSFIdentityDbContext identityContext) : base(unit)
        {
            _identityContext = identityContext;
        }

        public async Task<LazyRoleViewModel> LazyRoles(int take, int skip, string nameFilter = null)
        {
            var query = _identityContext.Roles
                .Where(x => x.Name.Contains(!string.IsNullOrEmpty(nameFilter) ? nameFilter : string.Empty));

            var count = await query.CountAsync();

            var roles = query
                .Select(s => new RoleViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .OrderBy(o => o.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var lazyRoleViewModel = new LazyRoleViewModel
            {
                Roles = await roles,
                Count = count
            };

            return lazyRoleViewModel;
        }

        public async Task<LazyRoleViewModel> LazyRolesByUser(int userId, int take, int skip, string nameFilter = null)
        {
            var query = _identityContext.UserRoles.Where(w => w.UserId == userId)
                .Join(_identityContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                .Select(s => s.r).Distinct()
                .Where(x => x.Name.Contains(!string.IsNullOrEmpty(nameFilter) ? nameFilter : string.Empty));

            var count = await query.CountAsync();

            var roles = query
                .Select(s => new RoleViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .OrderBy(o => o.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var lazyRoleViewModel = new LazyRoleViewModel
            {
                Roles = await roles,
                Count = count
            };

            return lazyRoleViewModel;
        }

        public async Task<List<RoleViewModel>> GetRolesWithoutUser(int userId)
        {
            var userRoles = await _identityContext.UserRoles.Where(w => w.UserId == userId)
                .Join(_identityContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                .Select(s => s.r).Distinct().ToListAsync();

            var roles = await _identityContext.Roles.Select(s => s).ToListAsync();

            var except = roles.Except(userRoles).Select(s => new RoleViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return except;
        }

        public async Task CreateUserRole(int userId, int roleId)
        {
            var userRole = new UserRole { 
                UserId = userId, 
                RoleId = roleId 
            };

            await _identityContext.UserRoles.AddAsync(userRole);

            await _identityContext.SaveChangesAsync();
        }

        public async Task DeleteUserRole(int userId, int roleId)
        {
            var userRole = await _identityContext.UserRoles.FirstOrDefaultAsync(u => u.RoleId == roleId && u.UserId == userId);

            if (userRole != null)
            {
                _identityContext.UserRoles.Remove(userRole);

                await _identityContext.SaveChangesAsync();
            }
        }
    }

    public interface IRoleService
    {
        Task<LazyRoleViewModel> LazyRoles(int take, int skip, string nameFilter = null);
        Task<LazyRoleViewModel> LazyRolesByUser(int userId, int take, int skip, string nameFilter = null);
        Task<List<RoleViewModel>> GetRolesWithoutUser(int userId);
        Task CreateUserRole(int userId, int roleId);
        Task DeleteUserRole(int userId, int roleId);
    }
}
