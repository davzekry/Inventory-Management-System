namespace Inventory_Management_System.DTO.Reports
{
    public class DTOProductBelowThreshold
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int LowStockThreshold { get; set; }
        public int CategoryId { get; set; }

    }
}
