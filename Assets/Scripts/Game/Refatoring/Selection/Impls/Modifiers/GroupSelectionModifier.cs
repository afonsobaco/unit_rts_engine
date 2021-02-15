using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class GroupSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]

        [SerializeField] private ModifierEqualityComparerComponent equalityComparer;


        private Modifier modifier = new Modifier();
        public SelectionType Type { get => type; set => type = value; }

        private void Start()
        {
            StartVariables();
        }

        private void OnValidate()
        {
            if (modifier != null)
            {
                StartVariables();
            }
        }

        private void StartVariables()
        {
            modifier.EqualityComparer = equalityComparer;
        }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return this.modifier.Apply(oldSelection, newSelection, actualSelection, type);
        }

        public class Modifier
        {
            public SelectionType Type { get; set; }
            public int Limit { get; set; }
            public IEqualityComparer<ISelectable> EqualityComparer { get; internal set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                return GetOrderedSelection(oldSelection, newSelection, actualSelection);
            }

            private ISelectable[] GetOrderedSelection(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                List<ISelectable> list = new List<ISelectable>();
                if (actualSelection.Length > 0)
                {
                    var grouped = actualSelection.GroupBy(x => x, EqualityComparer);
                    var sorted = grouped.ToList();
                    sorted.Sort(new ObjectComparer());
                    foreach (var item in sorted)
                    {
                        list.AddRange(item);
                    }
                }
                return list.ToArray();
            }
        }

        public class ObjectComparer : IComparer<IGrouping<ISelectable, ISelectable>>
        {
            public int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }
    }
}
