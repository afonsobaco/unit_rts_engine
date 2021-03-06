using UnityEngine;

namespace RTSEngine.RTSSelection
{
    public interface IViewportHelper
    {
        Vector2 InitialViewportPoint { get; set; }
        Vector2 FinalViewportPoint { get; set; }
    }
}