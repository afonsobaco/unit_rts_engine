using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class PartyUpdateSignal
    {
        public Dictionary<object, ISelectable[]> Parties;

    }
}