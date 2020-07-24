using System;
using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ProviderId { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string ProductName { get; set; }

        public string ProviderName { get; set; }
    }

    public class LazyStocksViewModel
    {
        public IEnumerable<StockViewModel> Stocks { get; set; }

        public int Count { get; set; }
    }
}
