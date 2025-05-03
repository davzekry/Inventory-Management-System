using Inventory_Management_System.Repository.Interfaces;

namespace Inventory_Management_System.Data
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepo { get; }
        public ITransactionRepository TransactionRepo {  get; }
        public IUserRepository UserRepo {  get; }

        public void save();
    }
}