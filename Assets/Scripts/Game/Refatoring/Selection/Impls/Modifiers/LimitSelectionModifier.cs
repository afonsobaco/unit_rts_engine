using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class LimitSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] [Range(1, 100)] private int limit = 20;

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
            modifier.Limit = limit;
        }

        public ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            return this.modifier.Apply(actualSelection);
        }

        public class Modifier
        {
            public int Limit { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                IEnumerable<ISelectable> result = actualSelection.ToList().Take(Limit);
                return result.ToArray();
            }
        }
    }




}
