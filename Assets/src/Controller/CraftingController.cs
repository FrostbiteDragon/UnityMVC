using Prevail.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CraftingController
{
    InventoryController _InventoryController;

    public CraftingController(InventoryController inventoryController)
    {
        _InventoryController = inventoryController;
    }

    public void Craft(int id, InventoryItem item)
    {
        _InventoryController.AddItem(id, item, 2);
    }
}
