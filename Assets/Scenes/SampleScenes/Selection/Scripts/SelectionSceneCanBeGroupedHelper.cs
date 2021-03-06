using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;

namespace RTSEngine.RTSSelection.Scene
{
    public class SelectionSceneCanBeGroupedHelper : AbstractModifierHelper
    {
        [SerializeField] private List<string> types;
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
            var gameType = GameUtils.GetComponent<SelectionSceneObject, SelectionSceneGameType>(element);
            return gameType && !types.Contains(gameType.Type);
        }
    }
}
