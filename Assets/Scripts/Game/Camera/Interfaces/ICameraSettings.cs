using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSCamera
{
    public interface ICameraSettings
    {

        float CameraSpeed { get; set; }
        float PanSpeed { get; set; }
        float BoundriesOffset { get; set; }
        float ZoomScale { get; set; }
        float MinZoom { get; set; }
        float MaxZoom { get; set; }
        float AxisPressure { get; set; }
        float SizeFromMidPoint { get; set; }
    }
}