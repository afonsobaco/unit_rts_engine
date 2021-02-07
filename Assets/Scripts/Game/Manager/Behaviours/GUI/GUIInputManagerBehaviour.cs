using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUIInputManagerBehaviour : MonoBehaviour
    {

        private GUIManager _manager;
        private bool _performingGUIAction;

        [Inject]
        public void Constructor(GUIManager manager)
        {
            this._manager = manager;
        }

        private void Update()
        {
            this.SetMouseClick();
        }

        public void SetMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_manager.ClickedOnGUI(Input.mousePosition))
                {
                    _performingGUIAction = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_performingGUIAction)
                {
                    _performingGUIAction = false;
                }
            }
        }
    }
}