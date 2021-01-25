using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using System;

namespace RTSEngine.Manager.SelectionMods.Impls
{
    [CreateAssetMenu(fileName = "AdditiveModifier", menuName = "ScriptableObjects/Mods/Additive Modifier", order = 1)]
    public class AdditiveModifier : BaseSelectionModSO
    {
        private SelectionModifier selectionModifier = new SelectionModifier();

        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return selectionModifier.Apply(args);
        }

        public class SelectionModifier
        {
            public SelectionArgsXP Apply(SelectionArgsXP args)
            {
                //TODO add random to be remove to simulate other Mods
                if (args.IsAdditive)
                {
                    //o[0,1] n[0] => a[1] r[0, 4] 
                    if (ContainsAllSelected(args.SelectionType, args.OldSelection, args.NewSelection))
                    {
                        args.ToBeRemoved.RemoveAll(x => args.OldSelection.Contains(x));
                        args.ToBeRemoved = args.ToBeRemoved.Union(args.NewSelection).ToList();
                        args.ToBeAdded = args.OldSelection.FindAll(x => !args.NewSelection.Contains(x));
                    }
                    else
                    {
                        args.ToBeAdded = args.OldSelection.Union(args.ToBeAdded).ToList();
                        args.ToBeRemoved.RemoveAll(x => args.OldSelection.Contains(x));
                    }
                }
                return args;
            }

            private bool ContainsAllSelected(SelectionTypeEnum selectionType, List<ISelectable> oldSelection, List<ISelectable> newSelection)
            {
                bool types = selectionType == SelectionTypeEnum.CLICK || selectionType == SelectionTypeEnum.KEY;
                bool containsAll = newSelection.TrueForAll(x => oldSelection.Contains(x));
                bool differentCounts = oldSelection.Count != newSelection.Count;
                return types && containsAll && differentCounts;
            }
        }

    }


}
