using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using RTSEngine.Commons;
using RTSEngine.RTSUserInterface.Utils;

namespace RTSEngine.RTSUserInterface
{
    public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private float _tooltipTime;
        private Coroutine coroutine;
        private void Update()
        {
            if(_tooltip.activeInHierarchy){
                _tooltip.transform.position = GetToolTipPosition();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            coroutine = StartCoroutine(DoShowTooltip());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltip.SetActive(false);
            StopCoroutine(coroutine);
        }

        public IEnumerator DoShowTooltip()
        {
            yield return new WaitForSeconds(this._tooltipTime);
            _tooltip.SetActive(true);
        }

        private Vector3 GetToolTipPosition()
        {
            var rectTransform = (_tooltip.transform as RectTransform);
            var rect = UserInterfaceUtils.GetRectTransformSize(rectTransform);
            float xAux = rect.width / 2;
            float yAux = -rect.height / 2;
            if (Input.mousePosition.x + (xAux * 2) > Camera.main.pixelWidth)
            {
                xAux *= -1;
            }
            if (Input.mousePosition.y + (yAux * 2) < 0)
            {
                yAux *= -1;
            }
            return new Vector3(Input.mousePosition.x + xAux, Input.mousePosition.y + yAux, 0);
        }
    }
}

