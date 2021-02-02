using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Manager
{
    public class GUIManager : IGUIManager
    {
        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> manager;

        public GUIManager(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> manager)
        {
            this.manager = manager;
        }

        public void OnSelectionChange()
        {
            Debug.Log("Updated GUI");
            manager.GetCurrentSelection();
        }
    }

}