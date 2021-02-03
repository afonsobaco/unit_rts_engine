using System.Net.Mime;
using RTSEngine.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.Manager
{
    public class SelectedInfoBehaviour : MonoBehaviour, ISelectedInfo
    {
        [SerializeField] private Image SelectionBorder;
        [SerializeField] private Image Picture;
        [SerializeField] private StatusBarBehaviour LifeBar;
        [SerializeField] private StatusBarBehaviour ManaBar;
        private ISelectableObjectBehaviour selected;

        public ISelectableObjectBehaviour Selected { get => selected; set => selected = value; }
    }
}