using System;
using Systems.Persistence;
using UnityEngine;

namespace Systems.Inventory {
    [Serializable]
    public class InventoryData : ISaveable {
        [field: SerializeField] public SerializableGuid Id { get; set; }
        public Item[] Items;
        public int Capacity;
        public int Coins;
    }
}