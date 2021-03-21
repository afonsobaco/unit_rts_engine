using Zenject;
using UnityEngine;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class UIContent : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {

        [Inject] private UIContainer container;
        private UIContentInfo info;

        public UIContentInfo Info { get => info; set => info = value; }
        public UIContainer Container { get => container; set => container = value; }

        private IMemoryPool _pool;

        public void Dispose()
        {
            if (_pool != null)
                _pool.Despawn(this);
            this.transform.SetAsLastSibling();
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            this.transform.SetSiblingIndex(container.GetComponentsInChildren<UIContent>().Length);
            _pool = pool;
        }

        public virtual void UpdateAppearance()
        {
            Debug.Log("Update appearance ");
        }
    }
}