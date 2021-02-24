using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;


namespace RTSEngine.Refactoring
{
    public class GroupRestrictionHelper : AbstractModifierHelper
    {
        [SerializeField] private List<ObjectTypeEnum> types;
        public override ISelectable[] Apply(ISelectable[] selection)
        {
            List<ISelectable> selectables = selection.ToList();
            selectables.RemoveAll(x =>
                {
                    return TypesDoesNotContainsElement(x);
                });
            if (selectables.Count == 0)
            {
                return selection;
            }
            return selectables.ToArray();
        }

        private bool TypesDoesNotContainsElement(ISelectable element)
        {
            var gameType = (element as GameDefaultObject).GetComponent<GameType>();
            if (gameType)
            {
                return !types.Contains(gameType.Type);
            }
            return false;
        }
    }
}
