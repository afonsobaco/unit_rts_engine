using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.Core
{
    public class SelectableObjectMainList : MainList<SelectableObject>
    {
        public static SelectableObjectMainList Instance { get; private set; }

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
}
