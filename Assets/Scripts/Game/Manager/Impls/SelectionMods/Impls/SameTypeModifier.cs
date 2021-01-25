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
                if (args.NewSelection.Count == 1 && args.SameTypeArgs.isSameType)
                {
                    var allFromType = GetAllFromSameTypeOnScreen(args);

                    if (args.OldSelection.Contains(args.NewSelection[0]))
                    {
                        args.ToBeAdded.RemoveAll(x => allFromType.Contains(x));
                        args.ToBeRemoved = args.ToBeRemoved.Union(allFromType).ToList();
                    }
                    else
                    {
                        args.ToBeAdded = args.ToBeAdded.Union(allFromType).ToList();
                    }

                }
                return args;
            }

            public virtual List<ISelectable> GetAllFromSameTypeOnScreen(SelectionArgsXP args)
            {
                return SameTypeUtil.GetFromSameTypeInScreen(args);
            }


        }

    }


}
