using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTSEngine.Manager
{
    public interface IGUIManager
    {
        bool ClickedOnGUI(Vector3 mousePosition);
        void SetSelectionGridPlaceholder(Transform selectionGridTransform);
        void SetPortraitPlaceholder(Transform profileInfoTransform);
        void SetRaycaster(GraphicRaycaster graphicRaycaster);
        void SetSelectedMiniaturePrefab(GameObject selectedMiniaturePrefab);
        void SetSelectedPortraitPrefab(GameObject selectedPortraitPrefab);
    }
}