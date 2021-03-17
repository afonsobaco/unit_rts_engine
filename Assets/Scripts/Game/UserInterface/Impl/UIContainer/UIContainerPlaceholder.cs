using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerPlaceholder : MonoBehaviour
    {
        [SerializeField] private string _containerId;
        public string ContainerId { get => _containerId; set => _containerId = value; }
    }
}