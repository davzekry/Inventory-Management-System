using System.Collections.Specialized;
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
            List<DTOLowStockProducts> products = unitOfWork.ProductRepo
                .GetAllWithFilter(x => x.Quantity < x.LowStockThreshold)
                .Select(x => new DTOLowStockProducts
                {
                    Name = x.Name,
                    Quantity = x.Quantity,
                    LowStockThreshold = x.LowStockThreshold,
                }).ToList();

            int NoOfItems = products.Count;
            if (NoOfItems == 0)
                return;

            // trigger email if Quantity Below Threshold
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"{NoOfItems} products are below their stock threshold:");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine();
            int count = 1;
            foreach (var p in products)
            {
                messageBuilder.AppendLine($"{count}- {p.Name}: Current Quantity = {p.Quantity}, Threshold = {p.LowStockThreshold}");
                count++;
            }

            string message = messageBuilder.ToString();

            List<string> emails = unitOfWork.UserRepo
                                      .GetAllWithFilter(x => true)
                                      .Select(x => x.Email)
                                      .ToList();

            await emailService.SendEmailsAsync(emails, $"Daily Report of Low Stock Products", message);
        }

    }
}

