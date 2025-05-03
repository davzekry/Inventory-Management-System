using Inventory_Management_System.Data;
using Inventory_Management_System.DTO.Reports;
using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;
        public ReportController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpGet]
        public ActionResult GetBelowThresholdProducts(int? CategoryId, int page = 1, int count = 10)
        {
            IEnumerable<DTOProductBelowThreshold> products = serviceUnitOfWork.ProductService.GetBelowThresholdProducts(page, count, CategoryId);
            return Ok(products);
        }

        [HttpGet]
        public ActionResult GetTransactionHistory(int? productId, int? categoryId, int? transactionTypeId, DateTime? startDate, DateTime? endDate, int page = 1, int noOfItems = 10)
        {
            IEnumerable<DTOTransactionHistory> Transactions = serviceUnitOfWork.TransactionService
                        .GetTransactionsWithFilter(productId, categoryId, transactionTypeId, startDate, endDate, page, noOfItems);
            return Ok(Transactions);
        }
    }
}
