using UnityEngine;
using System;

namespace RTSEngine.Manager
{
    [CreateAssetMenu(fileName = "SelectionLimitModifier", menuName = "ScriptableObjects/Mods/Selection Limit Modifier", order = 1)]
    public class LimitModifierSO : BaseSelectionModSO
    {

        [Header("Modifier attributes: ")]
        [Space(10)]
        [Range(1, 100)]
        [SerializeField] private int maxLimit = 12;
        private ISelectionModifier selectionModifier = new LimitSelectionModifier();
        public override ISelectionModifier SelectionModifier { get => selectionModifier; set => selectionModifier = value; }
        public int Limit { get => maxLimit; set => maxLimit = value; }
        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return selectionModifier.Apply(args, Limit);
        }

    }


}
