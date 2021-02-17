﻿using UnityEngine;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class SelectionManager
    {
        private IAreaSelection areaSelection;
        private IGroupSelection groupSelection;
        private IIndividualSelection individualSelection;

        public SelectionManager(IAreaSelection areaSelection, IGroupSelection groupSelection, IIndividualSelection individualSelection)
        {
            this.areaSelection = areaSelection;
            this.groupSelection = groupSelection;
            this.individualSelection = individualSelection;
        }

        public virtual ISelectable[] GetAreaSelection(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint)
        {
            return areaSelection.GetSelection(mainList, startPoint, endPoint);
        }

        public virtual ISelectable[] GetGroupSelection(ISelectable[] mainList, object groupId)
        {
            return groupSelection.GetSelection(mainList, groupId);
        }

        public virtual void SetGroupSelection(ISelectable[] selectables, object groupId)
        {
            groupSelection.ChangeGroup(groupId, selectables);
        }

        public virtual ISelectable[] GetIndividualSelection(ISelectable[] mainList, ISelectable clicked)
        {
            return individualSelection.GetSelection(mainList, clicked);
        }

    }
}