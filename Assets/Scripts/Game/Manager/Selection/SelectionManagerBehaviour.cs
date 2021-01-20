using System.Collections;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private IRuntimeSet<SelectableObject> selectableList;
        private SelectionManager manager = new SelectionManager();

        public SelectionManager Manager { get => manager; set => manager = value; }

        private void Awake()
        {
            Manager.SelectableList = selectableList;
        }
    }
}
