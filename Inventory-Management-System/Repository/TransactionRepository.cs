using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repository.Interfaces;

namespace Inventory_Management_System.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository, IGenericRepository<Transaction>
    {
        public TransactionRepository(InventoryContext context): base(context)
        { }
    }
}
