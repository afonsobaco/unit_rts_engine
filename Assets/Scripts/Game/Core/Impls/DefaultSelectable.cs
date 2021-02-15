using UnityEngine;

namespace RTSEngine.Core
{
    public abstract class DefaultSelectable : MonoBehaviour, ISelectable
    {
        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public bool IsPreSelected { get; set; }
        public Vector3 Position
        {
            get { return this.transform.position; }
            set
            {
                this.transform.position = value;
            }
        }

        public abstract int CompareTo(object obj);
    }

}
