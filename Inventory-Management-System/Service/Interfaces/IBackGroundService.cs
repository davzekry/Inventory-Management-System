namespace Inventory_Management_System.Service.Interfaces
{
    public interface IBackGroundService
    {
        public Task CheckLowStockProductsAsync();
        public Task ArchiveTransactionData();

    }
}