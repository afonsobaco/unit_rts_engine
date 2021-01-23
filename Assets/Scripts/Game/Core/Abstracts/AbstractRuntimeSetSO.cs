using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    public abstract class AbstractRuntimeSetSO<T> : ScriptableObject, IRuntimeSet<T>
    {
        [SerializeField] private List<T> _list = new List<T>();

        public List<T> List { get => _list; set => _list = value; }

        public void AddToList(T item)
        {
            if (item != null && !List.Contains(item))
            {
                List.Add(item);
            }
        }

        public void RemoveFromList(T item)
        {
            if (item != null && List.Contains(item))
            {
                List.Remove(item);
            }
        }

        public List<T> GetList()
        {

            return List;
        }

        public T GetItem(int index)
        {
            if (index < List.Count)
            {
                return List[index];
            }
            return default(T);
        }

    }
}
