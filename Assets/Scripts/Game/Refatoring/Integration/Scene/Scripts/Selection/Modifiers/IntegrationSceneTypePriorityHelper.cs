using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
namespace RTSEngine.Refactoring
{
    public class IntegrationSceneTypePriorityHelper : AbstractModifierHelper
    {
        [SerializeField] private List<IntegrationSceneObjectTypeEnum> partySelectionType;
        [SerializeField] private List<IntegrationSceneObjectTypeEnum> singleSelectionType;

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

        private List<ISelectable> GetPriorityFromTypeList(ISelectable[] selection, List<IntegrationSceneObjectTypeEnum> typeList)
        {
            List<ISelectable> result = new List<ISelectable>();
            foreach (var item in selection)
            {
                if (item is IntegrationSceneObject)
                {
                    var a = item as IntegrationSceneObject;
                    var gameType = a.GetComponent<IntegrationSceneGameType>();
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
