using System.Collections.Generic;


namespace RTSEngine.Core
{
    public interface IRuntimeSet<T>
    {
        HashSet<T> GetList();
        T GetItem(int index);
        void AddToList(T item);
        void RemoveFromList(T item);
    }
}
