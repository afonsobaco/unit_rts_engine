using System.Collections.Generic;


namespace RTSEngine.Core.Interfaces
{
    public interface IRuntimeSet<T>
    {
        void AddToList(T item);
        T GetItem(int index);
        List<T> GetList();
        void RemoveFromList(T item);
    }
}
