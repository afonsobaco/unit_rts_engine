using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class LimitSelectionModifier : MonoBehaviour
    {
        [SerializeField] private SelectionType type;
        [SerializeField] [Range(1, 100)] private int limit = 20;

        private Modifier modifier;

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
            modifier.Type = type;
        }

        public class Modifier : ISelectionModifier
        {
            public SelectionType Type { get; set; }
            public int Limit { get; set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                IEnumerable<ISelectable> result = actualSelection.ToList().Take(Limit);
                return result.ToArray();
            }
        }
    }




}
