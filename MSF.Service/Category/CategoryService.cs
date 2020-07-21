using MSF.Domain.Comparers;
using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Service.Category
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unit) : base(unit) { }

        public async Task<int> AddAsync(Domain.Models.Category category)
        {
            _unit.CategoryRepository.Add(category);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(Domain.Models.Category category)
        {
            _unit.CategoryRepository.Delete(category);
            return await _unit.CommitChangesAsync();
        }

        public Task<LazyCategoriesViewModel> LazyCategoriesViewModelAsync(string filter, int take, int skip) =>
            _unit.CategoryRepository.LazyCategoriesViewModelAsync(filter, take, skip);

        public async Task<int> UpdateAsync(Domain.Models.Category category)
        {
            var equalityComparer = new SubcategoryEqualityComparer();
            var subcategoriesInDatabase = await _unit.SubcategoryRepository.FindByCategory(category.Id);
            var subcategoriesToDelete = subcategoriesInDatabase.Except(category.Subcategories, equalityComparer);
            _unit.SubcategoryRepository.Delete(subcategoriesToDelete);
            var subcategoriesToAdd = category.Subcategories.Except(subcategoriesInDatabase, equalityComparer);
            foreach (var s in subcategoriesToAdd) 
            {
                s.CategoryId = category.Id;
                _unit.SubcategoryRepository.Add(s); 
            }
            _unit.CategoryRepository.Update(category.Id, category);
            return await _unit.CommitChangesAsync();
        }

        public Task<Domain.Models.Category> FindAsync(int? id) =>
            _unit.CategoryRepository.FindAsync(id);

        public Task<IEnumerable<CategorySubcategoryViewModel>> FindByFilter(string filter) =>
            _unit.SubcategoryRepository.FindByFilter(filter);
    }

    public interface ICategoryService
    {
        Task<LazyCategoriesViewModel> LazyCategoriesViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Category category);
        
        Task<int> DeleteAsync(Domain.Models.Category category);

        Task<int> UpdateAsync(Domain.Models.Category category);

        Task<Domain.Models.Category> FindAsync(int? id);

        Task<IEnumerable<CategorySubcategoryViewModel>> FindByFilter(string filter);
    }
}
