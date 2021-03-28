using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RTSEngine.RTSUserInterface
{
    public class UIMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool mouseOver;
        public bool MouseOver { get => mouseOver; set => mouseOver = value; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseOver = true;
            StartCoroutine(DoFocusAnimation());
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            MouseOver = false;
            StartCoroutine(DoBlurAnimation());
        }
        
        public virtual IEnumerator DoFocusAnimation()
        {
            yield return null;
        }
        public virtual IEnumerator DoBlurAnimation()
        {
            yield return null;
        }
    }
}

