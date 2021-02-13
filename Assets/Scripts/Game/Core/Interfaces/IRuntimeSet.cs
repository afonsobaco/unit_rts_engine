using System.Collections.Generic;


namespace RTSEngine.Core
{
    public interface IRuntimeSet<T>
    {
        HashSet<T> GetAllItems();
        T GetItem(int index);
        void Add(T item);
        void Remove(T item);
    }
}
