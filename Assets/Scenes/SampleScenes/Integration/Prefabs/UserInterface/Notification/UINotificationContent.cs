using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UINotificationContent : UIContent
    {
        [SerializeField] private Text title; 
        [SerializeField] private Text mainText; 
        [SerializeField] private Text subText; 
        [SerializeField] private Text toolTip; 
        public override void UpdateAppearance()
        {
            var info = Info as UINotificationContentInfo;
            title.text = info.Title;
            mainText.text = info.MainText;
            subText.text = info.SubText;
            toolTip.text = info.Tooltip;
        }
    }
}