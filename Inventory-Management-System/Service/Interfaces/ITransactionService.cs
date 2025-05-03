using Inventory_Management_System.DTO.Reports;
using Inventory_Management_System.DTO.Transactions;

namespace Inventory_Management_System.Service.Interfaces
{
    public interface ITransactionService
    {
        public Task<bool> AddStock(DTOAddTransaction transaction);
        public Task<bool> RemoveStock(DTORemoveTransaction transaction);
        public Task<bool> TransferStock(DTOTransferTransaction transaction);
        public IEnumerable<DTOTransactionHistory> GetTransactionsWithFilter(int? productId, int? categoryId, int? transactionTypeId, DateTime? startDate, DateTime? endDate, int page, int noOfItems);

    }
}