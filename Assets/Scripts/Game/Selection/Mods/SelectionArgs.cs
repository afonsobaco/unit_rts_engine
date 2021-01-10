using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Selection.Mod
{
    public class SelectionArgs
    {
        private List<SelectableObject> mainList;        
        private List<SelectableObject> newList;
        private List<SelectableObject> oldList;
        private Camera camera;
        private Vector2 initialPos;
        private Vector2 finalPos;
        private SelectableObject clicked;
        private bool isAditive;
        private bool isSameType;
        private bool isPreSelection;
        private bool isDoubleClick;

        public List<SelectableObject> MainList { get => mainList; set => mainList = value; }
        public List<SelectableObject> NewList { get => newList; set => newList = value; }
        public List<SelectableObject> OldList { get => oldList; set => oldList = value; }
        public Camera Camera { get => camera; set => camera = value; }
        public Vector2 InitialPos { get => initialPos; set => initialPos = value; }
        public Vector2 FinalPos { get => finalPos; set => finalPos = value; }
        public SelectableObject Clicked { get => clicked; set => clicked = value; }
        public bool IsAditive { get => isAditive; set => isAditive = value; }
        public bool IsSameType { get => isSameType; set => isSameType = value; }
        public bool IsPreSelection { get => isPreSelection; set => isPreSelection = value; }
        public bool IsDoubleClick { get => isDoubleClick; set => isDoubleClick = value; }
    }

}