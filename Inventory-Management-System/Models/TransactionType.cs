using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Management_System.Models
{
    public class TransactionType
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public ICollection<Transaction> Transactions { get; set; }
    }
}