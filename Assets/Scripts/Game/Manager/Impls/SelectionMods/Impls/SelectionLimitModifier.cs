using UnityEngine;
using System.Linq;

namespace RTSEngine.Manager.SelectionMods.Impls
{
    [CreateAssetMenu(fileName = "SelectionLimitModifier", menuName = "ScriptableObjects/Mods/Selection Limit Modifier", order = 1)]
    public class SelectionLimitModifier : BaseSelectionModSO
    {

        [Header("Modifier attributes: ")]
        [Space(10)]
        [Range(1, 100)]
        [SerializeField] private int maxLimit = 20;
        private SelectionModifier selectionModifier = new SelectionModifier();

        public int MaxLimit { get => maxLimit; set => maxLimit = value; }
        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return selectionModifier.Apply(MaxLimit, args);
        }

        public class SelectionModifier
        {
            public SelectionArgsXP Apply(int maxLimit, SelectionArgsXP args)
            {
                args.ToBeAdded = args.ToBeAdded.Take(maxLimit).ToList();
                return args;
            }
        }

    }


}
