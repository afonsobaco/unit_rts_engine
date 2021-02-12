using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public interface IAreaSelection
    {
        ISelectable[] GetSelection(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint);
    }
}