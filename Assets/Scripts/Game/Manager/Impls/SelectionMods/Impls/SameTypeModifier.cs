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

        [SerializeField] private SameTypeSelectionModeEnum mode;

        private SelectionModifier selectionModifier = new SelectionModifier();

        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return selectionModifier.Apply(args, mode);
        }

        public class SelectionModifier
        {
            public SelectionArgsXP Apply(SelectionArgsXP args, SameTypeSelectionModeEnum mode)
            {
                if (args.Arguments.NewSelection.Count == 1 && args.ModifierArgs.IsSameType)
                {
                    var allFromType = GetAllFromSameTypeOnScreen(args, mode);
                    if (args.Arguments.OldSelection.Contains(args.Arguments.NewSelection[0]))
                    {
                        RemoveFromSelection(args, allFromType);
                    }
                    else
                    {
                        AddToSelection(args, allFromType);
                    }
                }
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

                args.Result = result;
            }

            public virtual List<ISelectable> GetAllFromSameTypeOnScreen(SelectionArgsXP args, SameTypeSelectionModeEnum mode)
            {
                List<ISelectable> selectables = SameTypeUtil.GetFromSameTypeInScreen(args);
                ISelectable clicked = args.Arguments.NewSelection[0];
                List<ISelectable> list = new List<ISelectable>() { clicked };
                if (mode == SameTypeSelectionModeEnum.RANDOM)
                {
                    list = list.Union(Shuffle(selectables)).ToList();
                }
                else
                {
                    list = list.Union(SortListByDistance(selectables, clicked.Position)).ToList();
                }
                return list;
            }

            public List<ISelectable> SortListByDistance(List<ISelectable> list, Vector3 initialPosittion)
            {
                list.Sort((v1, v2) => (v1.Position - initialPosittion).sqrMagnitude.CompareTo((v2.Position - initialPosittion).sqrMagnitude));
                return list;
            }

            public List<ISelectable> Shuffle(List<ISelectable> list)
            {
                System.Random rng = new System.Random();

                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    ISelectable value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
                return list;
            }


        }

    }

    public enum SameTypeSelectionModeEnum
    {
        DISTANCE,
        RANDOM
    }
}
