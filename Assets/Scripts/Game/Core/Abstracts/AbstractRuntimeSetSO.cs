using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    public abstract class AbstractRuntimeSetSO<T> : ScriptableObject, IRuntimeSet<T>
    {
        [SerializeField] private HashSet<T> _mainList = new HashSet<T>();

        public HashSet<T> MainList { get => _mainList; set => _mainList = value; }

        public void Add(T item)
        {
            if (item != null && !MainList.Contains(item))
            {
                MainList.Add(item);
            }
        }

        public void Remove(T item)
        {
            if (item != null && MainList.Contains(item))
            {
                MainList.Remove(item);
            }
        }

        public HashSet<T> GetAllItems()
        {
            return MainList;
        }

        public T GetItem(int index)
        {
            if (index < MainList.Count)
            {
                return MainList.ElementAt(index);
            }
            return default(T);
        }

    }
}
