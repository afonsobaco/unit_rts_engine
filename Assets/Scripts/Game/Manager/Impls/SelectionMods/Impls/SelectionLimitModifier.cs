using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager.Impls.SelectionMods.Abstracts;
using RTSEngine.Manager.Enums;
using RTSEngine.Core.Impls;
using System.Linq;

namespace RTSEngine.Manager.Impls.SelectionMods.Impls
{
    [CreateAssetMenu(fileName = "SelectionLimitModifier", menuName = "ScriptableObjects/Mods/Selection Limit Modifier", order = 1)]
    public class SelectionLimitModifier : AbstractSelectionMod<SelectableObject, SelectionTypeEnum>
    {

        [Header("Modifier attributes: ")]
        [Space(10)]
        [Range(1, 100)]
        [SerializeField] private int maxLimit = 20;

        private Modifier modifier;
        public override ISelectionArgsXP<SelectableObject, SelectionTypeEnum> Apply(ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args)
        {
            return modifier.Apply(maxLimit, args);
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
