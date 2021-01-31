using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class OrderOfSelectionModifier : ISelectionModifier
    {
        public SelectionArgsXP Apply(SelectionArgsXP args, params object[] other)
        {
            if (other != null && other.Length > 1)
            {
                HashSet<ObjectTypeEnum> primary = (HashSet<ObjectTypeEnum>)other[0];
                HashSet<ObjectTypeEnum> secondary = (HashSet<ObjectTypeEnum>)other[1];
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
            }
            return args;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetObjectsFromListOfPriority(HashSet<ISelectableObjectBehaviour> toBeAdded, HashSet<ObjectTypeEnum> objectTypeList)
        {
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
