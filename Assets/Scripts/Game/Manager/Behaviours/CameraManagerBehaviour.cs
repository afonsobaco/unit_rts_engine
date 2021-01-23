using UnityEngine;
using Zenject;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls
{

    public class CameraManagerBehaviour : MonoBehaviour
    {
        private ICameraManager manager;
        public CameraSettings cameraSettings;

        public ICameraManager Manager { get => manager; private set => manager = value; }

        [Inject]
        public void Construct(ICameraManager manager)
        {
            this.Manager = manager;
            this.Manager.CameraSettings = cameraSettings;
        }
    }
}