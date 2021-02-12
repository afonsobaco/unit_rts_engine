
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{

    public class IndividualSelection : IIndividualSelection
    {
        public ISelectable[] GetSelection(ISelectable[] mainList, ISelectable clicked)
        {
            if (mainList == null || clicked == null)
            {
                return new ISelectable[] { };
            }
            return new ISelectable[] { clicked };
        }
    }
}
