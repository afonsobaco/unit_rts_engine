using UnityEngine;

namespace RTSEngine.RTSUserInterface.Utils
{
    public static class UserInterfaceUtils
    {
        public static Rect GetRectTransformSize(RectTransform rectTransform)
        {
            var canvas = rectTransform.GetComponentInParent<Canvas>();
            float width = rectTransform.sizeDelta.x * canvas.scaleFactor;
            float height = rectTransform.sizeDelta.y * canvas.scaleFactor;
            return new Rect(rectTransform.position, new Vector2(width, height));
        }

    }
}