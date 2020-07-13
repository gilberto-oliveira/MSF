using MSF.Domain.UnitOfWork;
using System;

namespace MSF.Service.Base
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IUnitOfWork _unit;

        public BaseService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public interface IBaseService : IDisposable { }
}
