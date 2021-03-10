using UnityEngine.EventSystems;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public interface ILogMessageTarget : IEventSystemHandler
    {
        void AddLog(RectTransform log);
        void Clear();
    }

}