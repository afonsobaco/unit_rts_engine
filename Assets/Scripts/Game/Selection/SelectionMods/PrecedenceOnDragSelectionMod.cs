using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
namespace RTSEngine.Selection.Mod
{
    public class PrecedenceOnDragSelectionMod : AbstractSelectionMod
    {

        [SerializeField] protected List<SelectableTypeEnum> primaryTypes = new List<SelectableTypeEnum>();
        [SerializeField] protected List<SelectableTypeEnum> secondaryOrderedTypes = new List<SelectableTypeEnum>();
        protected override List<SelectableObject> Apply(SelectionArgs args)
        {
            if (args.Clicked)
            {
                return args.NewList;
            }
            List<SelectableObject> filteredList = args.NewList.FindAll(a => primaryTypes.Contains(a.type));
            if (filteredList.Count == 0)
            {
                foreach (var secondaryType in secondaryOrderedTypes)
                {
                    List<SelectableObject> list = args.NewList.FindAll(a => a.type == secondaryType);
                    if (list.Count > 0)
                    {
                        filteredList.Add(list[Random.Range(0, list.Count)]);
                    }
                }
            }
            if (args.IsAditive)
            {
                filteredList = filteredList.Union(args.OldList).ToList();
            }
            return filteredList;
        }
    }
}
