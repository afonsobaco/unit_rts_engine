using UnityEngine.EventSystems;

namespace RTSEngine.RTSUserInterface
{
    public interface ILogMessageTarget : IEventSystemHandler
    {
        void AddLog(DefaultLogText log);
        void Clear();
    }

}