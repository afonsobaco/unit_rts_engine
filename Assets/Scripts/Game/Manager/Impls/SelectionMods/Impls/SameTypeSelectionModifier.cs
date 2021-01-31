using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace RTSEngine.Manager
{
    public class SameTypeSelectionModifier : ISelectionModifier
    {

        private ISelectionManager<ISelectableObjectBehaviour, IBaseSelectionMod, SelectionTypeEnum> selectionManager;

        [Inject]
        public void Construct(ISelectionManager<ISelectableObjectBehaviour, IBaseSelectionMod, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public SelectionArgsXP Apply(SelectionArgsXP args, params object[] other)
        {
            if (selectionManager.IsSameType() && args.ToBeAdded.Count == 1)
            {
                args.ToBeAdded = GetAllFromSameTypeThatCanGroup(args, other);
            }
            return args;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetAllFromSameTypeThatCanGroup(SelectionArgsXP args, object[] other)
        {
            SameTypeSelectionModeEnum mode = GetMode(other);
            HashSet<ISelectableObjectBehaviour> allFromSameType =
                SameTypeUtil.GetAllFromSameType(args.ToBeAdded.First(), args.MainList, selectionManager.GetInitialScreenPosition(), selectionManager.GetFinalScreenPosition(), mode);
            return allFromSameType;
        }

        private static SameTypeSelectionModeEnum GetMode(object[] other)
        {
            var mode = SameTypeSelectionModeEnum.DISTANCE;
            if (other.Length > 0 && other[0] is SameTypeSelectionModeEnum)
            {
                mode = (SameTypeSelectionModeEnum)other[0];
            }
            return mode;
        }

    }
}
