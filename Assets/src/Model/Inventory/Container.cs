using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Prevail.Inventory
{
    [Serializable]
    public class Container
    {
        public int Id { get; set; }
        public ItemStack[] Items { get; set; }
    }
}
