using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    public abstract class AbstractRuntimeSetSO<T> : ScriptableObject, IRuntimeSet<T>
    {
        [SerializeField] private HashSet<T> _list = new HashSet<T>();

        public HashSet<T> HashSet { get => _list; set => _list = value; }

        public void Add(T item)
        {
            if (item != null && !HashSet.Contains(item))
            {
                HashSet.Add(item);
            }
        }

        public void Remove(T item)
        {
            if (item != null && HashSet.Contains(item))
            {
                HashSet.Remove(item);
            }
        }

        public HashSet<T> GetAllItems()
        {

            return HashSet;
        }

        public T GetItem(int index)
        {
            if (index < HashSet.Count)
            {
                return HashSet.ElementAt(index);
            }
            return default(T);
        }

    }
}
