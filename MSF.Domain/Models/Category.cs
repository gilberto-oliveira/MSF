using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class Category
    {
        public Category()
        {
            Subcategories = new List<Subcategory>();
        }

        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public IList<Subcategory> Subcategories { get; set; }
    }
}
