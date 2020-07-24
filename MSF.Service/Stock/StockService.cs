﻿using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System.Threading.Tasks;

namespace MSF.Service.Stock
{
    public class StockService : BaseService, IStockService
    {
        public StockService(IUnitOfWork unit) : base(unit) { }

        public async Task<int> AddAsync(Domain.Models.Stock stock)
        {
            _unit.StockRepository.Add(stock);
            return await _unit.CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(Domain.Models.Stock stock)
        {
            _unit.StockRepository.Delete(stock);
            return await _unit.CommitChangesAsync();
        }

        public Task<Domain.Models.Stock> FindAsync(int? id) =>
            _unit.StockRepository.FindAsync(id);

        public Task<LazyStocksViewModel> LazyStocksViewModelAsync(string filter, int take, int skip) =>
            _unit.StockRepository.LazyStocksViewModelAsync(filter, take, skip);

        public async Task<int> UpdateAsync(Domain.Models.Stock stock)
        {
            _unit.StockRepository.Update(stock.Id, stock);
            return await _unit.CommitChangesAsync();
        }
    }

    public interface IStockService
    {
        Task<LazyStocksViewModel> LazyStocksViewModelAsync(string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Stock stock);

        Task<int> DeleteAsync(Domain.Models.Stock stock);

        Task<int> UpdateAsync(Domain.Models.Stock stock);

        Task<Domain.Models.Stock> FindAsync(int? id);
    }
}
