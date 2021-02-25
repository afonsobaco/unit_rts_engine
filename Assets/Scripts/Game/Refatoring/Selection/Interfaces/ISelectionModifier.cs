using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface ISelectionModifier
    {
        SelectionType Type { get; set; }
        ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection);
    }

}