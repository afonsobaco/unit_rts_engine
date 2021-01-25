using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using System;

namespace RTSEngine.Manager.SelectionMods.Impls
{
    [CreateAssetMenu(fileName = "SameTypeModifier", menuName = "ScriptableObjects/Mods/SameType Modifier", order = 1)]
    public class SameTypeModifier : BaseSelectionModSO
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
                if (args.Arguments.NewSelection.Count == 1 && args.ModifierArgs.IsSameType)
                {
                    var allFromType = GetAllFromSameTypeOnScreen(args);
                    Debug.Log("allFromType: " + allFromType.Count);
                    if (args.Arguments.OldSelection.Contains(args.Arguments.NewSelection[0]))
                    {
                        RemoveFromSelection(args, allFromType);
                    }
                    else
                    {
                        AddToSelection(args, allFromType);
                    }

                }
                Debug.Log("IsAdditive: " + args.ModifierArgs.IsAdditive);
                Debug.Log("IsPreSelection: " + args.Arguments.IsPreSelection);
                Debug.Log("OldSelection: " + args.Arguments.OldSelection.Count);
                Debug.Log("NewSelection: " + args.Arguments.NewSelection.Count);
                Debug.Log("ToBeAdded: " + args.Result.ToBeAdded.Count);
                Debug.Log("ToBeRemoved: " + args.Result.ToBeRemoved.Count);
                return args;
            }

            private static void AddToSelection(SelectionArgsXP args, List<ISelectable> allFromType)
            {
                SelectionResult result = args.Result;

                result.ToBeAdded = args.Result.ToBeAdded.Union(allFromType).ToList();

                args.Result = result;
            }

            private static void RemoveFromSelection(SelectionArgsXP args, List<ISelectable> allFromType)
            {
                SelectionResult result = args.Result;

                result.ToBeAdded = args.Arguments.OldSelection.Union(args.Result.ToBeAdded).ToList();
                result.ToBeAdded.RemoveAll(x => allFromType.Contains(x));
                result.ToBeRemoved = args.Result.ToBeRemoved.Union(allFromType).ToList();

                args.Result = result;
            }

            public virtual List<ISelectable> GetAllFromSameTypeOnScreen(SelectionArgsXP args)
            {
                return SameTypeUtil.GetFromSameTypeInScreen(args);
            }


        }

    }


}
