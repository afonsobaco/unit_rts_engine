using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Utils;
using RTSEngine.RTSUserInterface.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultUserInterfaceLogManager : IUserInterfaceLogManager
    {

        private UserInterfaceBase _userInterfaceBase;
        private DefaultLogText.Factory _logFactory;

        public DefaultUserInterfaceLogManager(UserInterfaceBase userInterfaceBase, DefaultLogText.Factory logFactory)
        {
            _userInterfaceBase = userInterfaceBase;
            _logFactory = logFactory;
        }

        public void AddLog(string log)
        {
            AddLog(log, true);
        }

        public void AddLog(string log, bool topDown)
        {
            var logObject = CreateLog(log);
            logObject.transform.SetParent(GetLogPanel(), false);
            if (topDown)
            {
                logObject.transform.SetAsFirstSibling();
            }
        }

        public virtual DefaultLogText CreateLog(string log)
        {
            DefaultLogText logComponent = _logFactory.Create();
            GameUtils.FindInComponent<Text>(logComponent.gameObject).text = log;
            return logComponent;
        }

        public void Clear()
        {
            UserInterfaceUtils.ClearPanel(GetLogPanel());
        }

        private RectTransform GetLogPanel()
        {
            return _userInterfaceBase.UserInterfaceBaseComponent.LogPanel;
        }
    }

}