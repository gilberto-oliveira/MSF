using MSF.Domain.Context;
using MSF.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace MSF.Domain.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICategoryRepository _categoryRepository;

        private ISubcategoryRepository _subcategoryRepository;

        private IProviderRepository _providerRepository;

        private IStateRepository _stateRepository;

        private IShopRepository _shopRepository;

        private IWorkCenterRepository _workCenterRepository;

        private IProductRepository _productRepository;

        private IStockRepository _stockRepository;

        private IWorkCenterControlRepository _workCenterControlRepository;

        private readonly IMSFDbContext _context;

        private bool _disposed;

        public UnitOfWork(IMSFDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository CategoryRepository => _categoryRepository ?? (_categoryRepository = new CategoryRepository(_context));

        public ISubcategoryRepository SubcategoryRepository => _subcategoryRepository ?? (_subcategoryRepository = new SubcategoryRepository(_context));
        
        public IProviderRepository ProviderRepository => _providerRepository ?? (_providerRepository = new ProviderRepository(_context));
        
        public IStateRepository StateRepository => _stateRepository ?? (_stateRepository = new StateRepository(_context));
        
        public IShopRepository ShopRepository => _shopRepository ?? (_shopRepository = new ShopRepository(_context));

        public IWorkCenterRepository WorkCenterRepository => _workCenterRepository ?? (_workCenterRepository = new WorkCenterRepository(_context));
        
        public IWorkCenterControlRepository WorkCenterControlRepository => _workCenterControlRepository ?? (_workCenterControlRepository = new WorkCenterControlRepository(_context));
        
        public IProductRepository ProductRepository => _productRepository ?? (_productRepository = new ProductRepository(_context));
        
        public IStockRepository StockRepository => _stockRepository ?? (_stockRepository = new StockRepository(_context));

        public int? UserId { get { return _context._currentUserId; } }

        public int CommitChanges()
        {
            return _context.CommitChanges();
        }

        public Task<int> CommitChangesAsync()
        {
            return _context.CommitChangesAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }

        ISubcategoryRepository SubcategoryRepository { get; }

        IProviderRepository ProviderRepository { get; }

        IStateRepository StateRepository { get; }

        IShopRepository ShopRepository { get; }

        IWorkCenterRepository WorkCenterRepository { get; }

        IProductRepository ProductRepository { get; }

        IStockRepository StockRepository { get; }

        IWorkCenterControlRepository WorkCenterControlRepository { get; }

        int? UserId { get; }

        int CommitChanges();

        Task<int> CommitChangesAsync();
    }
}
