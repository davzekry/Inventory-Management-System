using Inventory_Management_System.Data;
using Inventory_Management_System.DTO.Products;
using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    [Route("api/[Action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public ProductController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult> Index(int page=1, int amount=10)
        {
            IEnumerable<DTOProductDetails> products = serviceUnitOfWork.ProductService.GetAllProducts(page,amount);
           
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductDetails(int id)
        {
            DTOProductDetails product = serviceUnitOfWork.ProductService.GetProductById(id);
            if (product == null)
                return BadRequest();
            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult> AddProduct(DTOAddProduct product)
        {
            await serviceUnitOfWork.ProductService.AddProduct(product);
            serviceUnitOfWork.save();

            return Ok("Product Added Successfully!");
        }


        [HttpPut("{Id:int}")]
        public async Task<ActionResult> EditProduct(int Id, DTOEditProduct product)
        {
            bool success = await serviceUnitOfWork.ProductService.EditProduct(Id, product);
            if(!success)
                return BadRequest();

            serviceUnitOfWork.save();
            return Ok(new {Message = "Product Edited Successfully!"});
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            bool success = await serviceUnitOfWork.ProductService.DeleteProduct(id);
            if (!success)
                return BadRequest(new {Error = "Product Not Found!" });
            serviceUnitOfWork.save();
            return Ok("Product Deleted Successfully!");
        }


    }
}
