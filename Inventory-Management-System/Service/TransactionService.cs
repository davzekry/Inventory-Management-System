using Inventory_Management_System.Data;
using Inventory_Management_System.DTO.Reports;
using Inventory_Management_System.DTO.Transactions;
using Inventory_Management_System.Models;
using Inventory_Management_System.Service.Interfaces;

namespace Inventory_Management_System.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork unitOfWork;
        public IEmailService EmailService { get; }
        //private IServiceUnitOfWork serviceUnitOfWork;

        public TransactionService(IUnitOfWork unitOfWork, IEmailService emailService)//, IServiceUnitOfWork serviceUnitOfWork)
        {
            this.unitOfWork = unitOfWork;
            EmailService = emailService;
            //this.serviceUnitOfWork = serviceUnitOfWork;
        }


        public async Task<bool> AddStockAsync(DTOAddTransaction transaction)
        {
            Product product = await unitOfWork.ProductRepo
                                   .GetItemAsync(x=>x.Id == transaction.ProductId);
            if (product == null)
                throw new Exception("Invalid Product");

            product.Quantity += transaction.Amount;

            await unitOfWork.TransactionRepo.AddAsync(new Transaction
            {
                ProductId = transaction.ProductId,
                TransactionTypeId = 1,
                Amount = transaction.Amount,
                Date = transaction.Date,
                AppUserId = transaction.AppUserId
            });

            // trigger email if Quantity Below Threshold
            if (product.Quantity < product.LowStockThreshold)
            {
                //serviceUnitOfWork.EmailService.SendEmailAsync(); 
                string message = $"Product: {product.Name} Need to be ReStock Current Quantity is {product.Quantity}, Threshold is {product.LowStockThreshold}";

                IQueryable<string> emails = unitOfWork.UserRepo
                                      .GetAllWithFilter(x => true)
                                      .Select(x => x.Email);
                                      
                EmailService.SendEmailsAsync(emails, $"Shortage Product({product.Name}", message);
            }

            return true;
        }

        public async Task<bool> RemoveStockAsync(DTORemoveTransaction transaction)
        {
            Product? product = await unitOfWork.ProductRepo
                                    .GetItemAsync(x => x.Id == transaction.ProductId);
            if (product == null)
                throw new Exception("Invalid Product");

            product.Quantity -= transaction.Amount;
            if (product.Quantity < 0)
                throw new Exception($"Transaction Amount is Higher than Storage {product.Quantity}");
                        
            await unitOfWork.TransactionRepo.AddAsync(new Transaction
            {
                ProductId = transaction.ProductId,
                TransactionTypeId = 2,
                Amount = transaction.Amount,
                Date = transaction.Date,
                AppUserId = transaction.AppUserId
            });

            // trigger email if Quantity Below Threshold
            if (product.Quantity < product.LowStockThreshold)
            {
                //serviceUnitOfWork.EmailService.SendEmailAsync(); 
                string message = $"Product: {product.Name} Need to be ReStock Current Quantity is {product.Quantity}, Threshold is {product.LowStockThreshold}";

                IQueryable<string> emails = unitOfWork.UserRepo
                                     .GetAllWithFilter(x => true)
                                     .Select(x => x.Email);

                EmailService.SendEmailsAsync(emails, $"Shortage Product({product.Name}", message);
            }

            return true;
        }

        public async Task<bool> TransferStockAsync(DTOTransferTransaction transaction)
        {
            Product product = await unitOfWork.ProductRepo
                                    .GetItemAsync(x => x.Id == transaction.ProductId);
            if (product == null)
                throw new Exception("Invalid Product");

            if (product.Quantity < transaction.Amount)
                throw new Exception($"Transaction Amount is Higher than Storage {product.Quantity}");
            
            await unitOfWork.TransactionRepo.AddAsync(new Transaction
            {
                ProductId = transaction.ProductId,
                TransactionTypeId = 3,
                Amount = transaction.Amount,
                Date = transaction.Date,
                AppUserId = transaction.AppUserId
            });
                       
            return true;
        }


        public IEnumerable<DTOTransactionHistory> GetTransactionsWithFilter(int? productId, int? categoryId, int? transactionTypeId, DateTime? startDate, DateTime? endDate, int page, int noOfItems)
        {
            return unitOfWork.TransactionRepo
                    .GetAllWithFilter(x =>
                    (!productId.HasValue || x.ProductId == productId) &&
                    (!categoryId.HasValue || x.Product.CategoryId == categoryId) &&
                    (!transactionTypeId.HasValue || x.TransactionTypeId == transactionTypeId) &&
                    (!startDate.HasValue || x.Date >= startDate) &&
                    (!endDate.HasValue || x.Date <= endDate)
                    ).Skip((page - 1) * noOfItems)
                    .Take(noOfItems)
                    .Select(x=> new DTOTransactionHistory
                    {
                        Amount = x.Amount,
                        AppUserName = x.AppUser.UserName,
                        Date = x.Date,
                        ProductName = x.Product.Name,
                        TransactionType = x.TransactionType.Name
                    });
                         
        }

 
    }
}
