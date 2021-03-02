using UnityEngine;

namespace RTSEngine.Refactoring
{
    public interface IViewportHelper
    {
        Vector2 InitialViewportPoint { get; set; }
        Vector2 FinalViewportPoint { get; set; }
    }
}