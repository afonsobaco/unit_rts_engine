using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class ContainerPlaceholder : MonoBehaviour
    {
        [SerializeField] private string lookupId;
        public string LookupId { get => lookupId; set => lookupId = value; }
    }
}