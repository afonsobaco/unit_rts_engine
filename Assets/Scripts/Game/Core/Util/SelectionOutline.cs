using UnityEngine;

namespace RTSEngine.Core
{
    public class SelectionOutline : MonoBehaviour
    {
        private Outline outline;

        void Awake()
        {
            outline = this.GetComponent<Outline>();
            if(!outline){
                this.enabled = false;
                return;
            }
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
