using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public IEnumerable<SubcategoryViewModel> Subcategories { get; set; }
    }

    public class SubcategoryViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class LazyCategoriesViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public int Count { get; set; }
    }
}
