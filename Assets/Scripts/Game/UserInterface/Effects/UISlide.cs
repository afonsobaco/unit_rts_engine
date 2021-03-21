using System.Collections;
using UnityEngine;
using RTSEngine.Commons;

namespace RTSEngine.RTSUserInterface
{
    public class UISlide : UIEffectBase
    {
        [SerializeField] private Direction _direction;
        [SerializeField] private float _offset = 50f;

        [SerializeField] private bool _startOutside = true;
        [SerializeField] private bool _hideBeforeDestroy = true;

        private RectTransform rectTransform;
        private Vector2 realAnchoredPosition;
        private bool _hide;

        private void Awake()
        {
            rectTransform = (this.transform as RectTransform);
            realAnchoredPosition = rectTransform.anchoredPosition;
        }

        public override IEnumerator Create()
        {
            if (_startOutside)
            {
                rectTransform.anchoredPosition = GetOutsidePositionByDirection();
                _hide = true;
            }
            yield return StartCoroutine(Show());
        }

        public override IEnumerator Destroy()
        {
            if (_hideBeforeDestroy)
                yield return StartCoroutine(Hide());
            yield return StartCoroutine(base.Destroy());
        }

        public override IEnumerator Hide()
        {
            yield return StartCoroutine(SlideOut());
        }

        public override IEnumerator Show()
        {
            yield return StartCoroutine(SlideIn());
        }

        private IEnumerator SlideIn()
        {
            if (_hide)
            {
                this._hide = false;
                yield return StartCoroutine(Slide(GetOutsidePositionByDirection(), realAnchoredPosition, CreateAnimationTime));
            }
        }

        private IEnumerator SlideOut()
        {
            if (!_hide)
            {
                _hide = true;
                yield return StartCoroutine(Slide(realAnchoredPosition, GetOutsidePositionByDirection(), DestroyAnimationTime));
            }
        }

        private Vector2 GetOutsidePositionByDirection()
        {
            Vector2 result = Vector2.zero;
            switch (_direction)
            {
                case Direction.UP:
                    result = new Vector2(realAnchoredPosition.x, rectTransform.rect.height + _offset);
                    break;
                case Direction.DOWN:
                    result = new Vector2(realAnchoredPosition.x, -rectTransform.rect.height - _offset);
                    break;
                case Direction.LEFT:
                    result = new Vector2(rectTransform.rect.width + _offset, realAnchoredPosition.y);
                    break;
                case Direction.RIGHT:
                    result = new Vector2(-rectTransform.rect.width - _offset, realAnchoredPosition.y);
                    break;
                default:
                    break;
            }
            return result;
        }

        private IEnumerator Slide(Vector2 startingPos, Vector2 finalPos, float animationTime)
        {
            float elapsedTime = 0;
            while (elapsedTime < animationTime && rectTransform)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / animationTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            rectTransform.anchoredPosition = finalPos;
        }
    }
}

