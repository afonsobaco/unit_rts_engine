using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RTSEngine.Manager
{
    public class SelectionUtil
    {
        public static HashSet<ISelectableObject> GetAllObjectsInsideSelectionArea(HashSet<ISelectableObject> allObjects, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            HashSet<ISelectableObject> list = new HashSet<ISelectableObject>();
            if (allObjects == null)
            {
                return list;
            }
            for (int i = 0; i < allObjects.Count; i++)
            {
                ISelectableObject obj = (ISelectableObject)allObjects.ElementAt(i);
                var screenPos = Camera.main.WorldToScreenPoint(obj.Position);
                if (IsPositionInsideArea(screenPos, initialScreenPosition, finalScreenPosition))
                {
                    list.Add(obj);
                }
            }
            return list;
        }

        public static ISelectableObject GetObjectClicked(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            var initialObject = GetObjectInScreenPoint(initialScreenPosition, Camera.main);
            var finalObject = GetObjectInScreenPoint(finalScreenPosition, Camera.main);
            if (initialObject != null && initialObject.Equals(finalObject))
            {
                return initialObject;
            }
            return null;
        }

        public static ISelectableObject GetObjectInScreenPoint(Vector2 mousePosition, Camera camera)
        {
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                return hit.transform.gameObject.GetComponent<ISelectableObject>();
            }
            return null;
        }

        private static bool IsPositionInsideArea(Vector2 screenPos, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            var min = GetMinAreaPosition(initialScreenPosition, finalScreenPosition);
            var max = GetMaxAreaPosition(initialScreenPosition, finalScreenPosition);
            return screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y;
        }

        private static Vector2 GetMaxAreaPosition(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            Vector2 size = GetAreaSize(initialScreenPosition, finalScreenPosition);
            Vector2 center = GetAreaCenter(initialScreenPosition, finalScreenPosition);
            return new Vector2(center.x + (size.x / 2), center.y + (size.y / 2));
        }

        private static Vector2 GetMinAreaPosition(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            Vector2 size = GetAreaSize(initialScreenPosition, finalScreenPosition);
            Vector2 center = GetAreaCenter(initialScreenPosition, finalScreenPosition);
            return new Vector2(center.x - (size.x / 2), center.y - (size.y / 2));
        }

        public static Vector2 GetAreaSize(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            return new Vector2(Mathf.Abs(initialScreenPosition.x - finalScreenPosition.x), Mathf.Abs(initialScreenPosition.y - finalScreenPosition.y));
        }

        public static Vector2 GetAreaCenter(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            return (initialScreenPosition + finalScreenPosition) / 2;
        }

    }
}
