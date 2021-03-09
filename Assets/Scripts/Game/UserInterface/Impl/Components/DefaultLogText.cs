using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultLogText : MonoBehaviour
    {
        [SerializeField] private GameObject _log;
        [SerializeField] private float _lifetime = 3f;
        [SerializeField] private float _fadeTime = 2f;

        public GameObject Log { get => _log; set => _log = value; }
        public float Lifetime { get => _lifetime; set => _lifetime = value; }

        private void Start()
        {
            StartCoroutine(DoDestroy());
        }

        private IEnumerator DoDestroy()
        {
            yield return new WaitForSeconds(_lifetime);
            yield return StartCoroutine(DoDestroyAnimation());
            Destroy(this.gameObject);
        }

        public virtual IEnumerator DoDestroyAnimation()
        {
            float elapsedTime = 0;
            CanvasGroup cg = GetComponent<CanvasGroup>();
            while (elapsedTime < this._fadeTime)
            {
                cg.alpha = Mathf.Lerp(1f, 0f, (elapsedTime / this._fadeTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public class Factory : PlaceholderFactory<DefaultLogText>
        {
        }
    }
}
