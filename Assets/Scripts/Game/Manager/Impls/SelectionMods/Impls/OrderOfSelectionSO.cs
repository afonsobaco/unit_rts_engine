using UnityEngine;
using System;
using System.Collections.Generic;


namespace RTSEngine.Manager
{

    [CreateAssetMenu(fileName = "OrderOfSelectionModifier", menuName = "ScriptableObjects/Mods/Order of Selection Modifier", order = 1)]
    public class OrderOfSelectionSO : BaseSelectionModSO
    {

        [SerializeField] private HashSet<ObjectTypeEnum> primaryList;
        [SerializeField] private HashSet<ObjectTypeEnum> secondaryList;

        private ISelectionModifier selectionModifier = new OrderOfSelectionModifier();

        public override ISelectionModifier SelectionModifier { get => selectionModifier; set => selectionModifier = value; }

        public override SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return SelectionModifier.Apply(args, primaryList, secondaryList);
        }

    }

}