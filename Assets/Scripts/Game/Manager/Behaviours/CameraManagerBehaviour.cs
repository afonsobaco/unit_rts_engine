using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{

    public class CameraManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private CameraSettingsSO settings;
        private ICameraManager manager;


        [Inject]
        public void Construct(ICameraManager manager)
        {
            this.manager = manager;
            this.manager.CameraSettings = settings;
        }
    }
}