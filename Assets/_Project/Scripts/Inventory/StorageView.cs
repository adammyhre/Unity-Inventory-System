using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory {
    public abstract class StorageView : MonoBehaviour {
        public Slot[] Slots;

        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;

        protected static VisualElement ghostIcon;

        static bool isDragging;
        static Slot originalSlot;
        
        protected VisualElement root;
        protected VisualElement container;
        
        public event Action<Slot, Slot> OnDrop;
        
        void Start() {
            ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            foreach (var slot in Slots) {
                slot.OnStartDrag += OnPointerDown;
            }
        }

        public abstract IEnumerator InitializeView(ViewModel viewModel);

        static void OnPointerDown(Vector2 position, Slot slot) {
            isDragging = true;
            originalSlot = slot;
        
            SetGhostIconPosition(position);
            
            ghostIcon.style.backgroundImage = originalSlot.BaseSprite.texture;
            originalSlot.Icon.image = null;
            originalSlot.StackLabel.visible = false;
            
            ghostIcon.style.visibility = Visibility.Visible;
            // TODO show stack size on ghost icon
        }
        
        void OnPointerMove(PointerMoveEvent evt) {
            if (!isDragging) return;
            
            SetGhostIconPosition(evt.position);
        }

        void OnPointerUp(PointerUpEvent evt) {
            if (!isDragging) return;
            Slot closestSlot = Slots
                .Where(slot => slot.worldBound.Overlaps(ghostIcon.worldBound))
                .OrderBy(slot => Vector2.Distance(slot.worldBound.position, ghostIcon.worldBound.position))
                .FirstOrDefault();
        
            if (closestSlot != null) {
                OnDrop?.Invoke(originalSlot, closestSlot);
            } else {
                originalSlot.Icon.image = originalSlot.BaseSprite.texture; 
            }
            
            isDragging = false;
            originalSlot = null;
            ghostIcon.style.visibility = Visibility.Hidden;
        }
        
        static void SetGhostIconPosition(Vector2 position) {
            ghostIcon.style.top = position.y - ghostIcon.layout.height / 2;
            ghostIcon.style.left = position.x - ghostIcon.layout.width / 2;
        }
        
        void OnDestroy() {
            foreach (var slot in Slots) {
                slot.OnStartDrag -= OnPointerDown;
            }
        }
    }
}