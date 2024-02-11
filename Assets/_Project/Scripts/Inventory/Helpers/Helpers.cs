using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory {
    public static class Helpers {
        public static Vector2 ClampToScreen(VisualElement element, Vector2 targetPosition) {
            float x = Mathf.Clamp(targetPosition.x, 0, Screen.width - element.layout.width);
            float y = Mathf.Clamp(targetPosition.y, 0, Screen.height - element.layout.height);

            return new Vector2(x, y);
        }
    }
}