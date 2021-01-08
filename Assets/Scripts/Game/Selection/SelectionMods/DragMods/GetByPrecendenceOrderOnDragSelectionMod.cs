
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Selection.Mod
{

    public class GetByPrecendenceOrderOnDragSelectionMod : AbstractDragSelectionMod
    {
        [SerializeField] protected List<SelectableTypeEnum> primaryTypes = new List<SelectableTypeEnum>();
        [SerializeField] protected List<SelectableTypeEnum> secondaryOrderedTypes = new List<SelectableTypeEnum>();

        protected override List<SelectableObject> Execute(SelectionArgs args)
        {

            List<SelectableObject> filteredList = args.NewList.FindAll(a => primaryTypes.Contains(a.type));
            if (filteredList.Count == 0)
            {
                if (args.IsPreSelection)
                {
                    return new List<SelectableObject>();
                }
                filteredList = GetSecondarySelection(args);
            }
            if (args.OldList.Count > 0)
            {
                filteredList = args.OldList.Union(filteredList).ToList();
            }
            return filteredList;
        }

        private List<SelectableObject> GetSecondarySelection(SelectionArgs args)
        {
            foreach (var secondaryType in secondaryOrderedTypes)
            {
                List<SelectableObject> list = args.NewList.FindAll(a => a.type == secondaryType);
                if (list.Count > 0)
                {
                    return new List<SelectableObject>() { list[Random.Range(0, list.Count)] };
                }
            }
            return new List<SelectableObject>();
        }

    }
}
