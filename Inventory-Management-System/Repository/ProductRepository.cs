using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repository.Interfaces;

namespace Inventory_Management_System.Repository
{
    public class ProductRepository:GenericRepository<Product>, IProductRepository, IGenericRepository<Product>
    {
        public ProductRepository(InventoryContext context):base(context) { }

    }
}
