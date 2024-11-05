namespace LostFramework
{
    public interface IInventoryDisplay
    {
        public IInventory inventory{get;set;}
        /// <summary>
        /// 刷新仓库显示
        /// </summary>
        public void UpdateInventory();
        /// <summary>
        /// 局部内容变化
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="quantity"></param>
        public void OnInventoryContentChanged(InventoryItem item,int index, int quantity);
    }
}