using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace RTSEngine.Manager
{
    public class SameTypeSelectionModifier : AbstractClickSelectionModifier
    {

        private ISelectionManager selectionManager;
        public SameTypeSelectionModifier(ISelectionManager selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public override SelectionArguments Apply(SelectionArguments args)
        {
            if (selectionManager.IsSameType() && args.ToBeAdded.Count == 1)
            {
                args.ToBeAdded = GetAllFromSameTypeThatCanGroup(args);
            }

            return args;
        }

        public virtual HashSet<ISelectableObject> GetAllFromSameTypeThatCanGroup(SelectionArguments args)
        {
            ISelectableObject selected = args.ToBeAdded.First();
            HashSet<ISelectableObject> allFromSameType = new HashSet<ISelectableObject>();
            if (selectionManager.GetSettings().CanGroup.Contains(selected.SelectableObjectInfo.Type))
            {
                allFromSameType = GetAllFromSameType(selected, args.MainList, selectionManager.GetInitialScreenPosition(), selectionManager.GetFinalScreenPosition(), selectionManager.GetSettings().Mode);
            }
            else
            {
                allFromSameType.Add(selected);
            }
            return allFromSameType;
        }

        public virtual HashSet<ISelectableObject> GetAllFromSameType(ISelectableObject selected, HashSet<ISelectableObject> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition, SameTypeSelectionModeEnum mode)
        {
            HashSet<ISelectableObject> list = new HashSet<ISelectableObject>();
            if (selected != null)
            {
                list.Add(selected);
                HashSet<ISelectableObject> allFromSameType = GetFromSameTypeInScreen(selected, mainList, initialScreenPosition, finalScreenPosition);
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

        public virtual HashSet<ISelectableObject> GetFromSameTypeInScreen(ISelectableObject selected, HashSet<ISelectableObject> mainList, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            return SameTypeUtil.GetFromSameTypeInScreen(selected, mainList, initialScreenPosition, finalScreenPosition);
        }
    }
}
