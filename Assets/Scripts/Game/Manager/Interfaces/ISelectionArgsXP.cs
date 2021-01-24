using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public interface ISelectionArgsXP
    {
        List<SelectableObject> OldSelection { get; set; }
        List<SelectableObject> NewSelection { get; set; }
        List<SelectableObject> ToBeAdded { get; set; }
        List<SelectableObject> ToBeRemoved { get; set; }
        SelectionTypeEnum SelectionType { get; set; }
    }

}