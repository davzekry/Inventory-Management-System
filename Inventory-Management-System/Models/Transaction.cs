using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Management_System.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public int ProductId { get; set; }
        public int TransactionTypeId { get; set; }
        
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;


        public AppUser AppUser { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("TransactionTypeId")]
        public TransactionType? TransactionType { get; set; }
        }
}