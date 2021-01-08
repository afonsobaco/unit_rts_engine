using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Selection.Mod
{
    public abstract class AbstractSelectionMod : MonoBehaviour
    {

        public bool active = true;
        public List<SelectableObject> ApplyMod(SelectionArgs args)
        {
            if (active)
            {
                args = NormalizeArgs(args);
                return Apply(args);
            }
            return args.NewList;
        }
        private SelectionArgs NormalizeArgs(SelectionArgs args)
        {
            if(args.IsAditive){
                args.NewList = args.OldList.Union(args.NewList).ToList();
            }
            if(args.MainList == null){
                args.MainList = new List<SelectableObject>();
            }
            if(args.NewList == null){
                args.NewList = new List<SelectableObject>();
            }
            if(args.OldList == null){
                args.OldList = new List<SelectableObject>();
            }
            return args;
        }

        protected abstract List<SelectableObject> Apply(SelectionArgs args);
    }
}
