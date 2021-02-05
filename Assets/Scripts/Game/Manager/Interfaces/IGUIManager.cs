using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.Manager
{
    public interface IGUIManager
    {
        bool ClickedOnGUI(Vector3 mousePosition);
        void OnSelectionChange();
        void SetSelectionGrid(Transform selectionGridTransform);
        void SetProfileInfo(Transform profileInfoTransform);
        void SetRaycaster(GraphicRaycaster graphicRaycaster);
    }
}