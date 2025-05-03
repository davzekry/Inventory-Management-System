using Inventory_Management_System.Data;
using Inventory_Management_System.DTO.Products;
using Inventory_Management_System.DTO.Reports;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        // 1- Show All
        public IEnumerable<DTOProductDetails> GetAllProducts(int page, int noOfItems)
        {
            return unitOfWork.ProductRepo
                .GetAllWithFilter(x=>true)
                .Skip((page - 1) * noOfItems).Take(noOfItems)
                .Select(x => new DTOProductDetails
                {
                    Id = x.Id,
                    Category = x.Category.Name,
                    Description = x.Description,
                    LowStockThreshold = x.LowStockThreshold,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity
                });
        }

        // 2- Show one product
        public DTOProductDetails GetProductById(int Id)
        {
            return unitOfWork.ProductRepo
                .GetAllWithFilter(x => x.Id == Id)
                .Select(x=> new DTOProductDetails
                {
                    Id = x.Id,
                    Category = x.Category.Name,
                    Description = x.Description,
                    LowStockThreshold = x.LowStockThreshold,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity
                }).FirstOrDefault();
        }

        // 3- Add product
        public async Task AddProductAsync(DTOAddProduct product)
        {
            await unitOfWork.ProductRepo.AddAsync(new Product
            {
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Quantity = product.Quantity,
                LowStockThreshold = product.LowStockThreshold
            });
        }

        // 4- Edit product
        public async Task<bool> EditProductAsync(int Id, DTOEditProduct product)
        {
            return await unitOfWork.ProductRepo
                .UpdateAsync(x=>x.Id == Id, new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    LowStockThreshold = product.LowStockThreshold
                });
        }

        // 5- Delete product
        public async Task<bool> DeleteProductAsync(int Id)
        {
            return await unitOfWork.ProductRepo
                .Delete(x => x.Id == Id);
        }

        // 3- Show one product
        public IEnumerable<DTOProductBelowThreshold> GetBelowThresholdProducts(int page, int noOfItems, int? catId = 0)
        {
            return unitOfWork.ProductRepo
                .GetAllWithFilter(x => x.Quantity < x.LowStockThreshold && 
                                  (catId == 0 ? true : x.CategoryId == catId))
                .Skip((page - 1) * noOfItems).Take(noOfItems)
                .Select(x=>new DTOProductBelowThreshold
                {
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    LowStockThreshold = x.LowStockThreshold,
                    Description = x.Description
                }).ToList();
        }
    }
}
