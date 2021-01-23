using UnityEngine;
using System.Linq;

namespace RTSEngine.Manager.SelectionMods.Impls
{
    [CreateAssetMenu(fileName = "SelectionLimitModifier", menuName = "ScriptableObjects/Mods/Selection Limit Modifier", order = 1)]
    public class SelectionLimitModifier : AbstractSelectionMod<SelectableObject, SelectionTypeEnum>
    {

        [Header("Modifier attributes: ")]
        [Space(10)]
        [Range(1, 100)]
        [SerializeField] private int maxLimit = 20;

        private Modifier selectionModifier;

        public int MaxLimit { get => maxLimit; set => maxLimit = value; }
        public Modifier SelectionModifier { get => selectionModifier; set => selectionModifier = value; }

        public override ISelectionArgsXP<SelectableObject, SelectionTypeEnum> Apply(ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args)
        {
            return SelectionModifier.Apply(MaxLimit, args);
        }

        public class Modifier
        {
            public ISelectionArgsXP<SelectableObject, SelectionTypeEnum> Apply(int maxLimit, ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args)
            {
                args.ToBeAdded = args.ToBeAdded.Take(maxLimit).ToList();
                return args;
            }
        }

    }


}
