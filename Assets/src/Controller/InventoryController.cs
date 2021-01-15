using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using Prevail.Data;

namespace Prevail.Inventory
{
    /// <summary>
    /// The base class for storage containers
    /// </summary>
    public class InventoryController
    {
        [Inject]
        InventoryData _InventoryData;

        Container GetContainer(int id) => _InventoryData.GetContainer(id);

        public Action OnContainerUpdate;

        internal void InitializeContainer(int id, ItemStack[] items)
        {
            _InventoryData.Save(id, items);
        }

        /// <summary>
        /// The Items in this container. [Depricated] use Container[] instead
        /// </summary>
        public IReadOnlyList<ItemStack> GetItems(int id) => GetContainer(id).Items;

        [SerializeField]
        private int _maxSlots = 5;
        public int MaxSlots { get => _maxSlots; }

        /// <summary>
        /// Get the tottal amount of (item) in container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetTotal(int id, InventoryItem item) => GetContainer(id).Items.Where(x => x.Item == item).Sum(x => x.ItemCount);

        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to move.</param>
        /// <returns>If you can add (item) to this container.</returns>
        public virtual bool CanAddItemToSlot(int id, InventoryItem item, int slot, int amount = 1)
        {
            return 
                slot < GetContainer(id).Items.Length && GetContainer(id).Items[slot].Item == null ||
                GetContainer(id).Items[slot].Item == item && GetContainer(id).Items[slot].ItemCount + amount < item.StackSize;
        }

        public virtual bool CanAddItem(int id, InventoryItem item, int amount)
        {
             return GetContainer(id).Items
                //where item or null
                .Where(x => x.Item == item || x.Item is null)
                //addition all available space
                .Sum(x => x.Item is null ? item.StackSize : x.Item.StackSize - x.ItemCount) >= amount;
        }
        
        public void AddItem(int id, InventoryItem item, int amount = 1)
        {
            if (CanAddItem(id, item, amount))
                AddItemToItems(id, item, amount);
        }

        /// <summary>
        /// Add (item) to this container.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to move.</param>
        public void AddItemToSlot(int id, InventoryItem item, int slot, int amount = 1)
        {
            //if there is no item or the item's quantity is smaller then the stack size
            if (CanAddItemToSlot(id, item, slot, amount))
            {
                GetContainer(id).Items[slot] = new ItemStack(item, GetContainer(id).Items[slot].ItemCount + amount);
                //Debug.Log($"Container.container.Items[{slot}] = " + container.Items[slot]);
            }
            else 
                Debug.LogError($"Could not add {amount} {{{item.name}}} to container {this}");

            OnContainerUpdate?.Invoke();
        }

        /// <summary>
        /// Adds as much of the (item) into this container as possible.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to add.</param>
        /// <returns>The remainder</returns>
        public ItemStack AddMax(int id, InventoryItem item, int amount = 1)
        {
            if (CanAddItem(id, item, amount))
            {
                AddItem(id, item, amount);
                return new ItemStack(item, 0);
            }
            else
            {
                var amountLeftToAdd = AddItemToItems(id, item, amount);
                return new ItemStack(item, amountLeftToAdd);
            }
        }

        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to move.</param>
        /// <returns>If you can remove an item to this container</returns>
        public virtual bool CanRemoveItem(int id, InventoryItem item, int amount = 1)
        {
            return GetContainer(id).Items.Where(x => x.Item == item).Sum(x => x.ItemCount) >= amount;
        }
        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to move.</param>
        /// <returns>If you can remove an item to this container</returns>
        public virtual bool CanRemoveItemFromSlot(int id, int slot, int amount = 1)
        {
            return slot < GetContainer(id).Items.Length && GetContainer(id).Items[slot].ItemCount >= amount;
        }
        public virtual bool CanRemoveItemFromSlot(int id, int slot)
        {
            return slot < GetContainer(id).Items.Length && GetContainer(id).Items[slot].Item != null;
        }

        /// <summary>
        /// Remove (item) from this container
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to move.</param>
        /// <returns>If the operation was succesfull</returns>
        public ItemStack RemoveItemFromSlot(int id, int slot, int amount)
        {
            var item = GetContainer(id).Items[slot].Item;

            //if container containes the item
            if (CanRemoveItemFromSlot(slot, amount))
            {
                var slotItem = GetContainer(id).Items[slot];
                GetContainer(id).Items[slot] = new ItemStack(item, slotItem.ItemCount - amount);

                //if none of the item are left, remove the item from dictionary
                if (GetContainer(id).Items[slot].ItemCount == 0)
                    GetContainer(id).Items[slot] = ItemStack.Zero;

                OnContainerUpdate?.Invoke();
                return new ItemStack(slotItem.Item, slotItem.ItemCount);
            }
            else throw new Exception();
        }

        public ItemStack RemoveItemFromSlot(int id, int slot)
        {
            var slotItem = GetContainer(id).Items[slot];

            //if container containes the item
            if (CanRemoveItemFromSlot(id, slot))
            {
                GetContainer(id).Items[slot] = ItemStack.Zero;

                OnContainerUpdate?.Invoke();
                return new ItemStack(slotItem.Item, slotItem.ItemCount);
            }
            else throw new Exception();
        }

        /// <summary>
        /// Remove (item) from this container
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">The amount of (item) to move.</param>
        /// <returns>If the operation was succesfull</returns>
        public ItemStack RemoveItem(int id, InventoryItem item, int amount = 1)
        {
            if (CanRemoveItem(id, item, amount))
            {
                RemoveItemFromItems(id, item, amount);
                return new ItemStack(item, amount);
            }
            else throw new Exception();

        }

        public ItemStack RemoveMax(int id, InventoryItem item, int amount = 1)
        {
            if (CanRemoveItem(id, item, amount))
            {
                RemoveItemFromItems(id, item, amount);
                return new ItemStack(item, 0);
            }
            else
            {
                var amountLeftToAdd = RemoveItemFromItems(id, item, amount);
                return new ItemStack(item, amountLeftToAdd);
            }
        }

        public void Moveitem(int id, int fromSlot, int toSlot)
        {
            var slotItem = GetContainer(id).Items[fromSlot];

            if (CanAddItemToSlot(id, slotItem.Item, toSlot, slotItem.ItemCount))
            {
                AddItemToSlot(id, slotItem.Item, toSlot, slotItem.ItemCount);
                RemoveItemFromSlot(fromSlot, slotItem.ItemCount);
            }
        }

        private int AddItemToItems(int id, InventoryItem inventoryItem, int amount = 1)
        {
            var amountLeftToAdd = amount;

            var i = -1;
            foreach (var slotItem in GetContainer(id).Items)
            {
                i++;
                if (amountLeftToAdd <= 0)
                    break;

                if (slotItem.Item != null && slotItem.Item != inventoryItem)
                    continue;

                var spaceLeftInSlot = inventoryItem.StackSize - slotItem.ItemCount;

                GetContainer(id).Items[i] = new ItemStack(inventoryItem, slotItem.ItemCount + (amountLeftToAdd <= spaceLeftInSlot ? amountLeftToAdd : spaceLeftInSlot));
                amountLeftToAdd = Mathf.Max(amountLeftToAdd - spaceLeftInSlot, 0);
            };

            OnContainerUpdate?.Invoke();
            return amountLeftToAdd;
        }

        private int RemoveItemFromItems(int id, InventoryItem item, int amount = 1)
        {
            var amountLeftToRemove = amount;
            foreach(var _ in GetContainer(id).Items)
            {
                if (amountLeftToRemove < 0)
                    break;

                if (!GetContainer(id).Items.Any(x => x.Item == item))
                    break;

                var index = GetContainer(id).Items.ToList().FindLastIndex(y => y.Item == item);
                var itemsLeftInSlot = GetContainer(id).Items[index].ItemCount;

                GetContainer(id).Items[index] = new ItemStack(GetContainer(id).Items[index].Item, GetContainer(id).Items[index].ItemCount - (amountLeftToRemove <= itemsLeftInSlot ? amountLeftToRemove : itemsLeftInSlot));
                amountLeftToRemove = Mathf.Max(amountLeftToRemove - itemsLeftInSlot, 0);

                if (GetContainer(id).Items[index].ItemCount == 0)
                    GetContainer(id).Items[index] = ItemStack.Zero;
            };

            OnContainerUpdate?.Invoke();
            return amountLeftToRemove;
        }
    }
}