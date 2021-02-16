using UnityEngine;
using Zenject;

namespace RTSEngine.Core
{
    public class DefaultSelectable : ZenAutoInjecter, ISelectable
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

        public virtual int CompareTo(object obj)
        {
            return 0;
        }

    }

}
