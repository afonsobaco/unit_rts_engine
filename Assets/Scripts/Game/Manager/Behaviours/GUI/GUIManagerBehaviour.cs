using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUIManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private GUISelectionGridBehaviour selectionGrid;
        [SerializeField] private GUISelectedPortraitBehaviour profileInfo;

        private IGUIManager manager;


        [Inject]
        public void Construct(IGUIManager manager)
        {
            this.manager = manager;
            this.UpdateVariables();
        }

        private void OnValidate()
        {
            if (this.manager != null)
            {
                UpdateVariables();
            }

        }

        private void UpdateVariables()
        {
            this.manager.SetRaycaster(GetComponent<GraphicRaycaster>());
            this.manager.SetSelectionGridPlaceholder(selectionGrid.transform);
            this.manager.SetPortraitPlaceholder(profileInfo.transform);
        }
    }


}