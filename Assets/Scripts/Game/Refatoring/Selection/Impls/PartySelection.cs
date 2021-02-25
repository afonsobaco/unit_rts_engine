
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{

    public class PartySelection : IPartySelection
    {
        private Dictionary<object, ISelectable[]> parties = new Dictionary<object, ISelectable[]>();

        public ISelectable[] GetSelection(ISelectable[] mainList, object partyId)
        {
            ISelectable[] party = new ISelectable[] { };
            if (mainList == null)
            {
                return party;
            }
            ISelectable[] found;
            parties.TryGetValue(partyId, out found);
            if (found != null)
            {
                party = found;
            }
            return party;
        }

        public void ChangeParty(object partyId, ISelectable[] selection)
        {
            parties[partyId] = selection;
        }
    }
}
