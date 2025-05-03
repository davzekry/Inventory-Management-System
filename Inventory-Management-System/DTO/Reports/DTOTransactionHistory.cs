namespace Inventory_Management_System.DTO.Reports
{
    public class DTOTransactionHistory
    {
        public string AppUserName { get; set; }
        public string ProductName { get; set; }
        public string TransactionType { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
