using System.Text;
using Inventory_Management_System.Data;
using Inventory_Management_System.DTO.BackGround;
using Inventory_Management_System.Models;
using Inventory_Management_System.Service.Interfaces;

namespace Inventory_Management_System.Service
{
    public class BackGroundService : IBackGroundService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;

        public BackGroundService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }

        public async Task CheckLowStockProductsAsync()
        {
            int size = 100;
            int skip = 0;
            List<DTOLowStockProducts> products;
            int count = 1;
            // Mail message 
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"The following products are below their stock threshold:");
            messageBuilder.AppendLine();

            // check products and adding them to the mail body
            do
            {
                products = unitOfWork.ProductRepo
                    .GetAllWithFilter(x => x.Quantity < x.LowStockThreshold)
                    .Select(x => new DTOLowStockProducts
                        {
                            Name = x.Name,
                            Quantity = x.Quantity,
                            LowStockThreshold = x.LowStockThreshold,
                        })
                    .Skip(skip)
                    .Take(size)
                    .ToList();

                                
                foreach (var p in products)
                {
                    messageBuilder.AppendLine($"{count}- {p.Name}: Current Quantity = {p.Quantity}, Threshold = {p.LowStockThreshold}");
                    count++;
                }
                skip += size;

            }while(products.Any());

            if (count == 1)
                return;

            // trigger email if Quantity Below Threshold
            string message = messageBuilder.ToString();

            IQueryable<string> emails = unitOfWork.UserRepo
                                      .GetAllWithFilter(x => true)
                                      .Select(x => x.Email);
                                      
            await emailService.SendEmailsAsync(emails, $"Daily Report of Low Stock Products", message);
        }


        public async Task ArchiveTransactionData()
        {
            int size = 100;
           
            do
            {
                List<TransactionArchive> oldTransactions = unitOfWork.TransactionRepo
                                                      .GetAllWithFilter(x => x.Date < DateTime.Now.AddDays(-365.25))
                                                      .Select(x => new TransactionArchive
                                                          {
                                                              Id = x.Id,
                                                              Amount = x.Amount,
                                                              AppUserId = x.AppUserId,
                                                              Date = x.Date,
                                                              ProductId = x.ProductId,
                                                              TransactionTypeId = x.TransactionTypeId
                                                          })
                                                      .Take(size)
                                                      .ToList();

                if (!oldTransactions.Any())
                    break;

                foreach (var t in oldTransactions)
                {
                    await unitOfWork.TransactionArchiveRepo
                                    .AddAsync(t);
                    unitOfWork.TransactionRepo
                              .Delete(x => x.Id == t.Id);
                }
            } while (true);
                    
        }

    }
}

