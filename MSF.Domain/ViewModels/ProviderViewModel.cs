using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class ProviderViewModel
    {
        public int Id { get; set; }

        public int StateId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string StateName { get; set; }
    }

    public class LazyProvidersViewModel
    {
        public IEnumerable<ProviderViewModel> Providers { get; set; }

        public int Count { get; set; }
    }
}
