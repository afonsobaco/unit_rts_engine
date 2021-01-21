using RTSEngine.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core.Abstracts
{
    public abstract class AbstractRuntimeSetSO<T> : ScriptableObject, IRuntimeSet<T>
    {
        private List<T> _list = new List<T>();

        public void AddToList(T item)
        {
            if (item != null && !_list.Contains(item))
            {
                _list.Add(item);
            }
        }

        public void RemoveFromList(T item)
        {
            if (item != null && _list.Contains(item))
            {
                _list.Remove(item);
            }
        }

        public List<T> GetList()
        {

            return _list;
        }

        public T GetItem(int index)
        {
            if (index < _list.Count)
            {
                return _list[index];
            }
            return default(T);
        }

    }
}
