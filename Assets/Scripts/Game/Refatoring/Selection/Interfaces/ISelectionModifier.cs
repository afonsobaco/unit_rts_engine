using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface ISelectionModifier
    {
        SelectionType Type { get; set; }
        ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type);
    }

}