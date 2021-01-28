using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SameTypeSelectionModifier : ISelectionModifier
    {
        public SelectionArgsXP Apply(SelectionArgsXP args, params object[] other)
        {
            if (args.Arguments.NewSelection.Count == 1 && args.ModifierArgs.IsSameType)
            {
                SameTypeSelectionModeEnum mode = GetMode(other);
                var allFromType = GetAllFromSameTypeOnScreen(args, mode);
                if (args.Arguments.OldSelection.Contains(args.Arguments.NewSelection[0]) && args.Arguments.OldSelection.Count > 1)
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

        private static SameTypeSelectionModeEnum GetMode(object[] other)
        {
            var mode = SameTypeSelectionModeEnum.DISTANCE;
            if (other.Length > 0 && other[0] is SameTypeSelectionModeEnum)
            {
                mode = (SameTypeSelectionModeEnum)other[0];
            }
            return mode;
        }

        private static void DebugModifier(SelectionArgsXP args, List<ISelectable> allFromType)
        {
            var all = new List<ISelectable>();
            all = all.Union(args.Arguments.OldSelection).ToList();
            all = all.Union(args.Arguments.NewSelection).ToList();
            all = all.Union(args.Result.ToBeAdded).ToList();
            all = all.Union(allFromType).ToList();
            all = all.Select((o, i) => { o.Index = i; return o; }).ToList();
            Debug.Log("OldSelection");
            args.Arguments.OldSelection.ForEach(x => Debug.Log(x.Index));
            Debug.Log("NewSelection");
            args.Arguments.NewSelection.ForEach(x => Debug.Log(x.Index));
            Debug.Log("ToBeAdded");
            args.Result.ToBeAdded.ForEach(x => Debug.Log(x.Index));
            Debug.Log("sameTypeList");
            allFromType.ForEach(x => Debug.Log(x.Index));
        }

        private static void AddToSelection(SelectionArgsXP args, List<ISelectable> allFromType)
        {
            SelectionResult result = args.Result;

            result = new SelectionResult(args.Result.ToBeAdded.Union(allFromType).ToList());

            args.Result = result;
        }

        private static void RemoveFromSelection(SelectionArgsXP args, List<ISelectable> allFromType)
        {
            SelectionResult result = args.Result;

            result = new SelectionResult(args.Arguments.OldSelection.Union(args.Result.ToBeAdded).ToList());
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
