using UnityEngine;

namespace RTSEngine.Manager
{
    public struct SelectionOptions
    {
        private bool isSelectionDone;
        private Vector2 initialScreenPoint;
        private Vector2 finalScreenPoint;
        private int numberKeyPressed;
        private bool aditiveKeyPressed;
        private bool sameTypeKeyPressed;
        private bool groupKeyPressed;

        public bool IsSelectionDone { get => isSelectionDone; set => isSelectionDone = value; }
        public Vector2 InitialScreenPoint { get => initialScreenPoint; set => initialScreenPoint = value; }
        public Vector2 FinalScreenPoint { get => finalScreenPoint; set => finalScreenPoint = value; }
        public int NumberKeyPressed { get => numberKeyPressed; set => numberKeyPressed = value; }
        public bool AditiveKeyPressed { get => aditiveKeyPressed; set => aditiveKeyPressed = value; }
        public bool SameTypeKeyPressed { get => sameTypeKeyPressed; set => sameTypeKeyPressed = value; }
        public bool GroupKeyPressed { get => groupKeyPressed; set => groupKeyPressed = value; }

    }

}
