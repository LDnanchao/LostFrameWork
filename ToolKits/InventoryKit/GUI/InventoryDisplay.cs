using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LostFramework
{
    public class InventoryDisplay:MonoBehaviour,IInventoryDisplay,IEventListener<InventoryEvent>
    {
        public List<InventorySlot> SlotContainer { get;protected set; }
        public InventorySlot prefabSlot;
        private ObjectPool<InventorySlot> slotPool;
        private IInventory _inventory;
        public IInventory inventory
        {
            get=>_inventory;
            set
            {
                _inventory = value;
                if (inventory != null)
                {
                    UpdateInventory();
                }
            }
        }

        public virtual void Awake()
        {
            SlotContainer = new List<InventorySlot>();
            slotPool = new ObjectPool<InventorySlot>(() =>
                {
                    var newItem = Instantiate(prefabSlot, prefabSlot.transform.parent, true);
                    newItem.gameObject.SetActive(false);
                    newItem.transform.SetAsLastSibling();
                    newItem.Clear();
                    return newItem;
                },
                (item) =>
                {
                    item.gameObject.SetActive(false);
                    item.transform.SetAsLastSibling();
                    item.Clear();
                });
            if (inventory != null)
            {
                UpdateInventory();
            }
        }
        
       
        public virtual void OnEnable()
        {
	        this.EventStartListening();
        }

        public virtual void OnDisable()
        {
	        this.EventStopListening();
        }
        
        public void OnLEvent(InventoryEvent eventType)
        {
            if(inventory==null)
                return;
            if(inventory.InventoryName != eventType.TargetInventoryName || inventory.PlayerID != eventType.PlayerID)
                return;
            switch (eventType.InventoryEventType)
            {
                case InventoryEventType.ContentChanged:
                    OnInventoryContentChanged(eventType.EventItem, eventType.Index, eventType.Quantity);
                    break;
                case InventoryEventType.Redraw:
                    UpdateInventory();
                    break;
                default:
                    break;
            }
        }
        public void UpdateInventory()
        {
            prefabSlot.gameObject.SetActive(false);
            //刷新库存UI，同时会整理GO
            if(inventory==null)
                return;
            //多余的slot进行释放
            if (SlotContainer.Count > inventory.Items.Count)
            {
                for (int i = SlotContainer.Count - 1; i >= inventory.Items.Count; i--)
                {
                    slotPool.Release(SlotContainer[i]);
                    SlotContainer.RemoveAt(i);
                }
            }
            
            //以此渲染inventory的数据
            for (int i = 0; i < inventory.Items.Count; i++)
            {
                if (i >= SlotContainer.Count)
                {
                    var newSlot = slotPool.Get();
                    newSlot.gameObject.SetActive(true);
                    SlotContainer.Add(newSlot);
                }
                SlotContainer[i].ShowItem(inventory.Items[i]);
                SlotContainer[i].inventory = inventory;
            }
        }

        public void OnInventoryContentChanged(InventoryItem item, int index, int quantity)
        {
            if (SlotContainer.Count <= index)
            {
                for (int i = SlotContainer.Count; i <= index; i++)
                {
                    var newSlot = slotPool.Get();
                    newSlot.gameObject.SetActive(true);
                    SlotContainer.Add(newSlot);
                }
            }
            SlotContainer[index].UpdateItem(inventory.Items[index], quantity);
        }
    }
}