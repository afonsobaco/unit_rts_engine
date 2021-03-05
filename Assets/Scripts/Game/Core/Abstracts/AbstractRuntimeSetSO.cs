using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    public abstract class AbstractRuntimeSetSO<T> : ScriptableObject, IRuntimeSet<T>
    {
        [SerializeField] private List<T> _mainList = new List<T>();

        public virtual List<T> GetMainList()
        {
            return _mainList;
        }

        public virtual void SetMainList(List<T> value)
        {
            _mainList = value;
        }

        public virtual void Add(T item)
        {
            if (item != null && !_mainList.Contains(item))
            {
                _mainList.Add(item);
            }
        }

        public virtual void Remove(T item)
        {
            if (item != null && _mainList.Contains(item))
            {
                _mainList.Remove(item);
            }
        }

        public virtual T GetItem(int index)
        {
            if (index < _mainList.Count)
            {
                return _mainList.ElementAt(index);
            }
            return default(T);
        }

    }
}
