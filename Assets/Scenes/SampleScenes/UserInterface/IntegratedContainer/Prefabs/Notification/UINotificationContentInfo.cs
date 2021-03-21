using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UINotificationContentInfo : UIContentInfo
    {
        private string mainText;
        private string subText;
        private string title;
        private string tooltip;

        public string MainText { get => mainText; set => mainText = value; }
        public string SubText { get => subText; set => subText = value; }
        public string Title { get => title; set => title = value; }
        public string Tooltip { get => tooltip; set => tooltip = value; }
    }

}