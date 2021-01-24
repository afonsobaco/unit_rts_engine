using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectionArgsXP
    {
        List<ISelectable> OldSelection { get; set; }
        List<ISelectable> NewSelection { get; set; }
        List<ISelectable> ToBeAdded { get; set; }
        List<ISelectable> ToBeRemoved { get; set; }
        SelectionTypeEnum SelectionType { get; set; }
    }

}