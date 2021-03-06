using RTSEngine.Core;

namespace RTSEngine.RTSSelection
{
    public interface IIndividualSelection
    {
        ISelectable[] GetSelection(ISelectable[] mainList, ISelectable clicked);
    }
}