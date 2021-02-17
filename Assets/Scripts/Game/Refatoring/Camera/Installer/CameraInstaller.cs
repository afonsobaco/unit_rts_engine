using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using System;

namespace RTSEngine.Refactoring
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField]
        private RTSCameraClamperComponent cameraClamper;

        [Space]
        [Header("Camera attributes")]
        public float _moveSpeed;
        public float _panSpeed;
        public float _zoomSpeed;

        private RTSCamera _camera;

        public override void InstallBindings()
        {
            Container.Bind<RTSCameraSignalManager>().AsSingle();
            Container.Bind<RTSCameraManager>().AsSingle();
            Container.Bind<RTSCamera>().AsSingle().OnInstantiated<RTSCamera>(StartCamera);
            Container.Bind<ICameraClamper>().FromMethod(GetCameraClamper);

            Container.DeclareSignal<CameraMoveSignal>();
            Container.DeclareSignal<CameraPanSignal>();
            Container.DeclareSignal<CameraZoomSignal>();
            Container.BindSignal<CameraMoveSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraMoveSignal).FromResolve();
            Container.BindSignal<CameraPanSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraPanSignal).FromResolve();
            Container.BindSignal<CameraZoomSignal>().ToMethod<RTSCameraSignalManager>(x => x.OnCameraZoomSignal).FromResolve();
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

        private ICameraClamper GetCameraClamper()
        {
            return cameraClamper.GetComponent<ICameraClamper>();
        }

    }
}