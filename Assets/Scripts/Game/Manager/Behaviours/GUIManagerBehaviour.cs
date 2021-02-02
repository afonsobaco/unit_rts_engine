using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUIManagerBehaviour : MonoBehaviour
    {
        private IGUIManager manager;

        [Inject]
        public void Construct(IGUIManager manager)
        {
            this.manager = manager;
        }
    }
}