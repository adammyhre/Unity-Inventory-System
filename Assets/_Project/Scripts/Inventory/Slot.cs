using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory {
    public class Slot : VisualElement {
        public Image Icon;
        public Label StackLabel;
        public int Index => parent.IndexOf(this);
        public SerializableGuid ItemId { get; private set; } = SerializableGuid.Empty;
        public Sprite BaseSprite;

        public event Action<Vector2, Slot> OnStartDrag = delegate { };

        public Slot() {
            Icon = this.CreateChild<Image>("slotIcon");
            StackLabel = this.CreateChild("slotFrame").CreateChild<Label>("stackCount");
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        void OnPointerDown(PointerDownEvent evt) {
            if (evt.button != 0 || ItemId.Equals(SerializableGuid.Empty)) return;
            
            OnStartDrag.Invoke(evt.position, this);
            evt.StopPropagation();
        }

        public void Set(SerializableGuid id, Sprite icon, int qty = 0) {
            ItemId = id;
            BaseSprite = icon;
            
            Icon.image = BaseSprite != null ? icon.texture : null;
            
            StackLabel.text = qty > 1 ? qty.ToString() : string.Empty;
            StackLabel.visible = qty > 1;
        }

        public void Remove() {
            ItemId = SerializableGuid.Empty;
            Icon.image = null;
        }
    }
}