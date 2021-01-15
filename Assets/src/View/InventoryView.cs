using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Prevail.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [Inject]
        readonly InventoryController _inventoryController;
        [Inject]
        readonly CraftingController _craftingController;

        [SerializeField]
        private int containerId;

        [SerializeField]
        private GameObject slotPrefab;

        [SerializeField]
        private ItemStack[] initialContainer;

        private void Start()
        {
            _inventoryController.InitializeContainer(containerId, initialContainer);
            DisplayContainer();
        }

        private void DisplayContainer()
        {
            //clear display
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            //populate display
            foreach(var itemStack in _inventoryController.GetItems(containerId))
            {
                Instantiate(slotPrefab, transform).GetComponent<Text>().text = $"{itemStack.Item?.name} \n {itemStack.ItemCount}";
            }
        }

        public void OnInteract(InventoryItem item)
        {
            _craftingController.Craft(containerId, item);
            DisplayContainer();
        }
    }
}
