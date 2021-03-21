using UnityEngine.EventSystems;

namespace RTSEngine.RTSUserInterface
{
    public interface IInfoMessageTarget : IEventSystemHandler
    {
        // TODO
        // void RemoveInfo(DefaultInfoButton button);
        // void AddInfo(DefaultInfoButton button);
        // void RemoveAllInfo(DefaultInfoButton[] button);
        // void AddAllInfo(DefaultInfoButton[] button);
        void Clear();
    }

}