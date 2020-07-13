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

        private readonly IMSFDbContext _context;
        private bool _disposed;

        public UnitOfWork(IMSFDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository CategoryRepository => _categoryRepository ?? (_categoryRepository = new CategoryRepository(_context));

        public ISubcategoryRepository SubcategoryRepository => _subcategoryRepository ?? (_subcategoryRepository = new SubcategoryRepository(_context));

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

        int? UserId { get; }

        int CommitChanges();

        Task<int> CommitChangesAsync();
    }
}
