using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    public interface IMainList<T>
    {
        List<T> List { get; }
        void AddToMainList(T t);
        void RemoveFromMainList(T t);
    }

    public abstract class MainList<T> : MonoBehaviour, IMainList<T>
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