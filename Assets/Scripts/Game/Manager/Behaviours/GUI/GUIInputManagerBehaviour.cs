using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using System;

namespace RTSEngine.Manager
{
    public class GUIInputManagerBehaviour : MonoBehaviour
    {

        private GUIManager _manager;
        private SignalBus _signalBus;
        private bool _performingGUIAction;

        [Inject]
        public void Constructor(GUIManager manager, SignalBus signalBus)
        {
            this._manager = manager;
            this._signalBus = signalBus;
        }

        private void Update()
        {
            this.SetMouseClick();
            this.SetUserInterfaceKeys();
        }

        private void SetUserInterfaceKeys()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _manager.ChangeGroup(Input.GetKey(KeyCode.LeftShift));
            }
        }

        public void SetMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var results = _manager.GetGUIElementsClicked(Input.mousePosition);
                if (results.Count > 0)
                {
                    _performingGUIAction = true;
                    _manager.DoClickOnElement(results, KeyButtonType.DOWN);
                    this._signalBus.Fire(new GUIClickedSignal() { Value = true });
                }
            }
            if (Input.GetMouseButton(0))
            {
                var results = _manager.GetGUIElementsClicked(Input.mousePosition);
                if (results.Count > 0 && _performingGUIAction)
                {
                    _manager.DoClickOnElement(results, KeyButtonType.PRESSED);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_performingGUIAction)
                {
                    var results = _manager.GetGUIElementsClicked(Input.mousePosition);
                    _manager.DoClickOnElement(results, KeyButtonType.UP);
                    this._signalBus.Fire(new GUIClickedSignal() { Value = false });
                    _performingGUIAction = false;
                }
            }
        }
    }
}