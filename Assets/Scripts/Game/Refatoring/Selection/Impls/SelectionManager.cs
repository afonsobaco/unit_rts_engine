using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class SelectionManager
    {
        private IAreaSelection _areaSelection;
        private IPartySelection _partySelection;
        private IIndividualSelection _individualSelection;

        public SelectionManager(IAreaSelection areaSelection, IPartySelection partySelection, IIndividualSelection individualSelection)
        {
            this._areaSelection = areaSelection;
            this._partySelection = partySelection;
            this._individualSelection = individualSelection;
        }

        public virtual ISelectable[] GetAreaSelection(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint)
        {
            return _areaSelection.GetSelection(mainList, startPoint, endPoint);
        }

        public virtual ISelectable[] GetPartySelection(ISelectable[] mainList, object partyId)
        {
            return _partySelection.GetSelection(mainList, partyId);
        }

        public virtual void SetPartySelection(ISelectable[] selectables, object partyId)
        {
            _partySelection.ChangeParty(partyId, selectables);
        }

        public virtual ISelectable[] GetIndividualSelection(ISelectable[] mainList, ISelectable clicked)
        {
            return _individualSelection.GetSelection(mainList, clicked);
        }

    }
}