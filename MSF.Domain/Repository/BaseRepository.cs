using MSF.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MSF.Domain.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected MSFDbContext Context { get; set; }
        private readonly DbSet<T> _set;

        protected BaseRepository(IMSFDbContext context)
        {
            Context = context as MSFDbContext;
            _set = Context.Set<T>();
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            var query = All().Where(expression);
            foreach (var item in query)
            {
                Delete(item);
            }
        }

        public virtual void Delete(T item)
        {
            if (Context.Entry(item).State == EntityState.Detached)
            {
                _set.Attach(item);
            }

            if (item != null)
                _set.Remove(item);
        }

        public void DeleteAll()
        {
            var query = All();
            foreach (var item in query)
            {
                Delete(item);
            }
        }

        public void Delete(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return All().FirstOrDefault(expression);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> expression)
        {
            return All().SingleOrDefault(expression);
        }

        public T Single(Expression<Func<T, bool>> expression)
        {
            return All().Single(expression);
        }

        protected virtual IQueryable<T> All()
        {
            return _set.AsQueryable();
        }

        public virtual void Add(T item)
        {
            if (!_set.Local.Contains(item))
                _set.Add(item);
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public virtual void Update(int id, T item)
        {
            var db = Find(id);
            Context.Entry(db).CurrentValues.SetValues(item);
        }

        public virtual void Detach(T item)
        {
            Context.Entry(item).State = EntityState.Detached;
        }
        public virtual T Find(int? id)
        {
            return _set.Find(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<List<T>> AllAsync() => _set.ToListAsync();

        public abstract IOrderedQueryable<T> AllOrdered();

        public Task<T> FindAsync(int? id) => _set.FindAsync(id);

        public Task<bool> AnyAsync(Expression<Func<T, bool>> expression) => _set.AnyAsync(expression);
    }

    public interface IBaseRepository<T> : IDisposable where T : class
    {
        void Delete(Expression<Func<T, bool>> expression);

        void Delete(T item);

        void DeleteAll();

        void Delete(IEnumerable<T> items);

        T FirstOrDefault(Expression<Func<T, bool>> expression);

        T SingleOrDefault(Expression<Func<T, bool>> expression);

        Task<List<T>> AllAsync();

        IOrderedQueryable<T> AllOrdered();

        T Single(Expression<Func<T, bool>> expression);

        Task<T> FindAsync(int? id);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        void Add(T item);

        void Update(int id, T item);

        T Find(int? id);
    }
}
