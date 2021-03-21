using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UIMiniatureStatusBar : UIContentRootComponent
    {
        [SerializeField] private Image _statusBar;

        public Image StatusBar { get => _statusBar; set => _statusBar = value; }
    }
}
