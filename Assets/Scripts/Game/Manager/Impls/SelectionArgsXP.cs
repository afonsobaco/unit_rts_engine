using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectionArgsXP : ISelectionArgsXP<ISelectableObjectBehaviour>
    {
        public SelectionArgsXP(HashSet<ISelectableObjectBehaviour> oldSelection, HashSet<ISelectableObjectBehaviour> newSelection, HashSet<ISelectableObjectBehaviour> mainList)
        {
            OldSelection = oldSelection;
            NewSelection = newSelection;
            MainList = mainList;
            ToBeAdded = newSelection;
        }

        public HashSet<ISelectableObjectBehaviour> OldSelection { get; }
        public HashSet<ISelectableObjectBehaviour> NewSelection { get; }
        public HashSet<ISelectableObjectBehaviour> MainList { get; }
        public HashSet<ISelectableObjectBehaviour> ToBeAdded { get; set; }
    }

}