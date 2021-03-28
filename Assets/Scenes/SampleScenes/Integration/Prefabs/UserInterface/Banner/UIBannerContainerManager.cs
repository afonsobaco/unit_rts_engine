using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIBannerContainerManager : UILimitedContainerManager
    {
        public override UIContent AddToContainer(UIContentInfo info)
        {
            UIBannerContentInfo uIBannerContentInfo = (info as UIBannerContentInfo);
            if (!CheckIfContains(uIBannerContentInfo.Key))
            {
                Debug.Log(string.Format("Updated {0}", uIBannerContentInfo.Key));
                return null;
            }
            var content = base.AddToContainer(info);
            if (content)
            {
                SortBanners();
            }

            return content;
        }

        private bool CheckIfContains(int key)
        {
            return GetUIContentChildren().All(x => (x.Info as UIBannerContentInfo).Key != key);
        }

        private void SortBanners()
        {
            List<UIContent> items = GetUIContentChildren();
            items.Sort(new BannerComparer());
            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.SetSiblingIndex(i);
            }
        }
    }

    internal class BannerComparer : IComparer<UIContent>
    {
        public int Compare(UIContent x, UIContent y)
        {
            var xKey = (x.Info as UIBannerContentInfo).Key;
            var yKey = (y.Info as UIBannerContentInfo).Key;
            return xKey - yKey;
        }
    }
}