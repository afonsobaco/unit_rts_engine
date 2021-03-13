using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainer : MonoBehaviour, IInitializable
    {
        [SerializeField] private string _containerId = "Default container";
        [SerializeField] private GameObject placeholder;

        public string ContainerId { get => _containerId; set => _containerId = value; }
        public GameObject Placeholder { get => placeholder; set => placeholder = value; }

        public void Initialize()
        {
            Debug.Log("Initialized Container: " + _containerId);
            Canvas canvas = FindObjectOfType<Canvas>();
            this.transform.SetParent(canvas.transform, false);
            if(!placeholder){
                placeholder = this.gameObject;
            }
        }
    }
}