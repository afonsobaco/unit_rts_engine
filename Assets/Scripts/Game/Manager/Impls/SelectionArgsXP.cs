using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectionArgsXP : ISelectionArgsXP
    {
        private List<ISelectable> oldSelection = new List<ISelectable>();
        private List<ISelectable> newSelection = new List<ISelectable>();
        private List<ISelectable> toBeAdded = new List<ISelectable>();
        private List<ISelectable> toBeRemoved = new List<ISelectable>();
        private SelectionTypeEnum selectionType;
        private bool isPreSelection;
        private bool isAdditive;
        private SameTypeArgs sameTypeArgs;

        public List<ISelectable> OldSelection { get => oldSelection; set => oldSelection = value; }
        public List<ISelectable> NewSelection { get => newSelection; set => newSelection = value; }
        public List<ISelectable> ToBeAdded { get => toBeAdded; set => toBeAdded = value; }
        public List<ISelectable> ToBeRemoved { get => toBeRemoved; set => toBeRemoved = value; }
        public SelectionTypeEnum SelectionType { get => selectionType; set => selectionType = value; }
        public bool IsPreSelection { get => isPreSelection; set => isPreSelection = value; }
        public bool IsAdditive { get => isAdditive; set => isAdditive = value; }
        public SameTypeArgs SameTypeArgs { get => sameTypeArgs; set => sameTypeArgs = value; }
    }

    public struct SameTypeArgs
    {
        public bool isSameType;
        public Vector2 initialScreenPosition;
        public Vector2 finalScreenPosition;
    }

}