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

        public SelectionType Type { get => type; set => type = value; }
        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return actualSelection;
        }
        // public override SelectionArguments Apply(SelectionArguments args)
        // {
        //     if (selectionManager.IsAdditive())
        //     {
        //         AddOrRemoveFromSelection(args);
        //     }
        //     return args;
        // }



        // private void AddOrRemoveFromSelection(SelectionArguments args)
        // {
        //     HashSet<ISelectableObject> aux = new HashSet<ISelectableObject>(args.OldSelection);
        //     aux.UnionWith(args.ToBeAdded);
        //     if (ContainsAllSelected(args.OldSelection, args.NewSelection))
        //     {
        //         aux.RemoveWhere(x => args.ToBeAdded.Contains(x));
        //     }
        //     args.ToBeAdded = aux;
        // }

        // private bool ContainsAllSelected(HashSet<ISelectableObject> oldSelection, HashSet<ISelectableObject> newSelection)
        // {
        //     bool oldContainsNew = newSelection.All(x => oldSelection.Contains(x));
        //     HashSet<ISelectableObject> aux = new HashSet<ISelectableObject>(oldSelection);
        //     aux.RemoveWhere(x => newSelection.Contains(x));
        //     return oldContainsNew && aux.Count > 0;
        // }
    }
}
