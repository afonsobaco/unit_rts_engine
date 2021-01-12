using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    public class SelectableObjectMainList : MonoBehaviour
    {
        private List<SelectableObject> mainList = new List<SelectableObject>();
        public static SelectableObjectMainList Instance { get; private set; }
        public List<SelectableObject> MainList { get => mainList;}

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RemoveFromMainList(SelectableObject selectableObject)
        {
            this.MainList.Remove(selectableObject);
        }

        public void AddToMainList(SelectableObject selectableObject)
        {
            this.MainList.Add(selectableObject);
        }
    }
}