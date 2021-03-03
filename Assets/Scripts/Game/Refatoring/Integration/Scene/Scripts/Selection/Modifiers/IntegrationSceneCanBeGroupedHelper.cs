using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;
namespace RTSEngine.Refactoring
{
    public class IntegrationSceneCanBeGroupedHelper : AbstractModifierHelper
    {
        [SerializeField] private List<IntegrationSceneObjectTypeEnum> types;
        public override ISelectable[] Apply(ISelectable[] selection)
        {
            List<ISelectable> selectables = selection.ToList();
            selectables.RemoveAll(x =>
                {
                    return TypesDoesNotContainsElement(x);
                });
            if (selectables.Count > 0)
            {
                return selectables.ToArray();
            }
            else if (selection.Length > 0)
            {
                selectables.Add(selection[0]);
            }

            return selectables.ToArray();
        }

        private bool TypesDoesNotContainsElement(ISelectable element)
        {
            var gameType = GameUtils.GetComponent<IntegrationSceneObject, IntegrationSceneGameType>(element);
            return gameType && !types.Contains(gameType.Type);
        }
    }
}
