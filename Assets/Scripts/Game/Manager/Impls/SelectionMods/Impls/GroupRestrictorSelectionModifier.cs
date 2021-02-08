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
        private ISelectionManager _selectionManager;
        public GroupRestrictorSelectionModifier(ISelectionManager selectionManager)
        {
            this._selectionManager = selectionManager;
        }

        public override SelectionArguments Apply(SelectionArguments args)
        {
            return GroupRestrictorSelection(args);
        }

        public SelectionArguments GroupRestrictorSelection(SelectionArguments args)
        {
            ObjectTypeEnum[] canGroup = this._selectionManager.GetSettings().Restricted;
            var onlyRestrictedObject = args.ToBeAdded.ToList().TrueForAll(x =>
            {
                if (x.SelectableObjectInfo != null)
                    return !canGroup.Contains(x.SelectableObjectInfo.Type);
                return false;
            });

            if (!onlyRestrictedObject)
            {

                args.ToBeAdded.RemoveWhere(x =>
                {
                    if (x.SelectableObjectInfo != null)
                        return !canGroup.Contains(x.SelectableObjectInfo.Type);
                    return false;
                });
            }

            return args;
        }
    }
}