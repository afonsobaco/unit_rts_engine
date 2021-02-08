using Zenject;
using UnityEngine;
using RTSEngine.Manager;
using System;

namespace RTSEngine.Installers
{
    public class CameraManagerInstaller : MonoInstaller
    {
        [SerializeField] private CameraSettingsSO _settings;
        private ICameraManager _cameraManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle().OnInstantiated<CameraManager>(Init);
            Container.Bind<CameraInputManagerBehaviour>().FromNewComponentOnNewGameObject().WithGameObjectName("Input Manager").AsSingle().NonLazy();

            Container.DeclareSignal<CanMoveSignal>();
            Container.DeclareSignal<PrimaryObjectSelectedSignal>();

            Container.BindSignal<SelectedPortraitClickSignal>().ToMethod<CameraManager>(x => x.DoSelectedPortraitClick).FromResolve();
            Container.BindSignal<PrimaryObjectSelectedSignal>().ToMethod<CameraManager>(x => x.OnSelectionChange).FromResolve();
            Container.BindSignal<CanMoveSignal>().ToMethod<CameraManager>(x => x.CanMoveSignal).FromResolve();
        }

        private void Init(InjectContext arg1, CameraManager cameraManager)
        {
            this._cameraManager = cameraManager;
            InitializeVariables();
        }

        private void OnValidate()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            if (this._cameraManager != null)
            {
                this._cameraManager.SetCameraSettings(this._settings);
            }
        }
    }
}