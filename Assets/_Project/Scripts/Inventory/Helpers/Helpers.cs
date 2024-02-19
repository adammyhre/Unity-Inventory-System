using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public static class Helpers {
    public static Guid CreateGuidFromString(string input) {
        return new Guid(MD5.Create().ComputeHash(Encoding.Default.GetBytes(input)));
    }
    
    public static Vector2 ClampToScreen(VisualElement element, Vector2 targetPosition) {
        float x = Mathf.Clamp(targetPosition.x, 0, Screen.width - element.layout.width);
        float y = Mathf.Clamp(targetPosition.y, 0, Screen.height - element.layout.height);

        return new Vector2(x, y);
    }
}
