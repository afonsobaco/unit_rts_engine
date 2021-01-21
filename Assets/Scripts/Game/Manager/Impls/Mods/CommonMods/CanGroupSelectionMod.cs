using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public class CanGroupSelectionMod<T, E, O> : AbstractSelectionMod<T, E, O>
    {
        //     protected override List<SelectableObject> Apply(SelectionArgs args)
        //     {
        //         var allNewObjectsThatCanGroup = args.NewList.FindAll(a => args.Settings.CanGroupTypes.Contains(a.type));
        //         if (allNewObjectsThatCanGroup.Count > 0)
        //         {
        //             return allNewObjectsThatCanGroup;
        //         }
        //         else if (args.NewList.Count > 0)
        //         {
        //             return new List<SelectableObject>() { args.NewList[Random.Range(0, args.NewList.Count)] };

        //         }
        //         else if (args.OldList.Count > 0)
        //         {
        //             return args.OldList;
        //         }
        //         else
        //         {
        //             return args.NewList;
        //         }
        //     }

        public override SelectionArgsXP<T, E, O> Apply(SelectionArgsXP<T, E, O> args)
        {
            throw new System.NotImplementedException();
        }
    }

}