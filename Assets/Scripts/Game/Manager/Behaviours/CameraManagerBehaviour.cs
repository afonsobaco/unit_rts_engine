using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{

    public class CameraManagerBehaviour : MonoBehaviour
    {
        private ICameraManager manager;
        private CameraSettings cameraSettings;

        public CameraSettings CameraSettings { get => cameraSettings; set => cameraSettings = value; }
        public ICameraManager Manager { get => manager; set => manager = value; }

        [Inject]
        public void Construct(ICameraManager manager)
        {
            this.Manager = manager;
            this.Manager.CameraSettings = CameraSettings;
        }
    }
}