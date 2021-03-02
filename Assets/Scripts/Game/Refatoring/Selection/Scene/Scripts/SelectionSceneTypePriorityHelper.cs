using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.Refactoring.Scene.Selection
{
    public class SelectionSceneTypePriorityHelper : AbstractModifierHelper
    {
        [SerializeField] private List<string> partySelectionType;
        [SerializeField] private List<string> singleSelectionType;

        public override ISelectable[] Apply(ISelectable[] selection)
        {
            if (selection.Length == 0)
            {
                return selection;
            }
            List<ISelectable> result = new List<ISelectable>();
            var aux = GetPriorityFromTypeList(selection, partySelectionType);
            if (aux.Count > 0)
            {
                result = aux;
            }
            else
            {
                aux = GetPriorityFromTypeList(selection, singleSelectionType);
                if (aux.Count > 0)
                {
                    result.Add(aux.First());
                }
                else
                {
                    result.Add(selection.First());
                }
            }
            return result.ToArray();
        }

        private List<ISelectable> GetPriorityFromTypeList(ISelectable[] selection, List<string> typeList)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in selection)
            {
                if (item is SelectionSceneObject)
                {
                    var a = item as SelectionSceneObject;
                    var gameType = a.GetComponent<SelectionSceneGameType>();
                    if (gameType && typeList.Contains(gameType.Type))
                    {
                        result.Add(a);
                    }
                }
            }
            return result;
        }

    }
}
