using System.Collections;
using UnityEngine;
using RTSEngine.RTSUserInterface.Utils;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneInfoButton : DefaultInfoButton
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private bool _enableToolTip;
        [SerializeField] private float _toolTipTime;

        public int offset;
        private float _lastTimeEnter;

        public void Update()
        {
            if (IsMouseHover && Time.time - this._lastTimeEnter > this._toolTipTime)
            {
                ToolTip.gameObject.SetActive(true);
                ToolTip.transform.position = GetToolTipPosition();
            }
            else
            {
                ToolTip.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                offset++;
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                offset--;
            }

            if (Input.GetKey(KeyCode.KeypadMultiply))
            {
                offset++;
            }
            if (Input.GetKey(KeyCode.KeypadDivide))
            {
                offset--;
            }
        }

        private Vector3 GetToolTipPosition()
        {
            var rectTransform = (ToolTip.transform as RectTransform);
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

        public override void DoMouseEnter()
        {
            if (highlight)
            {
                highlight.SetActive(true);
                this._lastTimeEnter = Time.time;
            }
        }

        public override void DoMouseExit()
        {
            if (highlight)
            {
                highlight.SetActive(false);
            }
        }

        public override IEnumerator DoCreateAnim()
        {
            var content = this.GetComponentInChildren<UserInterfaceSceneUIBaseContent>();
            if (content)
            {
                yield return StartCoroutine(SlideIn(content.transform as RectTransform));
            }
            yield return null;
        }

        public override IEnumerator DoDestroyAnim()
        {
            var content = this.GetComponentInChildren<UserInterfaceSceneUIBaseContent>();
            if (content)
            {
                yield return StartCoroutine(SlideOut(content.transform as RectTransform));
            }
            yield return null;
        }

        private IEnumerator SlideIn(RectTransform rectTransform)
        {
            var startingPos = new Vector2(rectTransform.anchoredPosition.x + rectTransform.rect.width + 50, rectTransform.anchoredPosition.y);
            var finalPos = rectTransform.anchoredPosition;
            yield return StartCoroutine(Slide(rectTransform, startingPos, finalPos));
        }

        private IEnumerator SlideOut(RectTransform rectTransform)
        {
            var startingPos = rectTransform.anchoredPosition;
            var finalPos = new Vector2(rectTransform.anchoredPosition.x + rectTransform.rect.width + 50, rectTransform.anchoredPosition.y);
            yield return StartCoroutine(Slide(rectTransform, startingPos, finalPos));
        }

        private IEnumerator Slide(RectTransform rectTransform, Vector2 startingPos, Vector2 finalPos)
        {
            float elapsedTime = 0;
            while (elapsedTime < this.CreationAnimTime && rectTransform)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / this.CreationAnimTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            rectTransform.anchoredPosition = finalPos;
        }

    }
}