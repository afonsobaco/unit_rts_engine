using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTSEngine.Manager
{
    public interface IGUIManager
    {
        void SetSelectionGridPlaceholder(Transform selectionGridTransform);
        void SetPortraitPlaceholder(Transform profileInfoTransform);
        void SetRaycaster(GraphicRaycaster graphicRaycaster);
        void SetSelectedMiniaturePrefab(GameObject selectedMiniaturePrefab);
        void SetSelectedPortraitPrefab(GameObject selectedPortraitPrefab);
        void ChangeGroup(bool back);
        void DoClickOnElement(List<RaycastResult> results, KeyButtonType type);
        List<RaycastResult> GetGUIElementsClicked(Vector3 mousePosition);
    }
}