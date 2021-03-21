using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using RTSEngine.Utils;

namespace RTSEngine.RTSUserInterface
{
    public abstract class UIEffectBase : MonoBehaviour, IUIEffect
    {

        [SerializeField] private float _createAnimationTime = 1f;
        [SerializeField] private float _destroyAnimationTime = 1f;

        public float CreateAnimationTime { get => _createAnimationTime; set => _createAnimationTime = value; }
        public float DestroyAnimationTime { get => _destroyAnimationTime; set => _destroyAnimationTime = value; }

        public virtual IEnumerator Create() { yield return null; }

        public virtual IEnumerator Destroy() { yield return null; }

        public virtual IEnumerator Show()
        {
            this.gameObject.SetActive(true);
            yield return null;
        }

        public virtual IEnumerator Hide()
        {
            yield return null;
            this.gameObject.SetActive(false);
        }

        public virtual IEnumerator OnClick() { yield return null; }

        public virtual IEnumerator OnMouseOver() { yield return null; }

    }
}

