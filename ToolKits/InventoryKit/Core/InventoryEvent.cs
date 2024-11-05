using UnityEngine;

namespace LostFramework
{
    public enum InventoryEventType
    {
        Select, 
        Click, 
        Move, 
        Drop, 
        Destroy, 
        Error, 
        /// <summary>
        /// 重新绘制
        /// </summary>
        Redraw, 
        /// <summary>
        /// 内容变化
        /// </summary>
        ContentChanged, 
    }
    public struct InventoryEvent
    {
        /// the type of event
        public InventoryEventType InventoryEventType;
        /// the name of the inventory where the event happened
        public string TargetInventoryName;
        /// the item involved in the event
        public InventoryItem EventItem;
        /// the quantity involved in the event
        public int Quantity;
        /// the index inside the inventory at which the event happened
        public int Index;
        /// the unique ID of the player triggering this event
        public string PlayerID;

        public InventoryEvent(InventoryEventType eventType, string targetInventoryName, InventoryItem eventItem, int quantity, int index, string playerID)
        {
            InventoryEventType = eventType;
            TargetInventoryName = targetInventoryName;
            EventItem = eventItem;
            Quantity = quantity;
            Index = index;
            PlayerID = (playerID != "") ? playerID : "Player1";
        }

        static InventoryEvent e;
        public static void Trigger(InventoryEventType eventType, string targetInventoryName, InventoryItem eventItem, int quantity, int index, string playerID)
        {
            e.InventoryEventType = eventType;
            e.TargetInventoryName = targetInventoryName;
            e.EventItem = eventItem;
            e.Quantity = quantity;
            e.Index = index;
            e.PlayerID = (playerID != "") ? playerID : "Player1";
            LEventManager.TriggerEvent(e);
        }
    }
}