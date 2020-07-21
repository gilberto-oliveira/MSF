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
    public class ProductRepository: BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMSFDbContext context) : base(context) { }

        public override IOrderedQueryable<Product> AllOrdered() => All().OrderBy(p => p.Description);

        public async Task<LazyProductsViewModel> LazyProductsViewModelAsync(string filter, int take, int skip)
        {
            var query = All().Include(c => c.Subcategory)
                .Where(x => (x.Description + x.Profit.ToString() + x.Subcategory.Description).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var products = query.Select(s => new ProductViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Profit = s.Profit,
                SubcategoryId = s.SubcategoryId,
                SubcategoryName = s.Subcategory.Description
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyProducts = new LazyProductsViewModel
            {
                Products = await products,
                Count = count
            };

            return lazyProducts;
        }
    }

    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<LazyProductsViewModel> LazyProductsViewModelAsync(string filter, int take, int skip);
    }
}
