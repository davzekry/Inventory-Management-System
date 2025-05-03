using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Data
{
    public class InventoryContext: IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products {  get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<TransactionArchive> TransactionArchives { get; set; }


        public InventoryContext(DbContextOptions<InventoryContext> options):base(options) { }
        
    }
}
