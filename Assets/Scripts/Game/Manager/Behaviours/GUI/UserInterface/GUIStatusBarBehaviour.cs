using UnityEngine;
using UnityEngine.UI;
namespace RTSEngine.Manager
{
    public class GUIStatusBarBehaviour : MonoBehaviour
    {
        [SerializeField] private Image statusBar;
        [SerializeField] private Text statusText;

        public Image StatusBar { get => statusBar; set => statusBar = value; }
        public Text StatusText { get => statusText; set => statusText = value; }
    }
}