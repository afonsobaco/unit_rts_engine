using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Selection;
using System.Linq;

namespace RTSEngine.Selection.Mod
{
    public abstract class AbstractSelectionMod : MonoBehaviour
    {

        [SerializeField] private bool active = true;
        private SelectionTypeEnum type;

        public bool Active { get => active; set => active = value; }
        public SelectionTypeEnum Type { get => type; set => type = value; }

        public List<SelectableObject> ApplyMod(SelectionArgsXP args)
        {
            return Apply(args);
        }
        protected abstract List<SelectableObject> Apply(SelectionArgsXP args);
    }
}
