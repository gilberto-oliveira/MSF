using System.Collections.Generic;

namespace MSF.Domain.Models
{
    public class Subcategory
    {
        public Subcategory()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public IList<Product> Products { get; set; }
    }
}
