using System;

namespace Lost
{
    /// <summary>
    /// 背包Item
    /// </summary>
    public class InventoryItem
    {
        //id
        public string ItemID = String.Empty;
        //最大数量
        public int MaxQuantity;
        //当前数量
        public int Quantity;
        public ItemClasses ItemClass;
        public IInventory OwnInventory;
        public int Index;
        public InventoryItem(){}
        public  InventoryItem(string itemID, int maxQuantity)
        {
            ItemID = itemID;
            MaxQuantity = maxQuantity;
        }

        public virtual InventoryItem Copy()
        {
            InventoryItem tempItem = new InventoryItem();
            tempItem.ItemID = ItemID;
            tempItem.MaxQuantity = MaxQuantity;
            return  tempItem;
        }
    }
}