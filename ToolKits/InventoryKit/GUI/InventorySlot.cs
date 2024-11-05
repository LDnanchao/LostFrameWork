using UnityEngine;

namespace LostFramework
{
    /// <summary>
    /// 不持有数据，由InventoryDisplay进行管理
    /// </summary>
    public  class InventorySlot:MonoBehaviour,IInventorySlot
    {
        public IInventory inventory;
        public InventoryItem item;
        public virtual void UpdateItem(InventoryItem inventoryItem, int quantity)
        {
            item = inventoryItem;
        }

        public virtual void Clear()
        {
            item = null;
            inventory = null;
        }

        public virtual void ShowItem(InventoryItem inventoryItem)
        {
            item = inventoryItem;
        }
    }
}