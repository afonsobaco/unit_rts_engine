using System.Collections;
using UnityEngine;

namespace RTSEngine.Integration.Scene
{
    public class UserInterfaceSceneIntegratedLog : UserInterfaceSceneIntegratedContent
    {
        
        [SerializeField] private float _lifetime = 3f;
        [SerializeField] private float _fadeTime = 2f;

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

    }
}

