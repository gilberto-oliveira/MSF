using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Threading.Tasks;

namespace MSF.Service.Product
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unit) : base(unit) { }

        public Task<LazyProductsViewModel> LazyProductsViewModelAsync(string filter, int take, int skip) =>
            _unit.ProductRepository.LazyProductsViewModelAsync(filter, take, skip);

        public async Task<int> AddAsync(Domain.Models.Product product)
        {
            _unit.ProductRepository.Add(product);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(Domain.Models.Product product)
        {
            _unit.ProductRepository.Delete(product);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> UpdateAsync(Domain.Models.Product product)
        {
            _unit.ProductRepository.Update(product.Id, product);
            return await _unit.CommitChangesAsync();
        }

        public Task<Domain.Models.Product> FindAsync(int? id) =>
            _unit.ProductRepository.FindAsync(id);
    }

    public interface IProductService
    {
        Task<LazyProductsViewModel> LazyProductsViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Product product);

        Task<int> DeleteAsync(Domain.Models.Product product);

        Task<int> UpdateAsync(Domain.Models.Product product);

        Task<Domain.Models.Product> FindAsync(int? id);
    }
}
