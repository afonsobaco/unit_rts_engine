using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class AdditiveSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private KeyCode key = KeyCode.LeftShift;

        private Modifier modifier = new Modifier();
        public SelectionType Type { get => type; set => type = value; }

        private void Update()
        {
            modifier.Active = Input.GetKey(key);
        }

        public ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            return this.modifier.Apply(ref oldSelection, ref newSelection, actualSelection);
        }

        public class Modifier
        {
            public bool Active { get; set; }

            public ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                if (Active)
                {
                    return AddOrRemoveFromSelection(ref oldSelection, ref newSelection, actualSelection);
                }
                return actualSelection;
            }

            private ISelectable[] AddOrRemoveFromSelection(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                List<ISelectable> aux = new List<ISelectable>(oldSelection);
                aux = aux.Union(actualSelection).ToList();
                if (ContainsAllSelected(oldSelection, newSelection))
                {
                    aux.RemoveAll(x => actualSelection.Contains(x));
                }
                return aux.ToArray();
            }

            private bool ContainsAllSelected(ISelectable[] oldSelection, ISelectable[] newSelection)
            {
                bool oldContainsNew = newSelection.All(x => oldSelection.Contains(x));
                List<ISelectable> aux = new List<ISelectable>(oldSelection);
                aux.RemoveAll(x => newSelection.Contains(x));
                return oldContainsNew && aux.Count > 0;
            }
        }
    }
}
