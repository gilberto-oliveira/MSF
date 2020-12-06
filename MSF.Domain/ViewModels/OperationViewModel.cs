using System;
using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class OperationViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int WorkCenterControlId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public decimal UnitPrice { get; set; }

        public int Amount { get; set; }
    }

    public class LazyOperationViewModel
    {
        public IEnumerable<OperationViewModel> Operations { get; set; }

        public int Count { get; set; }
    }

    public class SaleByUserViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public decimal Sale { get; set; }
    }

    public class SaleByCategoryViewModel
    {
        public int CategoryId { get; set; }

        public string Category { get; set; }

        public decimal Sale { get; set; }
    }
}
