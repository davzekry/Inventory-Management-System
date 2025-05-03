using Inventory_Management_System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackGroundJobsController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;
        public BackGroundJobsController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        public ActionResult CkeckLowStockProducts()
        {
            serviceUnitOfWork.ProductService.
        }
    }
}
