using Inventory_Management_System.DTO.Products;
using Inventory_Management_System.DTO.Reports;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Service
{
    public interface IProductService
    {
        public IEnumerable<DTOProductDetails> GetAllProducts(int page = 1, int amount = 10);
        public DTOProductDetails GetProductById(int Id);
        public Task<bool> DeleteProduct(int Id);
        public Task AddProduct(DTOAddProduct product);
        public Task<bool> EditProduct(int id, DTOEditProduct product);
        public IEnumerable<DTOProductBelowThreshold> GetBelowThresholdProducts(int page, int noOfItems, int? catId=0);

    }
}