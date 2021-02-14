using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class RuntimeSetComponent : MonoBehaviour, IRuntimeSet<ISelectable>
    {
        public HashSet<ISelectable> Items = new HashSet<ISelectable>();

        public void Add(ISelectable thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
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