using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class Shop
    {
        public Shop()
        {
            WorkCenters = new List<WorkCenter>();
        }

        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public IList<WorkCenter> WorkCenters { get; set; }
    }
}
