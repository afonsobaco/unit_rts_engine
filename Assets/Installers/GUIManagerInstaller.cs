using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RTSEngine.Manager;
using System;

namespace RTSEngine.Installers
{
    public class GUIManagerInstaller : MonoInstaller
    {
        [SerializeField] private RectTransform selectionGridPlaceholder;
        [SerializeField] private RectTransform portraitPlaceHolder;

        [SerializeField] private GameObject selectedMiniaturePrefab;
        [SerializeField] private GameObject selectedPortraitPrefab;
        [SerializeField] private GraphicRaycaster raycaster;
        private IGUIManager _GUIManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GUIManager>().AsSingle().OnInstantiated<GUIManager>(Init);
            Container.Bind<GUIInputManagerBehaviour>().FromNewComponentOnNewGameObject().WithGameObjectName("Input Manager").AsSingle().NonLazy();
            DeclareSignals();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<SelectionChangeSignal>();
            Container.DeclareSignal<PrimaryObjectSelectedSignal>();
            Container.DeclareSignal<GUIClickedSignal>();
            Container.DeclareSignal<SelectedPortraitClickSignal>();
            Container.DeclareSignal<SelectedMiniatureClickSignal>();
            Container.BindSignal<SelectionChangeSignal>().ToMethod<GUIManager>(x => x.OnSelectionChange).FromResolve();
        }

        private void Init(InjectContext arg1, GUIManager gUIManager)
        {
            this._GUIManager = gUIManager;
            InitializeVariables();
        }

        private void OnValidate()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            if (this._GUIManager != null)
            {
                this._GUIManager.SetRaycaster(raycaster);
                this._GUIManager.SetSelectionGridPlaceholder(selectionGridPlaceholder.transform);
                this._GUIManager.SetPortraitPlaceholder(portraitPlaceHolder.transform);
                this._GUIManager.SetSelectedMiniaturePrefab(selectedMiniaturePrefab);
                this._GUIManager.SetSelectedPortraitPrefab(selectedPortraitPrefab);
            }
        }
    }
}