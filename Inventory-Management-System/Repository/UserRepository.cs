using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Inventory_Management_System.Repository.Interfaces;

namespace Inventory_Management_System.Repository
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(InventoryContext context):base(context) { }
    }
}
