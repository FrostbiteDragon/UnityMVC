using Prevail.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Prevail.Data
{
    public class InventoryData
    {
        [Inject]
        readonly ContainerCache _containerCache;

        public void Save(int containerId, ItemStack[] items)
        {
            //save localy
            _containerCache[containerId] = new Container() {Id = containerId, Items = items };
            //post to db
        }

        public Container GetContainer(int containerId)
        {
            return _containerCache[containerId];
        }
    }
}
