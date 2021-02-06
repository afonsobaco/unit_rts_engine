using UnityEngine;

namespace RTSEngine.Manager
{
    public abstract class AbstractCameraSettings : ScriptableObject, ICameraSettings
    {
        [Space]
        [Header("Camera movement")]
        [SerializeField] private float cameraSpeed = 1.5f;
        [SerializeField] private float panSpeed = 5;
        [SerializeField] private float boundriesOffset = 0.03f;

        [Space]
        [Header("Zoom")]
        [SerializeField] private float zoomScale = 100;
        [SerializeField] private float minZoom = 3;
        [SerializeField] private float maxZoom = 20;
        [SerializeField] private float axisPressure = 0.1f;


        //TODO remove to scriptableObject
        [Space]
        [Header("Map Configs(REMOVE)")]

        [SerializeField] private float sizeFromMidPoint = 50;
        [SerializeField] private Vector2 minViewportPoint = Vector2.zero;
        [SerializeField] private Vector2 maxViewportPoint = new Vector2(1f, 1f);


        public float CameraSpeed { get => cameraSpeed; set => cameraSpeed = value; }
        public float PanSpeed { get => panSpeed; set => panSpeed = value; }
        public float BoundriesOffset { get => boundriesOffset; set => boundriesOffset = value; }
        public float ZoomScale { get => zoomScale; set => zoomScale = value; }
        public float MinZoom { get => minZoom; set => minZoom = value; }
        public float MaxZoom { get => maxZoom; set => maxZoom = value; }
        public float AxisPressure { get => axisPressure; set => axisPressure = value; }
        public float SizeFromMidPoint { get => sizeFromMidPoint; set => sizeFromMidPoint = value; }
        public Vector2 MinViewportPoint { get => minViewportPoint; set => minViewportPoint = value; }
        public Vector2 MaxViewportPoint { get => maxViewportPoint; set => maxViewportPoint = value; }
    }

}