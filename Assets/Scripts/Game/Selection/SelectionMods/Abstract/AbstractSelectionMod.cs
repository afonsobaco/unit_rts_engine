using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Manager;
using RTSEngine.Manager.Selection;
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
                if (args.MainList == null || args.MainList.Count == 0)
                {
                    return new List<SelectableObject>();
                }
                args = NormalizeArgs(args);
                return Apply(args);
            }
            return args.NewList;
        }
        private SelectionArgs NormalizeArgs(SelectionArgs args)
        {
            return args;
        }

        protected SelectionSettingsSO GetSelectionSettings(){
            return SelectionManager.Instance.SelectionSettings;
        }

        protected abstract List<SelectableObject> Apply(SelectionArgs args);
    }
}
