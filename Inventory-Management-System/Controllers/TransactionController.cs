using System.Security.Claims;
using Inventory_Management_System.Data;
using Inventory_Management_System.DTO.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    [Authorize]
    [Route("api/[Action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public TransactionController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> AddStock(DTOAddTransaction transaction)
        {
            try
            {
                transaction.AppUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                await serviceUnitOfWork.TransactionService.AddStockAsync(transaction);
            
                serviceUnitOfWork.save();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveStock(DTORemoveTransaction transaction)
        {
            try
            {
                transaction.AppUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                bool success = await serviceUnitOfWork.TransactionService.RemoveStockAsync(transaction);

                serviceUnitOfWork.save();
                return Ok(transaction);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }

        [HttpPost]
        public async Task<ActionResult> TransferStock(DTOTransferTransaction transaction)
        {
            try
            {
                transaction.AppUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                await serviceUnitOfWork.TransactionService.TransferStockAsync(transaction);
                
                serviceUnitOfWork.save();
                return Ok(transaction);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
