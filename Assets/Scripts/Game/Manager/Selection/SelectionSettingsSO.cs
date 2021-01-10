using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Manager.Selection
{
    [CreateAssetMenu(fileName = "SelectionSettings", menuName = "ScriptableObjects/Selection Settings", order = 1)]
    public class SelectionSettingsSO : ScriptableObject
    {
        [SerializeField] private Vector2 initialGameScreenPos = new Vector2(0, 0);
        [SerializeField] private Vector2 finalGameScreenPos = new Vector2(1, 1);
        [SerializeField] private int selectionLimit = 20;
        [SerializeField] private List<SelectableTypeEnum> canSelectSameType = new List<SelectableTypeEnum>();
        [SerializeField] private List<SelectableTypeEnum> primaryTypes = new List<SelectableTypeEnum>();
        [SerializeField] private List<SelectableTypeEnum> secondaryOrderedTypes = new List<SelectableTypeEnum>();
        [SerializeField] private List<SelectableTypeEnum> canGroupTypes = new List<SelectableTypeEnum>();

        public Vector2 InitialGameScreenPos { get => initialGameScreenPos; }
        public Vector2 FinalGameScreenPos { get => finalGameScreenPos; }
        public List<SelectableTypeEnum> CanSelectSameType { get => canSelectSameType; }
        public List<SelectableTypeEnum> PrimaryTypes { get => primaryTypes; }
        public List<SelectableTypeEnum> SecondaryOrderedTypes { get => secondaryOrderedTypes; }
        public List<SelectableTypeEnum> CanGroupTypes { get => canGroupTypes; }
        public int SelectionLimit { get => selectionLimit; }
    }

}