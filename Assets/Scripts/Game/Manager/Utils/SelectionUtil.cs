using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RTSEngine.Manager
{
    public class SelectionUtil
    {
        public static HashSet<ISelectableObjectBehaviour> GetAllObjectsInsideSelectionArea(HashSet<ISelectableObjectBehaviour> allObjects, Vector3 initialScreenPosition, Vector3 finalScreenPosition)
        {
            HashSet<ISelectableObjectBehaviour> list = new HashSet<ISelectableObjectBehaviour>();
            if (allObjects == null)
            {
                return list;
            }
            for (int i = 0; i < allObjects.Count; i++)
            {
                ISelectableObjectBehaviour obj = (ISelectableObjectBehaviour)allObjects.ElementAt(i);
                var screenPos = Camera.main.WorldToScreenPoint(obj.Position);
                if (IsPositionInsideArea(screenPos, initialScreenPosition, finalScreenPosition))
                {
                    list.Add(obj);
                }
            }
            return list;
        }

        public static ISelectableObjectBehaviour GetObjectClicked(Vector3 initialScreenPosition, Vector3 finalScreenPosition)
        {
            var initialObject = GetObjectInScreenPoint(initialScreenPosition, Camera.main);
            var finalObject = GetObjectInScreenPoint(finalScreenPosition, Camera.main);
            if (initialObject != null && initialObject.Equals(finalObject))
            {
                return initialObject;
            }
            return null;
        }

        private static bool IsPositionInsideArea(Vector3 screenPos, Vector3 initialScreenPosition, Vector3 finalScreenPosition)
        {
            var min = GetMinAreaPosition(initialScreenPosition, finalScreenPosition);
            var max = GetMaxAreaPosition(initialScreenPosition, finalScreenPosition);
            return screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y;
        }

        private static Vector2 GetMaxAreaPosition(Vector3 initialScreenPosition, Vector3 finalScreenPosition)
        {
            Vector2 size = GetAreaSize(initialScreenPosition, finalScreenPosition);
            Vector3 center = GetAreaCenter(initialScreenPosition, finalScreenPosition);
            return new Vector2(center.x + (size.x / 2), center.y + (size.y / 2));
        }

        private static Vector2 GetMinAreaPosition(Vector3 initialScreenPosition, Vector3 finalScreenPosition)
        {
            Vector2 size = GetAreaSize(initialScreenPosition, finalScreenPosition);
            Vector3 center = GetAreaCenter(initialScreenPosition, finalScreenPosition);
            return new Vector2(center.x - (size.x / 2), center.y - (size.y / 2));
        }

        public static ISelectableObjectBehaviour GetObjectInScreenPoint(Vector3 mousePosition, Camera camera)
        {
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                return hit.transform.gameObject.GetComponent<ISelectableObjectBehaviour>();
            }
            return null;
        }

        public static Vector2 GetAreaSize(Vector2 initialScreenPosition, Vector3 finalScreenPosition)
        {
            return new Vector2(Mathf.Abs(initialScreenPosition.x - finalScreenPosition.x), Mathf.Abs(initialScreenPosition.y - finalScreenPosition.y));
        }

        public static Vector2 GetAreaCenter(Vector3 initialScreenPosition, Vector3 finalScreenPosition)
        {
            return (initialScreenPosition + finalScreenPosition) / 2;
        }

    }
}
