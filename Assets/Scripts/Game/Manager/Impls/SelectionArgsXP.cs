using System.Collections.Generic;
namespace RTSEngine.Manager
{
    public class SelectionArgsXP : ISelectionArgsXP
    {
        private List<SelectableObject> oldSelection = new List<SelectableObject>();
        private List<SelectableObject> newSelection = new List<SelectableObject>();
        private List<SelectableObject> toBeAdded = new List<SelectableObject>();
        private List<SelectableObject> toBeRemoved = new List<SelectableObject>();
        private SelectionTypeEnum selectionType;
        private bool isPreSelection;

        public List<SelectableObject> OldSelection { get => oldSelection; set => oldSelection = value; }
        public List<SelectableObject> NewSelection { get => newSelection; set => newSelection = value; }
        public List<SelectableObject> ToBeAdded { get => toBeAdded; set => toBeAdded = value; }
        public List<SelectableObject> ToBeRemoved { get => toBeRemoved; set => toBeRemoved = value; }
        public SelectionTypeEnum SelectionType { get => selectionType; set => selectionType = value; }
        public bool IsPreSelection { get => isPreSelection; set => isPreSelection = value; }
    }

}