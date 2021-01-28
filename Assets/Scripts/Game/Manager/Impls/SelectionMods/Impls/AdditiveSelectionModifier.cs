using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Manager
{
    public class AdditiveSelectionModifier : ISelectionModifier
    {
        public SelectionArgsXP Apply(SelectionArgsXP args, params object[] other)
        {
            if (args.ModifierArgs.IsAdditive)
            {
                AddOrRemoveFromSelection(args);
            }
            return args;
        }

        private void AddOrRemoveFromSelection(SelectionArgsXP args)
        {
            SelectionResult result = args.Result;
            List<ISelectable> toBeAdded = args.Arguments.OldSelection.Union(args.Result.ToBeAdded).ToList();
            if (ContainsAllSelected(args.Arguments.SelectionType, args.Arguments.OldSelection, args.Result.ToBeAdded))
            {
                toBeAdded.RemoveAll(x => args.Result.ToBeAdded.Contains(x));
            }
            args.Result = new SelectionResult(toBeAdded);
        }

        private bool ContainsAllSelected(SelectionTypeEnum selectionType, List<ISelectable> oldSelection, List<ISelectable> toBeAdded)
        {
            bool types = selectionType == SelectionTypeEnum.CLICK || selectionType == SelectionTypeEnum.KEY;
            bool containsAll = toBeAdded.Count > 0 && toBeAdded.TrueForAll(x => oldSelection.Contains(x));
            bool differentCounts = oldSelection.Count != toBeAdded.Count;
            return types && containsAll && differentCounts;
        }
    }
}
