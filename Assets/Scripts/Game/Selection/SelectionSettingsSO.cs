using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Mod;
using UnityEngine;

namespace RTSEngine.Selection
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

        public Vector2 InitialGameScreenPos { get => initialGameScreenPos; set => initialGameScreenPos = value; }
        public Vector2 FinalGameScreenPos { get => finalGameScreenPos; set => finalGameScreenPos = value; }

        public int SelectionLimit { get => selectionLimit; set => selectionLimit = value; }
        public List<SelectableTypeEnum> CanSelectSameType { get => canSelectSameType; set => canSelectSameType = value; }
        public List<SelectableTypeEnum> PrimaryTypes { get => primaryTypes; set => primaryTypes = value; }
        public List<SelectableTypeEnum> SecondaryOrderedTypes { get => secondaryOrderedTypes; set => secondaryOrderedTypes = value; }
        public List<SelectableTypeEnum> CanGroupTypes { get => canGroupTypes; set => canGroupTypes = value; }
    }

}