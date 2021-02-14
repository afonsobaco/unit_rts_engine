using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class GroupRestrictorSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;

        public SelectionType Type { get => type; set => type = value; }
        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return actualSelection;
        }


        // public override SelectionArguments Apply(SelectionArguments args)
        // {
        //     return GroupRestrictorSelection(args);
        // }

        // public SelectionArguments GroupRestrictorSelection(SelectionArguments args)
        // {
        //     ObjectTypeEnum[] canGroup = this._selectionManager.GetSettings().Restricted;
        //     var onlyRestrictedObject = args.ToBeAdded.ToList().TrueForAll(x =>
        //     {
        //         if (x.SelectableObjectInfo != null)
        //             return !canGroup.Contains(x.SelectableObjectInfo.Type);
        //         return false;
        //     });

        //     if (!onlyRestrictedObject)
        //     {

        //         args.ToBeAdded.RemoveWhere(x =>
        //         {
        //             if (x.SelectableObjectInfo != null)
        //                 return !canGroup.Contains(x.SelectableObjectInfo.Type);
        //             return false;
        //         });
        //     }

        //     return args;
        // }
    }
}