namespace Inventory_Management_System.Models
{
    public class TransactionArchive
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public int ProductId { get; set; }
        public int TransactionTypeId { get; set; }

        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
