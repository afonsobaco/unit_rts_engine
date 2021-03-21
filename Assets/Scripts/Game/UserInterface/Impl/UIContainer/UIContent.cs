using Zenject;
using UnityEngine;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class UIContent : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {

        [Inject] private UIContainer _container;
        private UIContentInfo _info;
        private IMemoryPool _pool;

        public UIContentInfo Info { get => _info; set => _info = value; }
        public UIContainer Container { get => _container; set => _container = value; }


        public virtual void Dispose()
        {
            if (_pool != null)
                _pool.Despawn(this);
            this.transform.SetAsLastSibling();
            this._info = null;
        }

        public virtual void OnDespawned()
        {
            _pool = null;
        }

        public virtual void OnSpawned(IMemoryPool pool)
        {
            this.transform.SetSiblingIndex(_container.GetComponentsInChildren<UIContent>().Length);
            _pool = pool;
        }

        public virtual void UpdateAppearance()
        {
            Debug.Log("Update appearance ");
        }
    }
}