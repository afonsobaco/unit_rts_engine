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
                if (args.Arguments.SelectionType != SelectionTypeEnum.ANY && args.ModifierArgs.IsAdditive && args.Arguments.NewSelection.Count > 0)
                {
                    if (!ContainsAllSelected(args.Arguments.SelectionType, args.Arguments.OldSelection, args.Arguments.NewSelection))
                    {
                        AddToSelection(args);
                    }
                    else
                    {
                        RemoveFromSelection(args);
                    }
                }
                return args;
            }

            private static void AddToSelection(SelectionArgsXP args)
            {
                SelectionResult result = args.Result;

                result.ToBeAdded = args.Arguments.OldSelection.Union(args.Result.ToBeAdded).ToList();

                args.Result = result;
            }

            private static void RemoveFromSelection(SelectionArgsXP args)
            {
                SelectionResult result = args.Result;

                result.ToBeAdded.RemoveAll(x => args.Arguments.NewSelection.Contains(x));

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
