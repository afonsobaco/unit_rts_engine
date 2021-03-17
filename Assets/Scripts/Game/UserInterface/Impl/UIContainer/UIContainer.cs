using Zenject;
using UnityEngine;
using System.Linq;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainer : MonoBehaviour, IInitializable
    {
        [SerializeField] private string _containerId = "Default container";
        [SerializeField] private GameObject _contentPlaceholder;

        public string ContainerId { get => _containerId; set => _containerId = value; }
        public GameObject ContentPlaceholder { get => _contentPlaceholder; set => _contentPlaceholder = value; }

        public void Initialize()
        {
            AddToCanvas();
            if (!_contentPlaceholder)
            {
                _contentPlaceholder = this.gameObject;
            }
        }

        private void AddToCanvas()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas)
            {
                Transform parentTransform = canvas.transform;
                UIContainerPlaceholder[] uIContainerPlaceholders = canvas.GetComponentsInChildren<UIContainerPlaceholder>();
                if (uIContainerPlaceholders.Length > 0)
                {
                    var containerPlaceholder = uIContainerPlaceholders.First(x => x.ContainerId.Equals(this.ContainerId));
                    if (containerPlaceholder)
                    {
                        parentTransform = containerPlaceholder.transform;
                    }
                }
                this.transform.SetParent(parentTransform, false);
            }
        }
    }
}