namespace Lost
{
    public interface IInventorySlot
    {
        public void UpdateItem(InventoryItem inventoryItem, int quantity);
        
        public void Clear();

        public void ShowItem(InventoryItem inventoryItem);
    }
}