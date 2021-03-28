using UnityEngine;
using UnityEngine.UI;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIMiniatureStatusBar : UIContentRootComponent
    {
        [SerializeField] private Image _statusBar;

        public Image StatusBar { get => _statusBar; set => _statusBar = value; }
    }
}
