
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface IAreaSelectionType
    {
        bool IsInsideScreenPoints(Vector2 startPoint, Vector2 endPoint, ISelectable selectable);
        bool IsInsideViewportPoints(Vector2 startPoint, Vector2 endPoint, ISelectable selectable);

    }
}
