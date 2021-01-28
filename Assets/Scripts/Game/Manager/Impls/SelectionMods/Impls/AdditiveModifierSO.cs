using UnityEngine;
using System;

namespace RTSEngine.Manager
{
    [CreateAssetMenu(fileName = "AdditiveModifier", menuName = "ScriptableObjects/Mods/Additive Modifier", order = 1)]
    public partial class AdditiveModifierSO : BaseSelectionModSO
    {
        private ISelectionModifier selectionModifier = new AdditiveSelectionModifier();

        public override ISelectionModifier SelectionModifier { get => selectionModifier; set => selectionModifier = value; }
        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return selectionModifier.Apply(args);
        }

    }


}
