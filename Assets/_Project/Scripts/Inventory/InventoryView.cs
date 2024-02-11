using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory {
    public class InventoryView : StorageView {
        [SerializeField] string panelName = "Inventory";

        public override IEnumerator InitializeView(int size = 20) {
            Slots = new Slot[size];
            root = document.rootVisualElement;
            root.Clear(); 

            root.styleSheets.Add(styleSheet);
 
            container = root.CreateChild("container");
            
            var inventory = container.CreateChild("inventory").WithManipulator(new PanelDragManipulator());
            inventory.CreateChild("inventoryFrame");
            inventory.CreateChild("inventoryHeader").Add(new Label(panelName));

            var slotsContainer = inventory.CreateChild("slotsContainer");
            for (int i = 0; i < size; i++) {
                var slot = slotsContainer.CreateChild<Slot>("slot");
                Slots[i] = slot;
            }
            
            ghostIcon = container.CreateChild("ghostIcon");
            
            yield return null; 
        }
    }
}
