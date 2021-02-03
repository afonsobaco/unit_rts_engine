using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectionArgsXP : ISelectionArgsXP<ISelectableObject>
    {
        public HashSet<ISelectableObject> OldSelection { get; }
        public HashSet<ISelectableObject> NewSelection { get; }
        public HashSet<ISelectableObject> MainList { get; }
        public HashSet<ISelectableObject> ToBeAdded { get; set; }
        public SelectionArgsXP(HashSet<ISelectableObject> oldSelection, HashSet<ISelectableObject> newSelection, HashSet<ISelectableObject> mainList)
        {
            OldSelection = oldSelection;
            NewSelection = newSelection;
            MainList = mainList;
            ToBeAdded = newSelection;
        }

    }

}