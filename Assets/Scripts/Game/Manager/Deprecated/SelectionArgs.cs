using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager.Deprecated
{
    public class SelectionArgs
    {
        private List<ISelectable> mainList;
        private List<ISelectable> newList;
        private List<ISelectable> oldList;
        private Camera camera;
        private ISelectable clicked;
        private bool isAditive;
        private bool isSameType;
        private bool isPreSelection;
        private bool isDoubleClick;
        private List<ISelectable> preSelectionList;
        public List<ISelectable> MainList { get => mainList; set => mainList = value; }
        public List<ISelectable> NewList { get => newList; set => newList = value; }
        public List<ISelectable> OldList { get => oldList; set => oldList = value; }
        public Camera Camera { get => camera; set => camera = value; }
        public ISelectable Clicked { get => clicked; set => clicked = value; }
        public bool IsAditive { get => isAditive; set => isAditive = value; }
        public bool IsSameType { get => isSameType; set => isSameType = value; }
        public bool IsPreSelection { get => isPreSelection; set => isPreSelection = value; }
        public bool IsDoubleClick { get => isDoubleClick; set => isDoubleClick = value; }
        public List<ISelectable> PreSelectionList { get => preSelectionList; set => preSelectionList = value; }
    }

}