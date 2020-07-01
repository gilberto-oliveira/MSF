using System;
using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class WorkCenterControl
    {
        public WorkCenterControl()
        {
            Operations = new List<Operation>();
        }

        public int Id { get; set; }

        public int WorkCenterId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinalDate { get; set; }

        public WorkCenter WorkCenter { get; set; }

        public IList<Operation> Operations { get; set; }
    }
}
