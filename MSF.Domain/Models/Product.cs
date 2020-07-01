using System;
using System.Collections.Generic;
using System.Text;

namespace MSF.Domain.Models
{
    public class Product
    {
        public Product()
        {
            Operations = new List<Operation>();
            Stocks = new List<Stock>();
        }

        public int Id { get; set; }

        public int SubcategoryId { get; set; }

        public string Description { get; set; }

        public decimal Profit { get; set; }

        public Subcategory Subcategory { get; set; }

        public IList<Operation> Operations { get; set; }

        public IList<Stock> Stocks { get; set; }
    }
}
