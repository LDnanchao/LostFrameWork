using UnityEngine;

namespace LostFramework
{
    //用于库存系统
    public class InventoryModel
    {
        public int id;
    }

    /// <summary>
    /// 这里的数据一旦改变，就会传播改变事件
    /// </summary>
    public class InventoryViewModel
    {
        
        public int id;
        public int num;
    }

    public class InventoryView:MonoBehaviour
    {
        
    }
}