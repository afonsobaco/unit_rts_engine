using System.Collections.Generic;


namespace RTSEngine.Core
{
    public interface IRuntimeSet<T>
    {
        List<T> GetMainList();
        T GetItem(int index);
        void Add(T item);
        void Remove(T item);
    }
}
