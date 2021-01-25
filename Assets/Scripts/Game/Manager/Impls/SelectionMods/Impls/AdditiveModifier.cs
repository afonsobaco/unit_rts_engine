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
                if (args.ModifierArgs.IsAdditive)
                {
                    if (ContainsAllSelected(args.Arguments.SelectionType, args.Arguments.OldSelection, args.Arguments.NewSelection))
                    {
                        RemoveFromSelection(args);
                    }
                    else
                    {
                        AddToSelection(args);
                    }
                }
                return args;
            }

            private static void AddToSelection(SelectionArgsXP args)
            {
                SelectionResult result = args.Result;

                result.ToBeAdded = args.Arguments.OldSelection.Union(args.Result.ToBeAdded).ToList();
                result.ToBeRemoved = args.Result.ToBeRemoved.FindAll(x => !args.Arguments.OldSelection.Contains(x));

                args.Result = result;
            }

            private static void RemoveFromSelection(SelectionArgsXP args)
            {
                SelectionResult result = args.Result;

                var toBeRemoved = args.Result.ToBeRemoved.FindAll(x => !args.Arguments.OldSelection.Contains(x));
                result.ToBeRemoved = toBeRemoved.Union(args.Arguments.NewSelection).ToList();
                result.ToBeAdded = args.Arguments.OldSelection.FindAll(x => !args.Arguments.NewSelection.Contains(x));

                args.Result = result;
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
