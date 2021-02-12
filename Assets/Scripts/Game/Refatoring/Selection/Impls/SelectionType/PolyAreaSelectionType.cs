
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;

namespace RTSEngine.Refactoring
{
    public class PolyAreaSelectionType : IAreaSelectionType
    {
        public bool IsInsideSelectionArea(Vector2 startPoint, Vector2 endPoint, ISelectable selectable)
        {
            Vector2[] area = Poly.GetDefaultQuadrilateral(startPoint, endPoint);
            Vector2 position = Camera.main.WorldToScreenPoint(selectable.Position);
            return Poly.ContainsPoint(area, position);

        }

    }
}
