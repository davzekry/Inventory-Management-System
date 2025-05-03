using Inventory_Management_System.Models;
using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.Interfaces;

namespace Inventory_Management_System.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryContext context;
        private IProductRepository _ProductRepo;
        private ITransactionRepository _TransactionRepo;
        private IUserRepository _UserRepo;

        public UnitOfWork(InventoryContext ctx)
        {
            context = ctx;
        }


        public IProductRepository ProductRepo
        {
            get
            {
                if (_ProductRepo == null)
                    _ProductRepo = new ProductRepository(context);
                return _ProductRepo;
            }
        }

        public ITransactionRepository TransactionRepo
        {
            get
            {
                if (_TransactionRepo == null)
                    _TransactionRepo = new TransactionRepository(context);
                return _TransactionRepo;
            }
        }

        public IUserRepository UserRepo
        {
            get
            {
                if (_UserRepo == null)
                    _UserRepo = new UserRepository(context);
                return _UserRepo;
            }
        }
        public void save()
        {
            context.SaveChanges();
        }




    }
}
