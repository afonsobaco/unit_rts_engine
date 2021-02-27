using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using Zenject;

namespace RTSEngine.Refactoring
{

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

        public override ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            StartVariables();
            return this._modifier.Apply(Input.GetKey(_key), oldSelection, newSelection, actualSelection);
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
                if (ContainsAllSelected(oldSelection, actualSelection))
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
