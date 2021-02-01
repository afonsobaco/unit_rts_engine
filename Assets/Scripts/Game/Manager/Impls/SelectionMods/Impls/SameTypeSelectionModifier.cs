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
                GetAllFromSameType(args.ToBeAdded.First(), args.MainList, selectionManager.GetInitialScreenPosition(), selectionManager.GetFinalScreenPosition(), mode);
            return allFromSameType;
        }

        public HashSet<ISelectableObjectBehaviour> GetAllFromSameType(ISelectableObjectBehaviour selected, HashSet<ISelectableObjectBehaviour> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition, SameTypeSelectionModeEnum mode)
        {
            HashSet<ISelectableObjectBehaviour> list = new HashSet<ISelectableObjectBehaviour>();
            if (selected != null)
            {
                list.Add(selected);
                HashSet<ISelectableObjectBehaviour> allFromSameType = GetFromSameTypeInScreen(selected, mainList, initialScreenPosition, finalScreenPosition);
                if (mode == SameTypeSelectionModeEnum.RANDOM)
                {
                    list.UnionWith(SameTypeUtil.Shuffle(allFromSameType));
                }
                else
                {
                    list.UnionWith(SameTypeUtil.SortListByDistance(allFromSameType, selected.Position));
                }
            }
            return list;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetFromSameTypeInScreen(ISelectableObjectBehaviour selected, HashSet<ISelectableObjectBehaviour> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            return SameTypeUtil.GetFromSameTypeInScreen(selected, mainList, initialScreenPosition, finalScreenPosition);
        }
    }
}
