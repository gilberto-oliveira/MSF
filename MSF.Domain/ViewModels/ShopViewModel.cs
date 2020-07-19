using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class ShopViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class LazyShopsViewModel
    {
        public IEnumerable<ShopViewModel> Shops { get; set; }

        public int Count { get; set; }
    }
}
