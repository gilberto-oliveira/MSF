using System;
using System.Collections.Generic;
using System.Text;

namespace MSF.Domain.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int SubcategoryId { get; set; }

        public string Description { get; set; }

        public decimal Profit { get; set; }

        public string SubcategoryName { get; set; }
    }

    public class LazyProductsViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }

        public int Count { get; set; }
    }
}
