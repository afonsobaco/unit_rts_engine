using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace RTSEngine.Manager
{
    public class SameTypeSelectionModifier : AbstractClickSelectionModifier
    {

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;
        public SameTypeSelectionModifier(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            if (selectionManager.IsSameType() && args.ToBeAdded.Count == 1)
            {
                args.ToBeAdded = GetAllFromSameTypeThatCanGroup(args);
            }

            return args;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetAllFromSameTypeThatCanGroup(SelectionArgsXP args)
        {
            ISelectableObjectBehaviour selected = args.ToBeAdded.First();
            HashSet<ISelectableObjectBehaviour> allFromSameType = new HashSet<ISelectableObjectBehaviour>();
            if (selectionManager.GetSettings().CanGroup.Contains(selected.Type))
            {
                allFromSameType = GetAllFromSameType(selected, args.MainList, selectionManager.GetInitialScreenPosition(), selectionManager.GetFinalScreenPosition(), selectionManager.GetSettings().Mode);
            }
            else
            {
                allFromSameType.Add(selected);
            }
            return allFromSameType;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetAllFromSameType(ISelectableObjectBehaviour selected, HashSet<ISelectableObjectBehaviour> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition, SameTypeSelectionModeEnum mode)
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
