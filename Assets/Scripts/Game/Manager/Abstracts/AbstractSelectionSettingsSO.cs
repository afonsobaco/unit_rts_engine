using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Abstracts;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Abstracts
{
    public class AbstractSelectionSettingsSO<T, E> : ScriptableObject, ISelectionSettings<T, E>
    {
        [SerializeField] private AbstractMainList<T> mainList;
        [SerializeField] private Vector2 initialGameScreenPos = new Vector2(0, 0);
        [SerializeField] private Vector2 finalGameScreenPos = new Vector2(1, 1);

        [SerializeField] private int selectionLimit = 20;
        [SerializeField] private List<E> canSelectSameType = new List<E>();
        [SerializeField] private List<E> primaryTypes = new List<E>();
        [SerializeField] private List<E> secondaryOrderedTypes = new List<E>();
        [SerializeField] private List<E> canGroupTypes = new List<E>();

        private List<IAbstractSelectionMod<T, E>> mods = new List<IAbstractSelectionMod<T, E>>();

        public Vector2 InitialGameScreenPos { get => initialGameScreenPos; set => initialGameScreenPos = value; }
        public Vector2 FinalGameScreenPos { get => finalGameScreenPos; set => finalGameScreenPos = value; }

        public int SelectionLimit { get => selectionLimit; set => selectionLimit = value; }
        public List<E> CanSelectSameType { get => canSelectSameType; set => canSelectSameType = value; }
        public List<E> PrimaryTypes { get => primaryTypes; set => primaryTypes = value; }
        public List<E> SecondaryOrderedTypes { get => secondaryOrderedTypes; set => secondaryOrderedTypes = value; }
        public List<E> CanGroupTypes { get => canGroupTypes; set => canGroupTypes = value; }
        public AbstractMainList<T> MainList { get => mainList; set => mainList = value; }
        public List<IAbstractSelectionMod<T, E>> Mods { get => mods; set => mods = value; }
    }

}