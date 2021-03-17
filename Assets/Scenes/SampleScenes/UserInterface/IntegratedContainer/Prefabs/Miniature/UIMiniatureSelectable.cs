using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSUserInterface.Scene
{

    public class UIMiniatureSelectable : ISelectable
    {
        private int type;

        public int Index { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool IsSelected { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool IsPreSelected { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector3 Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool IsHighlighted { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Type { get => type; set => type = value; }
    }

}