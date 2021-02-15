// using System.Security.Cryptography.X509Certificates;
// using System.Linq;
// using System.Collections.Generic;
// using RTSEngine.Core;
// using UnityEngine;
// using Zenject;

// namespace RTSEngine.Refactoring
// {
//     public class GroupRestrictorSelectionModifier : MonoBehaviour, ISelectionModifier
//     {

//         private Modifier modifier;

//         private void Start()
//         {
//             StartVariables();
//         }

//         private void OnValidate()
//         {
//             if (modifier != null)
//             {
//                 StartVariables();
//             }
//         }

//         private void StartVariables()
//         {

//         }

//         public class Modifier : ISelectionModifier
//         {
//             public SelectionType Type { get; set; }
//             public int Limit { get; set; }

//             public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
//             {
//                 IEnumerable<ISelectable> result = actualSelection.ToList().Take(Limit);
//                 return result.ToArray();
//             }
//         }


//         // public override SelectionArguments Apply(SelectionArguments args)
//         // {
//         //     return GroupRestrictorSelection(args);
//         // }

//         // public SelectionArguments GroupRestrictorSelection(SelectionArguments args)
//         // {
//         //     ObjectTypeEnum[] canGroup = this._selectionManager.GetSettings().Restricted;
//         //     var onlyRestrictedObject = args.ToBeAdded.ToList().TrueForAll(x =>
//         //     {
//         //         if (x.SelectableObjectInfo != null)
//         //             return !canGroup.Contains(x.SelectableObjectInfo.Type);
//         //         return false;
//         //     });

//         //     if (!onlyRestrictedObject)
//         //     {

//         //         args.ToBeAdded.RemoveWhere(x =>
//         //         {
//         //             if (x.SelectableObjectInfo != null)
//         //                 return !canGroup.Contains(x.SelectableObjectInfo.Type);
//         //             return false;
//         //         });
//         //     }

//         //     return args;
//         // }
//     }
// }