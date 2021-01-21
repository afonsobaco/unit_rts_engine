using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Abstracts;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Abstracts
{
    public class AbstractSelectionSettingsSO<T, S, O> : ScriptableObject, ISelectionSettings<T, S, O>
    {
        [SerializeField] private AbstractMainList<T> mainList;
        [SerializeField] private Vector2 initialGameScreenPos = new Vector2(0, 0);
        [SerializeField] private Vector2 finalGameScreenPos = new Vector2(1, 1);

        [SerializeField] private int selectionLimit = 20;
        [SerializeField] private List<S> canSelectSameType = new List<S>();
        [SerializeField] private List<S> primaryTypes = new List<S>();
        [SerializeField] private List<S> secondaryOrderedTypes = new List<S>();
        [SerializeField] private List<S> canGroupTypes = new List<S>();

        private List<IAbstractSelectionMod<T, S, O>> mods = new List<IAbstractSelectionMod<T, S, O>>();

        public Vector2 InitialGameScreenPos { get => initialGameScreenPos; set => initialGameScreenPos = value; }
        public Vector2 FinalGameScreenPos { get => finalGameScreenPos; set => finalGameScreenPos = value; }

        public int SelectionLimit { get => selectionLimit; set => selectionLimit = value; }
        public List<S> CanSelectSameType { get => canSelectSameType; set => canSelectSameType = value; }
        public List<S> PrimaryTypes { get => primaryTypes; set => primaryTypes = value; }
        public List<S> SecondaryOrderedTypes { get => secondaryOrderedTypes; set => secondaryOrderedTypes = value; }
        public List<S> CanGroupTypes { get => canGroupTypes; set => canGroupTypes = value; }
        public AbstractMainList<T> MainList { get => mainList; set => mainList = value; }
        public List<IAbstractSelectionMod<T, S, O>> Mods { get => mods; set => mods = value; }
    }

}