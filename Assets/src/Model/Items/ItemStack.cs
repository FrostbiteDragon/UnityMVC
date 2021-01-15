using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Prevail.Inventory
{
    [Serializable]
    public struct ItemStack
    {
        [SerializeField]
        private InventoryItem item;
        public InventoryItem Item { get => item; set => item = value; }

        [SerializeField]
        private int itemCount;
        public int ItemCount { get => itemCount; set => itemCount = value; }

    public static ItemStack Zero { get; } = new ItemStack(null, 0);

        public ItemStack(InventoryItem item, int itemCount)
        {
            this.item = item;
            this.itemCount = itemCount;
        }

        public static bool operator ==(ItemStack lhs, ItemStack rhs) => lhs.Equals(rhs);
        public static bool operator !=(ItemStack lhs, ItemStack rhs) => !(lhs.Equals(rhs));

        public override string ToString()
        {
            return $"{{{Item}, {ItemCount}}}";
        }
    }
}