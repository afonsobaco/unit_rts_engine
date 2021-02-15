
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;
using System;

namespace RTSEngine.Refactoring
{
    public class PolyAreaSelectionType : IAreaSelectionType
    {
        public bool IsInsideScreenPoints(Vector2 startPoint, Vector2 endPoint, ISelectable selectable)
        {
            Vector2[] area = Poly.GetDefaultQuadrilateral(startPoint, endPoint);
            Vector2 position = Camera.main.WorldToScreenPoint(selectable.Position);
            return Poly.ContainsPoint(area, position);
        }

        public bool IsInsideViewportPoints(Vector2 startPoint, Vector2 endPoint, ISelectable selectable)
        {
            Vector2[] area = Poly.GetDefaultQuadrilateral(startPoint, endPoint);
            Vector2 position = Camera.main.WorldToViewportPoint(selectable.Position);
            return Poly.ContainsPoint(area, position);
        }

    }
}
