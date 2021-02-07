using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.Manager
{
    public class SelectionChangeSignal
    {
        private ISelectableObject[] selection;

        public ISelectableObject[] Selection { get => selection; set => selection = value; }
    }
}