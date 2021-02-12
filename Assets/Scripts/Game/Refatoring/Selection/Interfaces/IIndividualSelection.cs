using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface IIndividualSelection
    {
        ISelectable[] GetSelection(ISelectable[] mainList, ISelectable clicked);
    }
}