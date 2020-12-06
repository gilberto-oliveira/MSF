using System;
using System.Collections.Generic;
using System.Text;

namespace MSF.Domain.ViewModels
{
    public class ProductStatsViewModel
    {
        public string ProductName { get; set; }

        public decimal Sale { get; set; }

        public decimal SalePercent { get; set; }

        public decimal Profit { get; set; }
    }

    public class LazyProductStatsViewModel
    {
        public IEnumerable<ProductStatsViewModel> ProductStats { get; set; }

        public int Count { get; set; }
    }
}
