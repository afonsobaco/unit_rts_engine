using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class PrimaryObjectSelectedSignal : ISelectableSignal<ISelectableObject>
    {
        private ISelectableObject selectable;

        public ISelectableObject Selectable { get => selectable; set => selectable = value; }
    }
}