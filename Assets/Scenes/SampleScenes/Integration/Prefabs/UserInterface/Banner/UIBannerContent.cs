using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIBannerContent : UIContent
    {
        [SerializeField] private Text key;

        public override void UpdateAppearance()
        {
            key.text = (Info as UIBannerContentInfo).Key.ToString();
        }
    }
}