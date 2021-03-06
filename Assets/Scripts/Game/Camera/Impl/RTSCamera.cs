using UnityEngine;

namespace RTSEngine.RTSCamera
{
    public class RTSCamera
    {
        public float MoveSpeed { get; set; }
        public float ZoomSpeed { get; set; }
        public float PanSpeed { get; set; }

        public const float MAGIC_NUMBER = 7.08f; //MagicNumber!

        public Vector3 GetCameraMovement(float horizontalAxis, float verticalAxis, float cameraHeight, float deltaTime)
        {
            return new Vector3(horizontalAxis, 0f, verticalAxis) * GetCameraMoveSpeedByHeight(cameraHeight) * deltaTime;
        }

        public virtual float GetCameraMoveSpeedByHeight(float height)
        {
            return (height * MoveSpeed) + MAGIC_NUMBER;
        }

        public Vector3 GetCameraZoom(float zoom, Vector3 forward, float deltaTime)
        {
            return zoom * forward * ZoomSpeed * deltaTime;
        }

        public Vector3 GetCameraPan(Vector2 mouseAxis, float height, float deltaTime)
        {
            var aux = mouseAxis * height * PanSpeed * deltaTime;
            return new Vector3(-aux.x, 0, -aux.y);
        }

    }
}
