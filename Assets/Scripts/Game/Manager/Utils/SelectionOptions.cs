using UnityEngine;

namespace RTSEngine.Manager.Utils
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
        private bool centerKeyPressed;

        public bool IsSelectionDone { get => isSelectionDone; set => isSelectionDone = value; }
        public Vector2 InitialScreenPoint { get => initialScreenPoint; set => initialScreenPoint = value; }
        public Vector2 FinalScreenPoint { get => finalScreenPoint; set => finalScreenPoint = value; }
        public int NumberKeyPressed { get => numberKeyPressed; set => numberKeyPressed = value; }
        public bool AditiveKeyPressed { get => aditiveKeyPressed; set => aditiveKeyPressed = value; }
        public bool SameTypeKeyPressed { get => sameTypeKeyPressed; set => sameTypeKeyPressed = value; }
        public bool GroupKeyPressed { get => groupKeyPressed; set => groupKeyPressed = value; }
        public bool CenterKeyPressed { get => centerKeyPressed; set => centerKeyPressed = value; }

        public override bool Equals(object obj)
        {
            return obj is SelectionOptions options &&
                   isSelectionDone == options.isSelectionDone &&
                   initialScreenPoint.Equals(options.initialScreenPoint) &&
                   finalScreenPoint.Equals(options.finalScreenPoint) &&
                   numberKeyPressed == options.numberKeyPressed &&
                   aditiveKeyPressed == options.aditiveKeyPressed &&
                   sameTypeKeyPressed == options.sameTypeKeyPressed &&
                   groupKeyPressed == options.groupKeyPressed &&
                   centerKeyPressed == options.centerKeyPressed &&
                   IsSelectionDone == options.IsSelectionDone &&
                   InitialScreenPoint.Equals(options.InitialScreenPoint) &&
                   FinalScreenPoint.Equals(options.FinalScreenPoint) &&
                   NumberKeyPressed == options.NumberKeyPressed &&
                   AditiveKeyPressed == options.AditiveKeyPressed &&
                   SameTypeKeyPressed == options.SameTypeKeyPressed &&
                   GroupKeyPressed == options.GroupKeyPressed &&
                   CenterKeyPressed == options.CenterKeyPressed;
        }

        public override int GetHashCode()
        {
            int hashCode = 140236522;
            hashCode = hashCode * -1521134295 + isSelectionDone.GetHashCode();
            hashCode = hashCode * -1521134295 + initialScreenPoint.GetHashCode();
            hashCode = hashCode * -1521134295 + finalScreenPoint.GetHashCode();
            hashCode = hashCode * -1521134295 + numberKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + aditiveKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + sameTypeKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + groupKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + centerKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + IsSelectionDone.GetHashCode();
            hashCode = hashCode * -1521134295 + InitialScreenPoint.GetHashCode();
            hashCode = hashCode * -1521134295 + FinalScreenPoint.GetHashCode();
            hashCode = hashCode * -1521134295 + NumberKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + AditiveKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + SameTypeKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + GroupKeyPressed.GetHashCode();
            hashCode = hashCode * -1521134295 + CenterKeyPressed.GetHashCode();
            return hashCode;
        }
    }

}
