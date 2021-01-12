using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Selection;

namespace RTSEngine.Manager
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {

        private SelectionManagerXP manager;
        public static SelectionManagerBehaviour Instance { get; private set; }
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



    }

    public class SelectionManagerXP
    {

        public SelectionTypeEnum GetSelectionType()
        {
            return SelectionTypeEnum.CLICK;
        }
    }
}
