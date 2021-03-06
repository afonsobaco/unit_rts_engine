using UnityEngine;
using RTSEngine.RTSCamera;
using GD.MinMaxSlider;

public class IntegrationSceneCameraClamper : RTSCameraClamperComponent
{

    [SerializeField] [Range(1, 200)] private int mapWidth = 10;
    [SerializeField] [Range(1, 200)] private int mapHeight = 10;
    [SerializeField] [MinMaxSlider(1, 50)] private Vector2 zoom = new Vector2(2, 15);

    public override Vector3 ClampCameraPos(Transform cameraTransform)
    {
        Vector3 newPosition = cameraTransform.position;
        float xRotation = cameraTransform.rotation.eulerAngles.x;
        newPosition = AdjustZoom(newPosition, cameraTransform.forward);
        newPosition = AdjustMovement(newPosition, GetCameraZDistance(xRotation, newPosition.y));
        return newPosition;
    }

    private Vector3 AdjustZoom(Vector3 position, Vector3 forward)
    {
        Vector3 newPosition = position;
        if (newPosition.y < zoom.x)
        {
            newPosition += clampZoomOnY(forward, newPosition.y, zoom.x);
        }
        else if (newPosition.y > zoom.y)
        {
            newPosition += clampZoomOnY(forward, newPosition.y, zoom.y);
        }
        return newPosition;
    }

    private Vector3 AdjustMovement(Vector3 position, float zDistance)
    {
        float clampedX = Mathf.Clamp(position.x, -mapWidth, mapWidth);
        float clampedZ = Mathf.Clamp(position.z, -mapHeight - (zDistance), mapHeight - zDistance);
        return new Vector3(clampedX, position.y, clampedZ);
    }

    private float GetCameraZDistance(float xRotation, float cameraHeight)
    {
        float angle = 90 - xRotation;
        angle = Mathf.Clamp(angle, 0, 80); // prevent weird angles
        float zDistance = cameraHeight * Mathf.Tan(angle * Mathf.Deg2Rad);
        return zDistance;
    }

    private Vector3 clampZoomOnY(Vector3 forward, float actualPosition, float desiredPosition)
    {
        var k = (desiredPosition - actualPosition) / forward.y;
        return k * forward;
    }
}
