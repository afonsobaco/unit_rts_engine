using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Utils;

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

            public IEqualityComparer<ISelectable> EqualityComparer { get; internal set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                return GameUtils.GetOrderedSelection(actualSelection, EqualityComparer);
            }

        }
    }
}
