
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Utils;
using System;

namespace RTSEngine.Refactoring
{
    public class PolyAreaSelectionType : IAreaSelectionType
    {
        public ISelectable[] GetAllInsideScreenArea(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in mainList)
            {
                if (IsInsideScreenPoints(startPoint, endPoint, item))
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }

        public ISelectable[] GetAllInsideViewportArea(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in mainList)
            {
                if (IsInsideViewportPoints(startPoint, endPoint, item))
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }

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
