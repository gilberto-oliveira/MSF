using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class Provider
    {
        public Provider()
        {
            Operations = new List<Operation>();
            Stocks = new List<Stock>();
        }

        public int Id { get; set; }

        public int StateId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public State State { get; set; }

        public IList<Operation> Operations { get; set; }

        public IList<Stock> Stocks { get; set; }
    }
}
