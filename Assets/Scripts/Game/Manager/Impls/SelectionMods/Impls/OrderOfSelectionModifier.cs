using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class OrderOfSelectionModifier : ISelectionModifier
    {

        public SelectionTypeEnum Type { get { return SelectionTypeEnum.DRAG; } }
        public bool ActiveOnPreSelection { get { return true; } }

        private List<ObjectTypeEnum> primary = new List<ObjectTypeEnum>() { ObjectTypeEnum.UNIT };
        private List<ObjectTypeEnum> secondary = new List<ObjectTypeEnum>() { ObjectTypeEnum.BUILDING };

        public SelectionArgsXP Apply(SelectionArgsXP args)
        {

            var aux = GetObjectsFromListOfPriority(args.ToBeAdded, primary);
            if (aux.Count == 0)
            {
                var sec = GetObjectsFromListOfPriority(args.ToBeAdded, secondary);
                if (sec.Count > 0)
                {
                    aux.Add(sec.First());
                }
            }
            args.ToBeAdded = aux;
            return args;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetObjectsFromListOfPriority(HashSet<ISelectableObjectBehaviour> toBeAdded, List<ObjectTypeEnum> objectTypeList)
        {
            if (objectTypeList == null)
            {
                objectTypeList = new List<ObjectTypeEnum>();
            }
            var result = new HashSet<ISelectableObjectBehaviour>();
            foreach (var type in objectTypeList)
            {
                result.UnionWith(toBeAdded.ToList().FindAll(x =>
                {
                    if (x is ISelectableObjectBehaviour)
                    {
                        var b = x as ISelectableObjectBehaviour;
                        return b.Type == type;
                    }
                    return false;
                }));
            }
            return result;
        }
    }
}
