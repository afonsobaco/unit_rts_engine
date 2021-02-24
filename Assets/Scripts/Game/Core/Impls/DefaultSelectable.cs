using UnityEngine;
using Zenject;

namespace RTSEngine.Core
{
    public class DefaultSelectable : ZenAutoInjecter, ISelectable
    {
        public virtual int Index { get; set; }
        public virtual bool IsSelected { get; set; }
        public virtual bool IsPreSelected { get; set; }
        public virtual Vector3 Position
        {
            get { return this.transform.position; }
            set
            {
                this.transform.position = value;
            }
        }
        public virtual bool IsHighlighted { get; set; }

    }

}
