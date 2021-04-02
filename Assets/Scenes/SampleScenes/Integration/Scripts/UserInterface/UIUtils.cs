using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public static class UIUtils
    {
        public static IntegrationSceneObject GetSelectable(UIContentInfo info)
        {
            return (info as UISceneIntegratedContentInfo).Selectable as IntegrationSceneObject;
        }

        public static List<UIContentInfo> GetInfoFromContent(List<UIContent> contentList)
        {
            return contentList.Select(x => x.Info).ToList();
        }

        public static List<IntegrationSceneObject> GetSelectableFromContent(List<UIContent> contentList)
        {
            return contentList.Select(x => GetSelectable(x.Info)).ToList();
        }

        public static List<IntegrationSceneObject> GetSelectableFromContentInfo(List<UIContentInfo> contentInfoList)
        {
            return contentInfoList.Select(x => GetSelectable(x)).ToList();
        }

        public static List<UIContentInfo> CreateContentInfoBySelection(ISelectable[] selection)
        {
            List<UIContentInfo> result = new List<UIContentInfo>();
            for (var i = 0; i < selection.Length; i++)
            {
                var info = new UISceneIntegratedContentInfo();
                info.Selectable = selection[i];
                result.Add(info);
            }
            return result;
        }
    }
}