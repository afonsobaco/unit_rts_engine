using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectionArguments<T> where T : ISelectable
    {
        HashSet<T> OldSelection { get; }
        HashSet<T> NewSelection { get; }
        HashSet<T> MainList { get; }
        HashSet<T> ToBeAdded { get; set; }
    }

}