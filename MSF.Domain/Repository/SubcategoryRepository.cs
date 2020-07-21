using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
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

        public async Task<IEnumerable<CategorySubcategoryViewModel>> FindByFilter(string filter)
        {
            var subcategories = All().Include(s => s.Category)
                .Where(s => (s.Category.Code + s.Category.Description + s.Description).Contains(filter ?? string.Empty));

            return await subcategories
                .Select(s => new CategorySubcategoryViewModel {
                    SubCategoryId = s.Id,
                    Subcategory = s.Description,
                    Category = s.Category.Description,
                    CategoryCode = s.Category.Code
                }).ToListAsync();
        }
    }

    public interface ISubcategoryRepository: IBaseRepository<Subcategory> 
    {
        Task<IEnumerable<Subcategory>> FindByCategory(int categoryId);

        Task<IEnumerable<CategorySubcategoryViewModel>> FindByFilter(string filter);
    }
}
