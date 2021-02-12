
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface IAreaSelectionType
    {
        bool IsInsideSelectionArea(Vector2 startPoint, Vector2 endPoint, ISelectable selectable);
    }
}
