using Microsoft.EntityFrameworkCore;
using MSF.Domain.Context;
using MSF.Domain.Models;
using MSF.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Category> AllOrdered() => All().OrderBy(o => o.Description);

        public async Task<LazyCategoriesViewModel> LazyCategoriesViewModelAsync(string filter, int take, int skip)
        {
            var query = All().Include(c => c.Subcategories)
                .Where(x => (x.Code + x.Description).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var categories = query.Select(s => new CategoryViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Description = s.Description,
                Subcategories = s.Subcategories.Select(ss => new SubcategoryViewModel {
                    Id = ss.Id,
                    Description = ss.Description
                })
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyCategories = new LazyCategoriesViewModel
            {
                Categories = await categories,
                Count = count
            };

            return lazyCategories;
        }
    }

    public interface ICategoryRepository: IBaseRepository<Category>
    {
        Task<LazyCategoriesViewModel> LazyCategoriesViewModelAsync(string filter, int take, int skip);
    }
}
