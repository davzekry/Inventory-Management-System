using Inventory_Management_System.Service;
using Inventory_Management_System.Service.Interfaces;

namespace Inventory_Management_System.Data
{
    public interface IServiceUnitOfWork
    {
        public IProductService ProductService { get; }
        public ITransactionService TransactionService {  get; }
        //public IEmailService EmailService {  get; }
        public void save();
    }
}