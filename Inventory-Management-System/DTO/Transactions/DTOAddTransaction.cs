namespace Inventory_Management_System.DTO.Transactions
{
    public class DTOAddTransaction
    {
        public string? AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
