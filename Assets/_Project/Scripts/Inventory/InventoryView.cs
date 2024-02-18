using System.Collections;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory {
    public class InventoryView : StorageView {
        [SerializeField] string panelName = "Inventory";

        public override IEnumerator InitializeView(ViewModel viewModel) {
            Slots = new Slot[viewModel.Capacity];
            root = document.rootVisualElement;
            root.Clear(); 

            root.styleSheets.Add(styleSheet);
 
            container = root.CreateChild("container");
            
            var inventory = container.CreateChild("inventory").WithManipulator(new PanelDragManipulator());
            inventory.CreateChild("inventoryFrame");
            inventory.CreateChild("inventoryHeader").Add(new Label(panelName));

            var slotsContainer = inventory.CreateChild("slotsContainer");
            for (int i = 0; i < viewModel.Capacity; i++) {
                var slot = slotsContainer.CreateChild<Slot>("slot");
                Slots[i] = slot;
            }
            
            var coins = inventory.CreateChild("coins");
            var coinsLabel = new Label();
            coins.CreateChild("coinsIcon");
            coins.Add(coinsLabel);
            coins.dataSource = viewModel.Coins;
            
            coinsLabel.SetBinding(nameof(Label.text), new DataBinding {
                dataSourcePath = new PropertyPath(nameof(BindableProperty<string>.Value)),
                bindingMode = BindingMode.ToTarget
            });
            
            ghostIcon = container.CreateChild("ghostIcon");
            
            yield return null; 
        }
    }
}