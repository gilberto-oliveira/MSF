using System;
using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class WorkCenterControlViewModel
    {
        public WorkCenterControlViewModel()
        {
            Operations = new List<OperationViewModel>();
        }

        public int Id { get; set; }

        public int WorkCenterId { get; set; }

        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinalDate { get; set; }

        public List<OperationViewModel> Operations { get; set; }
    }
}
