using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface IGroupSelection
    {
        void ChangeGroup(object groupId, ISelectable[] selection);
        ISelectable[] GetSelection(ISelectable[] mainList, object groupId);
    }
}