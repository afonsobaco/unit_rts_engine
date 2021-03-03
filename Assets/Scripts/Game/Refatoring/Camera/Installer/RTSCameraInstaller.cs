using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Utils;
using System;
using RTSEngine.Signal;

namespace RTSEngine.Refactoring
{

    [CreateAssetMenu(fileName = "CameraInstaller", menuName = "Installers/CameraInstaller")]
    public class RTSCameraInstaller : ScriptableObjectInstaller<RTSCameraInstaller>
    {
        [SerializeField]
        private RTSCameraClamperComponent cameraClamper;

        [Space]
        [Header("Camera attributes")]
        public float _moveSpeed = 0.5f;
        public float _panSpeed = 7f;
        public float _zoomSpeed = 100;

        private RTSCamera _camera;

        public override void InstallBindings()
        {
            Container.Bind<RTSCameraSignalManager>().AsSingle();
            Container.Bind<RTSCameraManager>().AsSingle();
            Container.Bind<RTSCamera>().AsSingle().OnInstantiated<RTSCamera>(StartCamera);
            Container.Bind<IRTSCameraClamper>().FromMethod(GetCameraClamper);

            Container.DeclareSignal<CameraMoveSignal>();
            Container.DeclareSignal<CameraPanSignal>();
            Container.DeclareSignal<CameraZoomSignal>();
            Container.DeclareSignal<CameraGoToPositionSignal>();

            Container.BindSignal<CameraMoveSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraMoveSignal).FromResolve();
            Container.BindSignal<CameraPanSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraPanSignal).FromResolve();
            Container.BindSignal<CameraZoomSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraZoomSignal).FromResolve();
            Container.BindSignal<CameraGoToPositionSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraGoToPositionSignal).FromResolve();
        }

        private void StartCamera(InjectContext ctx, RTSCamera camera)
        {
            this._camera = camera;
            StartCameraVariables();
        }

        private void OnValidate()
        {
            if (_camera != null)
            {
                StartCameraVariables();
            }
        }

        private void StartCameraVariables()
        {
            _camera.MoveSpeed = _moveSpeed;
            _camera.PanSpeed = _panSpeed;
            _camera.ZoomSpeed = _zoomSpeed;
        }

        private IRTSCameraClamper GetCameraClamper()
        {
            return cameraClamper.GetComponent<IRTSCameraClamper>();
        }

    }
}