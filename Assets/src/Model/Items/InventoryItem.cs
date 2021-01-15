using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Prevail.Inventory
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class InventoryItem : ScriptableObject
    { 
        [SerializeField]
        private string _displayName;
        public string DisplayName => string.IsNullOrWhiteSpace(_displayName) ? name : _displayName;

        [SerializeField]
        private string _discription;
        public string Description => _discription;

        [SerializeField]
        private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField]
        private int _stackSize = 1;
        public int StackSize => _stackSize;

        [SerializeField]
        private GameObject _Prefab;
        public GameObject Prefab => _Prefab;
    }
}
