using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using RTSEngine.Commons;
using RTSEngine.RTSUserInterface.Utils;

namespace RTSEngine.RTSUserInterface
{
    public class UIHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _highlight;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _highlight.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _highlight.SetActive(false);
        }

    }
}

