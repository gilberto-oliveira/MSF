using MSF.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSF.Domain.Comparers
{
    public class SubcategoryEqualityComparer : IEqualityComparer<Subcategory>
    {
        public bool Equals(Subcategory x, Subcategory y)
        {
            return x.Description == y.Description;
        }

        public int GetHashCode(Subcategory obj)
        {
            return obj.Description.GetHashCode();
        }
    }
}
