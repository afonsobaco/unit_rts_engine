using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSSelection
{
    public interface IAreaSelection
    {
        ISelectable[] GetSelection(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint);
    }
}