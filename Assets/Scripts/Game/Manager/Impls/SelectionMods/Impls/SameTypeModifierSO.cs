using UnityEngine;
using System;

namespace RTSEngine.Manager
{
    [CreateAssetMenu(fileName = "SameTypeModifier", menuName = "ScriptableObjects/Mods/SameType Modifier", order = 1)]
    public class SameTypeModifierSO : BaseSelectionModSO
    {

        [SerializeField] private SameTypeSelectionModeEnum mode;

        private ISelectionModifier selectionModifier = new SameTypeSelectionModifier();

        public override ISelectionModifier SelectionModifier { get => selectionModifier; set => selectionModifier = value; }

        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return SelectionModifier.Apply(args, mode);
        }

    }

}
