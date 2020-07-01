using System;

namespace MSF.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ProviderId { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public Product Product { get; set; }

        public Provider Provider { get; set; }
    }
}
