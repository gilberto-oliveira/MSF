using System;
using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class WorkCenterViewModel
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string ShopName { get; set; }

        public string UserName { get; set; }

        public int? UserId { get; set; }

        public DateTime? Date { get; set; }

        public string Status { get; set; }
    }


    public class LazyWorkCentersViewModel
    {
        public IEnumerable<WorkCenterViewModel> WorkCenters { get; set; }

        public int Count { get; set; }
    }
}
