using RTSEngine.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core.Abstracts
{
    public abstract class AbstractMainList<T> : MonoBehaviour, IMainList<T>
    {
        private List<T> list = new List<T>();
        public List<T> List { get => list; }
        public void RemoveFromMainList(T t)
        {
            this.List.Remove(t);
        }
        public void AddToMainList(T t)
        {
            this.List.Add(t);
        }
    }
}