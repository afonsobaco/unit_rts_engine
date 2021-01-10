using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Selection
{
    public class SelectionOutline : MonoBehaviour
    {
        private Outline outline;

        void Awake()
        {
            outline = this.GetComponent<Outline>();
            outline.enabled = false;
        }

        void OnMouseEnter()
        {
            outline.enabled = true;
        }

        void OnMouseExit()
        {
            outline.enabled = false;
        }
    }
}
