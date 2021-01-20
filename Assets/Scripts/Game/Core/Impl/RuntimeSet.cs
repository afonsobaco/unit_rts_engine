using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.Core
{
    public interface IRuntimeSet<T>
    {
        void AddToList(T item);
        T GetItem(int index);
        List<T> GetList();
        void RemoveFromList(T item);
    }

    public abstract class RuntimeSet<T> : ScriptableObject, IRuntimeSet<T>
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
