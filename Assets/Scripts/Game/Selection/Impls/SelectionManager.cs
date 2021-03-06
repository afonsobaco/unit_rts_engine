using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.RTSSelection
{
    public class SelectionManager
    {
        private IAreaSelection _areaSelection;
        private IIndividualSelection _individualSelection;

        public SelectionManager(IAreaSelection areaSelection, IIndividualSelection individualSelection)
        {
            this._areaSelection = areaSelection;
            this._individualSelection = individualSelection;
        }

        public virtual ISelectable[] GetAreaSelection(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint)
        {
            return _areaSelection.GetSelection(mainList, startPoint, endPoint);
        }

        public virtual ISelectable[] GetIndividualSelection(ISelectable[] mainList, ISelectable clicked)
        {
            return _individualSelection.GetSelection(mainList, clicked);
        }

    }
}