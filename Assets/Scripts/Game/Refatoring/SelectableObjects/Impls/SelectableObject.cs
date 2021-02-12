using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectableObject : MonoBehaviour, ISelectable
    {
        [SerializeField] private string objectName;
        [SerializeField] private LayerMask layer;
        [SerializeField] private Status[] statuses;

        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public bool IsPreSelected { get; set; }
        public Vector3 Position { get; set; }
        public string ObjectName { get => objectName; set => objectName = value; }
        public Status[] Statuses { get => statuses; set => statuses = value; }
    }
}