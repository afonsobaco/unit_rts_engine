using System.Net.Mime;
using RTSEngine.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.Manager
{
    public abstract class SelectedInfoBehaviour : MonoBehaviour, ISelectedInfo
    {
        [SerializeField] private Image selectionBorder;
        [SerializeField] private Image picture;
        [SerializeField] private StatusBarBehaviour lifeBar;
        [SerializeField] private StatusBarBehaviour manaBar;
        private ISelectableObjectBehaviour selected;
        public ISelectableObjectBehaviour Selected { get => selected; set => selected = value; }
        public Image SelectionBorder { get => selectionBorder; set => selectionBorder = value; }
        public Image Picture { get => picture; set => picture = value; }
        public StatusBarBehaviour LifeBar { get => lifeBar; set => lifeBar = value; }
        public StatusBarBehaviour ManaBar { get => manaBar; set => manaBar = value; }
    }
}