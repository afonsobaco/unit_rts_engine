using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Selection.Mod
{
    public class CanGroupSelectionMod : AbstractSelectionMod
    {
        [SerializeField] protected List<SelectableTypeEnum> canGroupTypes = new List<SelectableTypeEnum>();
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            var allNewObjectsThatCanGroup = args.NewList.FindAll(a => canGroupTypes.Contains(a.type));
            if (allNewObjectsThatCanGroup.Count > 0)
            {
                return allNewObjectsThatCanGroup;
            }
            else if (args.NewList.Count > 0)
            {
                return new List<SelectableObject>() { args.NewList[Random.Range(0, args.NewList.Count)] };

            }
            else if (args.OldList.Count > 0)
            {
                return args.OldList;
            }
            else
            {
                return args.NewList;
            }
        }

    }

}