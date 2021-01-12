using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Selection.Mod
{
    public class SelectionArgsXP
    {
        public List<SelectableObject> oldSelection;
        public List<SelectableObject> newSelection;
        public List<SelectableObject> toBeAddedInSelection;
        public List<SelectableObject> toBeRemovedSelection;
        public SelectionTypeEnum selectionType;
        public SelectionSettingsSO settings;
    }

}