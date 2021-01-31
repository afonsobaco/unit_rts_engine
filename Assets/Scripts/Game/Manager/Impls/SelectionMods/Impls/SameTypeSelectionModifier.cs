using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace RTSEngine.Manager
{
    public class SameTypeSelectionModifier : ISelectionModifier
    {

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;
        public SelectionTypeEnum Type { get { return SelectionTypeEnum.CLICK; } }
        public bool ActiveOnPreSelection { get { return false; } }
        private SameTypeSelectionModeEnum mode = SameTypeSelectionModeEnum.DISTANCE;

        public SameTypeSelectionModifier(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public SelectionArgsXP Apply(SelectionArgsXP args)
        {
            if (selectionManager.IsSameType() && args.ToBeAdded.Count == 1)
            {
                args.ToBeAdded = GetAllFromSameTypeThatCanGroup(args);
            }
            return args;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetAllFromSameTypeThatCanGroup(SelectionArgsXP args)
        {
            HashSet<ISelectableObjectBehaviour> allFromSameType =
                SameTypeUtil.GetAllFromSameType(args.ToBeAdded.First(), args.MainList, selectionManager.GetInitialScreenPosition(), selectionManager.GetFinalScreenPosition(), mode);
            return allFromSameType;
        }

    }
}
