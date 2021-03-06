using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSSelection
{
    [CreateAssetMenu(fileName = "AdditiveSelectionModifier", menuName = "Modifiers/AdditiveSelectionModifier")]
    public class AdditiveSelectionModifier : BaseSelectionModifier
    {

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private KeyCode _key = KeyCode.LeftShift;

        private Modifier _modifier;

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
        }

        public override ISelectable[] Apply(SelectionInfo info)
        {
            StartVariables();
            return this._modifier.Apply(Input.GetKey(_key), info.OldSelection, info.NewSelection, info.ActualSelection);
        }

        public class Modifier
        {

            public ISelectable[] Apply(bool active, ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                if (active)
                {
                    return AddOrRemoveFromSelection(oldSelection, newSelection, actualSelection);
                }
                return actualSelection;
            }

            private ISelectable[] AddOrRemoveFromSelection(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                List<ISelectable> aux = new List<ISelectable>(oldSelection);
                aux = aux.Union(actualSelection).ToList();
                if (ContainsAllSelected(oldSelection, actualSelection) || ContainsAllSelected(oldSelection, newSelection))
                {
                    aux.RemoveAll(x => actualSelection.Contains(x));
                }
                return aux.ToArray();
            }

            private bool ContainsClicked(ISelectable[] oldSelection, ISelectable[] newSelection)
            {
                return newSelection.Length == 1 && ContainsAllSelected(oldSelection, newSelection);
            }

            private bool ContainsAllSelected(ISelectable[] oldSelection, ISelectable[] actualSelection)
            {
                bool oldContainsNew = actualSelection.All(x => oldSelection.Contains(x));
                List<ISelectable> aux = new List<ISelectable>(oldSelection);
                aux.RemoveAll(x => actualSelection.Contains(x));
                return oldContainsNew && aux.Count > 0;
            }
        }
    }
}
