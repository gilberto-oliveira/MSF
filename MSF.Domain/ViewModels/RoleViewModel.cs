using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LazyRoleViewModel
    {
        public List<RoleViewModel> Roles { get; set; }
        public int Count { get; set; }
    }

    public class RequestLazyRoleViewModel
    {
        public RoleViewModel Filters { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public int? UserId { get; set; }
    }
}
