using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class GroupRestrictorSelectionModifier : AbstractSelectionModifier
    {
        private readonly ISelectionManager<ISelectableObject, SelectionTypeEnum> _selectionManager;
        public GroupRestrictorSelectionModifier(ISelectionManager<ISelectableObject, SelectionTypeEnum> selectionManager)
        {
            this._selectionManager = selectionManager;
        }

        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return GroupRestrictorSelection(args);
        }

        public SelectionArgsXP GroupRestrictorSelection(SelectionArgsXP args)
        {
            ObjectTypeEnum[] canGroup = this._selectionManager.GetSettings().Restricted;
            var onlyRestrictedObject = args.ToBeAdded.ToList().TrueForAll(x =>
            {
                return !canGroup.Contains(x.SelectableObjectInfo.Type);
            });

            if (!onlyRestrictedObject)
            {
                args.ToBeAdded.RemoveWhere(x => !canGroup.Contains(x.SelectableObjectInfo.Type));
            }

            return args;
        }
    }
}