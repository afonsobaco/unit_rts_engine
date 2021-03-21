using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UIPortraitStatusBar : UIContentRootComponent
    {
        [SerializeField] private Image _statusBar;
        [SerializeField] private Text _statusText;

        public Image StatusBar { get => _statusBar; set => _statusBar = value; }
        public Text StatusText { get => _statusText; set => _statusText = value; }
    }
}
