using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIBannerContentInfo : UIContentInfo
    {
        private int _key;
        public int Key { get => _key; set => _key = value; }
    }
}