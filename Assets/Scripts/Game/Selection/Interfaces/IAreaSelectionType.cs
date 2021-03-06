
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.RTSSelection
{
    public interface IAreaSelectionType
    {
        bool IsInsideScreenPoints(Vector2 startPoint, Vector2 endPoint, ISelectable selectable);
        bool IsInsideViewportPoints(Vector2 startPoint, Vector2 endPoint, ISelectable selectable);
        ISelectable[] GetAllInsideViewportArea(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint);
        ISelectable[] GetAllInsideScreenArea(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint);

    }
}
