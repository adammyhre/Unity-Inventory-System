using System;
using System.Collections.Generic;

namespace Systems.Inventory {
    public class InventoryModel {
        public ObservableArray<Item> Items { get; set; }

        public event Action<Item[]> OnModelChanged {
            add => Items.AnyValueChanged += value;
            remove => Items.AnyValueChanged -= value;
        }
        
        public InventoryModel(IEnumerable<ItemDetails> itemDetails, int capacity) {
            Items = new ObservableArray<Item>(capacity);
            foreach (var itemDetail in itemDetails) {
                Items.TryAdd(itemDetail.Create(1));
            }
        }
        
        public Item Get(int index) => Items[index];
        public void Clear() => Items.Clear();
        public bool Add(Item item) => Items.TryAdd(item);
        public bool Remove(Item item) => Items.TryRemove(item);
        
        public void Swap(int source, int target) => Items.Swap(source, target);
        
        public int Combine(int source, int target) {
            var total = Items[source].quantity + Items[target].quantity;
            Items[target].quantity = total;
            Remove(Items[source]);
            return total;
        }
    }
}