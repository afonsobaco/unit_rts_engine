using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface IPartySelection
    {
        void ChangeParty(object partyId, ISelectable[] selection);
        ISelectable[] GetSelection(ISelectable[] mainList, object partyId);
    }
}