using System;
using UnityEngine;

namespace Systems.Inventory {
    [Serializable]
    public class Item {
        [field: SerializeField] public SerializableGuid Id;
        [field: SerializeField] public SerializableGuid detailsId; 
        public ItemDetails details;
        public int quantity;
        
        public Item(ItemDetails details, int quantity = 1) {
            Id = SerializableGuid.NewGuid();
            this.detailsId = details.Id;
            this.details = details;
            this.quantity = quantity;
        }
    }
}