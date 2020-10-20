using MSF.Domain.UnitOfWork;
using MSF.Domain.ViewModels;
using MSF.Service.Base;
using System;
using System.Threading.Tasks;

namespace MSF.Service.Operation
{
    public class OperationService : BaseService, IOperationService
    {
        public OperationService(IUnitOfWork unit) : base(unit) { }

        public async Task<int> AddAsync(Domain.Models.Operation operation)
        {
            await ValidadeOperation(operation);
            _unit.OperationRepository.Add(operation);
            await UpdateStockOnInsert(operation);
            return await _unit.CommitChangesAsync();
        }

        private async Task ValidadeOperation(Domain.Models.Operation operation)
        {
            if (operation.Amount <= 0)
                throw new ArgumentException("A quantidade selecionada deve ser maior que zero!");

            if (operation.ProductId <= 0)
                throw new ArgumentException("Um produto deve ser selecionado!");

            if (operation.ProviderId <= 0)
                throw new ArgumentException("Um fornecedor deve ser selecionado!");

            if (operation.UnitPrice <= 0)
                throw new ArgumentException("O preço deve ser maior que zero!");

            var available = await _unit.StockRepository.HasAvailable(operation.ProductId, operation.ProviderId);
            if (!available)
                throw new ArgumentException("Estoque indisponível para o produto e fornecedor selecionados!");
        }

        private async Task UpdateStockOnInsert(Domain.Models.Operation operation)
        {
            var currentStock = await _unit.StockRepository.FindAvailableByProductAndProvider(operation.ProductId, operation.ProviderId);
            if (currentStock != null)
            {
                if (currentStock.Amount >= operation.Amount)
                {
                    currentStock.Amount -= operation.Amount;
                    _unit.StockRepository.Update(currentStock.Id, currentStock);
                }
                else
                {
                    throw new ArgumentException("Saldo insuficiente!");
                }
            }
            else
            {
                throw new Exception("Estoque inexistente para o produto e fornecedor selecionados!");
            }
        }

        public async Task<int> DeleteAsync(Domain.Models.Operation operation)
        {
            _unit.OperationRepository.Delete(operation);
            await UpdateStockOnDelete(operation);
            return await _unit.CommitChangesAsync();
        }

        private async Task UpdateStockOnDelete(Domain.Models.Operation operation)
        {
            var currentStock = await _unit.StockRepository.FindAvailableByProductAndProvider(operation.ProductId, operation.ProviderId);
            if (currentStock != null)
            {
                currentStock.Amount += operation.Amount;
                _unit.StockRepository.Update(currentStock.Id, currentStock);
            }
            else
            {
                throw new Exception("Estoque inexistente para o produto e fornecedor selecionados!");
            }
        }

        public Task<Domain.Models.Operation> FindAsync(int? id)
        => _unit.OperationRepository.FindAsync(id);

        public async Task<LazyOperationViewModel> LazyViewModelAsync(int workCenterControlId, string type, string filter, int take, int skip)
        {
            return await _unit.OperationRepository.LazyViewModelAsync(workCenterControlId, type, filter, take, skip);
        }

        public async Task<int> UpdateAsync(Domain.Models.Operation operation)
        {
            await ValidadeOperation(operation);
            await UpdateStockOnUpdate(operation);
            _unit.OperationRepository.Update(operation.Id, operation);
            return await _unit.CommitChangesAsync();
        }

        private async Task UpdateStockOnUpdate(Domain.Models.Operation operation)
        {
            var currentStock = await _unit.StockRepository.FindAvailableByProductAndProvider(operation.ProductId, operation.ProviderId);
            if (currentStock != null)
            {
                var lastOperation = await _unit.OperationRepository.FindByProductAndProvider(operation.Id, operation.ProductId, operation.ProviderId);
                
                if (lastOperation != null)
                {
                    if (operation.Amount <= currentStock.Amount + lastOperation.Amount)
                    {
                        var amount = lastOperation.Amount - operation.Amount;
                        currentStock.Amount += amount;
                        _unit.StockRepository.Update(currentStock.Id, currentStock);
                    }
                    else
                    {
                        throw new Exception("Saldo insuficiente para o produto e fornecedor selecionados!");
                    }
                }
                else
                {
                    throw new ArgumentException("Não é possível alterar produto/fornecedor ao editar operação!");
                }
            }
            else
            {
                throw new Exception("Estoque inexistente para o produto e fornecedor selecionados!");
            }
        }

        public Task<decimal> FindTotalPriceByWorkCenterControlAndTypeAsync(int workCenterControlId, string type)
        => _unit.OperationRepository.FindTotalPriceByWorkCenterControlAndTypeAsync(workCenterControlId, type);
    }

    public interface IOperationService
    {
        Task<LazyOperationViewModel> LazyViewModelAsync(int workCenterId, string type, string filter, int take, int skip);

        Task<int> AddAsync(Domain.Models.Operation operation);

        Task<int> DeleteAsync(Domain.Models.Operation operation);

        Task<int> UpdateAsync(Domain.Models.Operation operation);

        Task<Domain.Models.Operation> FindAsync(int? id);

        Task<decimal> FindTotalPriceByWorkCenterControlAndTypeAsync(int workCenterControlId, string type);
    }
}
