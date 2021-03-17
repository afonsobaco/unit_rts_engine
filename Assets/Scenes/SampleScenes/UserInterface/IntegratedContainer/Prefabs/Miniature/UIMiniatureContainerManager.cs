using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;
using System;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UIMiniatureContainerManager : UIContainerManager
    {

        private List<UIMiniatureSelectable> selection = new List<UIMiniatureSelectable>();

        public override UIContent AddToContainer(UIAddContentSignal signal)
        {
            var selectable = GetSelectable(signal.Info);
            if (!selection.Contains(selectable))
            {
                selection.Add(selectable);
                UIContent uIContent = base.AddToContainer(signal);
                SortMiniaturePanel();
                return uIContent;
            }
            return null;
        }

        private void SortMiniaturePanel()
        {
            List<UIContent> contents = GetUIContentChildren();
            contents.Sort(new UIMiniatureComparer());

            for (var i = 0; i < contents.Count; i++)
            {
                contents[i].transform.SetSiblingIndex(i);
            }
        }

        private static UIMiniatureSelectable GetSelectable(UIContentInfo info)
        {
            return ((info as UIMiniatureContentInfo).Selectable as UIMiniatureSelectable);
        }
    }

    internal class UIMiniatureComparer : IComparer<UIContent>
    {
        public int Compare(UIContent x, UIContent y)
        {
            var xKey = ((x.Info as UIMiniatureContentInfo).Selectable as UIMiniatureSelectable).Type;
            var yKey = ((y.Info as UIMiniatureContentInfo).Selectable as UIMiniatureSelectable).Type;
            return xKey - yKey;
        }
    }
}