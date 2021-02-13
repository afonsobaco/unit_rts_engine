using Zenject;
using UnityEngine;
using RTSEngine.Manager;
using System;

namespace RTSEngine.Installers
{
    public class SelectionManagerInstaller : MonoInstaller
    {
        [SerializeField] private SelectableObjectRuntimeSetSO _selectableList;
        [SerializeField] private SelectionSettingsSO _settings;
        [SerializeField] private RectTransform _selectionBox;
        private ISelectionManager _selectionManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SelectionManager>().AsSingle().OnInstantiated<SelectionManager>(Init);
            Container.Bind<SelectionInputManagerBehaviour>().FromNewComponentOnNewGameObject().WithGameObjectName("Input Manager").AsSingle().NonLazy();

            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();
            Container.DeclareSignal<GUIClickedSignal>();
            Container.DeclareSignal<SelectionChangeSignal>();
            Container.DeclareSignal<CanMoveSignal>();
            Container.DeclareSignal<PrimaryObjectSelectedSignal>();

            Container.DeclareSignal<SelectedPortraitClickSignal>();
            Container.DeclareSignal<SelectedMiniatureClickSignal>();
            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionManager>(x => x.AddSelectableObject).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionManager>(x => x.RemoveSelectableObject).FromResolve();
            Container.BindSignal<SelectedMiniatureClickSignal>().ToMethod<SelectionManager>(x => x.DoSelectedMiniatureClick).FromResolve();
            Container.BindSignal<GUIClickedSignal>().ToMethod<SelectionManager>(x => x.SetCanSelect).FromResolve();
        }

        private void Init(InjectContext arg1, SelectionManager selectionManager)
        {
            _selectionManager = selectionManager;
            InitializeVariables();
        }

        private void OnValidate()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            if (this._selectionManager != null)
            {
                this._selectionManager.SetMainList(this._selectableList.GetAllItems());
                this._selectionManager.SetSettings(this._settings);
            }
        }

        private void Update()
        {
            ActivateSelectionBox();
        }

        private void ActivateSelectionBox()
        {
            if (!this._selectionBox)
            {
                return;
            }
            if (this._selectionManager.IsSelecting())
            {
                if (!this._selectionBox.gameObject.activeInHierarchy)
                {
                    this._selectionBox.gameObject.SetActive(true);
                }
                DrawSelectionBox();
            }
            else
            {
                if (this._selectionBox.gameObject.activeInHierarchy)
                {
                    this._selectionBox.gameObject.SetActive(false);
                }
            }

        }

        private void DrawSelectionBox()
        {
            if (this._selectionBox.gameObject.activeInHierarchy)
            {
                this._selectionBox.position = SelectionUtil.GetAreaCenter(this._selectionManager.GetInitialScreenPosition(), this._selectionManager.GetFinalScreenPosition());
                this._selectionBox.sizeDelta = SelectionUtil.GetAreaSize(this._selectionManager.GetInitialScreenPosition(), this._selectionManager.GetFinalScreenPosition());
            }
        }
    }
}