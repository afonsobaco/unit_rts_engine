using UnityEngine;
namespace RTSEngine.Manager
{
    public interface ICameraSettings
    {
        float AxisPressure { get; set; }
        float BoundriesOffset { get; set; }
        float CameraSpeed { get; set; }
        float MaxZoom { get; set; }
        float MinZoom { get; set; }
        float PanSpeed { get; set; }
        float SizeFromMidPoint { get; set; }
        float ZoomScale { get; set; }
        Vector2 MinViewportPoint { get; set; }
        Vector2 MaxViewportPoint { get; set; }
    }
}