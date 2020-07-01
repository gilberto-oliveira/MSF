using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class State
    {
        public State()
        {
            Providers = new List<Provider>();
        }

        public int Id { get; set; }

        public string Initials { get; set; }

        public string Name { get; set; }

        public IList<Provider> Providers { get; set; }
    }
}
