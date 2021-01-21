using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core.Interfaces
{
    public interface IMainList<T>
    {
        List<T> List { get; }
        void AddToMainList(T t);
        void RemoveFromMainList(T t);
    }

}