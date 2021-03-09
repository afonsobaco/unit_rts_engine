using UnityEngine;
using RTSEngine.RTSUserInterface.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultUserInterfaceLogManager : MonoBehaviour, ILogMessageTarget
    {

        [Inject] private UserInterfaceBase userInterfaceBase;

        public RectTransform LogPanel { get => userInterfaceBase.UserInterfaceBaseComponent.LogPanel; }
        public void AddLog(DefaultLogText log)
        {
            log.transform.SetParent(LogPanel, false);
            log.transform.SetAsFirstSibling();
        }

        public void Clear()
        {
            UserInterfaceUtils.ClearPanel(LogPanel);
        }
    }

}