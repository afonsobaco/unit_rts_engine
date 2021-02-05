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
        [SerializeField] private SelectionGridBehaviour selectionGrid;
        [SerializeField] private ProfileInfoBehaviour profileInfo;

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
            this.manager.SetSelectionGrid(selectionGrid.transform);
            this.manager.SetProfileInfo(profileInfo.transform);
        }
    }


}