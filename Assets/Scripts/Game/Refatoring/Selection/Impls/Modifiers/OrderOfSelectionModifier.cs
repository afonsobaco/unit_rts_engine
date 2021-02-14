using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class OrderOfSelectionModifier : MonoBehaviour, ISelectionModifier
    {

        [SerializeField] private SelectionType type;

        public SelectionType Type { get => type; set => type = value; }
        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return actualSelection;
        }

        // public override SelectionArguments Apply(SelectionArguments args)
        // {
        //     var aux = GetObjectsFromListOfPriority(args.ToBeAdded, selectionManager.GetSettings().Primary.ToList());
        //     if (aux.Count == 0)
        //     {
        //         var sec = GetObjectsFromListOfPriority(args.ToBeAdded, selectionManager.GetSettings().Secondary.ToList());
        //         if (sec.Count > 0)
        //         {
        //             aux.Add(sec.First());
        //         }
        //     }
        //     args.ToBeAdded = aux;
        //     return args;
        // }

        // public virtual HashSet<ISelectableObject> GetObjectsFromListOfPriority(HashSet<ISelectableObject> toBeAdded, List<ObjectTypeEnum> objectTypeList)
        // {
        //     if (objectTypeList == null)
        //     {
        //         objectTypeList = new List<ObjectTypeEnum>();
        //     }
        //     var result = new HashSet<ISelectableObject>();
        //     foreach (var type in objectTypeList)
        //     {
        //         result.UnionWith(toBeAdded.ToList().FindAll(x =>
        //         {
        //             if (x is ISelectableObject)
        //             {
        //                 var b = x as ISelectableObject;
        //                 if (b.SelectableObjectInfo != null)
        //                 {
        //                     return b.SelectableObjectInfo.Type == type;
        //                 }
        //             }
        //             return false;
        //         }));
        //     }
        //     return result;
        // }
    }
}
