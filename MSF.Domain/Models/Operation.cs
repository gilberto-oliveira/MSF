using System;

namespace MSF.Domain.Models
{
    public class Operation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int WorkCenterControlId { get; set; }

        public int ProductId { get; set; }

        public int ProviderId { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public decimal UnitPrice { get; set; }

        public int Amount { get; set; }

        public WorkCenterControl WorkCenterControl { get; set; }

        public Product Product { get; set; }

        public Provider Provider { get; set; }
    }
}
