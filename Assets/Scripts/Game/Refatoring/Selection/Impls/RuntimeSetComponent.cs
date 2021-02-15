using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class RuntimeSetComponent : MonoBehaviour, IRuntimeSet<ISelectable>
    {
        public HashSet<ISelectable> Items = new HashSet<ISelectable>();

        private int count = 0;
        public void Add(ISelectable thing)
        {
            if (!Items.Contains(thing))
            { Items.Add(thing); thing.Index = count++; }
        }

        public void Remove(ISelectable thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }

        public HashSet<ISelectable> GetAllItems()
        {
            return Items;
        }

        public ISelectable GetItem(int index)
        {
            if (index < Items.Count)
                return Items.ToList().ElementAt(index);
            return null;
        }
    }

}