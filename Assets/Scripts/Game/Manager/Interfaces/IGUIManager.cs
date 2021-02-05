using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Manager
{
    public interface IGUIManager
    {
        bool ClickedOnGUI(Vector3 mousePosition);
        void OnSelectionChange();
    }
}