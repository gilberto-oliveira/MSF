using MSF.Domain.Context;
using MSF.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class SubcategoryRepository : BaseRepository<Subcategory>, ISubcategoryRepository
    {
        public SubcategoryRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Subcategory> AllOrdered() =>
            throw new NotImplementedException();

        public async Task<IEnumerable<Subcategory>> FindByCategory(int categoryId)
        {
            var subcategories = await AllAsync();
            return subcategories
                .Where(s => s.CategoryId == categoryId);
        }
    }

    public interface ISubcategoryRepository: IBaseRepository<Subcategory> 
    {
        Task<IEnumerable<Subcategory>> FindByCategory(int categoryId);
    }
}
