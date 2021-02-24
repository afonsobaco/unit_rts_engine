using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Utils;
using System;

namespace RTSEngine.Refactoring
{
    public class SubGroupSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]

        [SerializeField] private ModifierEqualityComparerComponent subGroupEqualityComparer;

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
            modifier.SubGroupComparer = subGroupEqualityComparer;
            modifier.EqualityComparer = subGroupEqualityComparer;
        }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return this.modifier.Apply(actualSelection);
        }

        public class Modifier
        {

            public IEqualityComparer<ISelectable> EqualityComparer { get; set; }
            public IComparer<IGrouping<ISelectable, ISelectable>> SubGroupComparer { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                if (actualSelection.Length <= 1)
                {
                    return actualSelection;
                }
                return OrderSubGroups(actualSelection);
            }

            public virtual ISelectable[] OrderSubGroups(ISelectable[] actualSelection)
            {
                var grouped = actualSelection.GroupBy(x => x, EqualityComparer).ToList();
                List<ISelectable> list = new List<ISelectable>();
                grouped.Sort(SubGroupComparer);
                foreach (var item in grouped)
                {
                    list.AddRange(item);
                }
                return list.ToArray();
            }
        }
    }
}
