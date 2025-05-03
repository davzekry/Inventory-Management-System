using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repository.Interfaces;

namespace Inventory_Management_System.Repository
{
    public class TransactionArchiveRepository : GenericRepository<TransactionArchive>, ITransactionArchiveRepository, IGenericRepository<TransactionArchive>
    {
        public TransactionArchiveRepository(InventoryContext inventoryContext):base(inventoryContext) { }
        
    }
}
