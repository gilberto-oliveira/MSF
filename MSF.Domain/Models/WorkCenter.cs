using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class WorkCenter
    {
        public WorkCenter()
        {
            WorkCenterControls = new List<WorkCenterControl>();
        }

        public int Id { get; set; }

        public int ShopId { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public Shop Shop { get; set; }

        public IList<WorkCenterControl> WorkCenterControls { get; set; }
    }
}
