using System.Collections.Generic;

namespace Lost
{
    public interface IInventory
    {
        /// <summary>
        /// 库存拥有者，可以通过id和name进行定位
        /// </summary>
        public string PlayerID { get; set; }
        /// <summary>
        /// 库存名称，可以通过id和name进行定位
        /// </summary>
        public string InventoryName{ get; set; }
        ///是否允许自动移除空位
        public bool AutoRemoveEmpty { get; set; }
        ///是否为无限制空间
        public bool Unlimited { get; set; }

        public List<InventoryItem> Items { get; }

        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <param name="quantity">数量</param>
        /// <returns></returns>
        public bool AddItem(InventoryItem itemToAdd, int quantity);
        /// <summary>
        /// 添加道具到指定位置
        /// </summary>
        public bool AddItemAt(InventoryItem itemToAdd, int quantity, int destinationIndex);
        /// <summary>
        /// 移动道具
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public bool MoveItem(int startIndex, int endIndex);
        /// <summary>
        /// 移动道具到目标背包
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="targetInventory"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public bool MoveItemToInventory(int startIndex, Inventory targetInventory, int endIndex = -1);
        /// <summary>
        /// 移除道具
        /// </summary>
        /// <param name="i"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool RemoveItem(int i, int quantity);
        /// <summary>
        /// 通过id 移除道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool RemoveItemByID(string itemID, int quantity);
        /// <summary>
        /// 销毁道具
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool DestroyItem(int i);
        /// <summary>
        /// 清空背包
        /// </summary>
        public void EmptyInventory();
        /// <summary>
        /// 返回可以添加到此库存的特定项目的最大值，而不会超过该项目上定义的最大数量
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <param name="newQuantity"></param>
        /// <returns></returns>
        public int CapMaxQuantity(InventoryItem itemToAdd, int newQuantity);
        /// <summary>
        /// 调整数组大小
        /// </summary>
        /// <param name="newSize"></param>
        public void ResizeArray(int newSize);
        /// <summary>
        /// 获得道具的总数量
        /// </summary>
        /// <param name="searchedItemID"></param>
        /// <returns></returns>
        public int GetQuantity(string searchedItemID);
        /// <summary>
        /// 获得所有匹配的道具的下标
        /// </summary>
        /// <param name="searchedItemID"></param>
        /// <returns></returns>
        public List<int> InventoryContains(string searchedItemID);
        /// <summary>
        /// 获得所有匹配的道具的下标
        /// </summary>
        /// <param name="searchedClass"></param>
        /// <returns></returns>
        public List<int> InventoryContains(ItemClasses searchedClass);



    }
}