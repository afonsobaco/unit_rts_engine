using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.Manager
{
    public class SelectionChangeSignal
    {
        private ISelectableObject[] selection;
        private bool additive;

        public ISelectableObject[] Selection { get => selection; set => selection = value; }
        public bool Additive { get => additive; set => additive = value; }
    }
}