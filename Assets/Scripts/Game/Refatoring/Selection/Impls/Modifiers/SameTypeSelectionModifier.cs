using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class SameTypeSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private KeyCode key = KeyCode.LeftControl;
        [SerializeField] private Vector2 initialViewportPoint = Vector2.zero;
        [SerializeField] private Vector2 finalViewportPoint = Vector2.one;

        [Inject]
        private IRuntimeSet<ISelectable> mainList;

        [Inject]
        private IAreaSelectionType areaSelectionType;

        private Modifier modifier = new Modifier();
        public SelectionType Type { get => type; set => type = value; }

        private void Start()
        {
            StartVariables();
        }

        private void OnValidate()
        {
            if (modifier != null)
                StartVariables();
        }

        private void StartVariables()
        {
            modifier.InitialViewportPoint = initialViewportPoint;
            modifier.FinalViewportPoint = finalViewportPoint;
            modifier.MainList = mainList;
            modifier.AreaSelectionType = areaSelectionType;
        }

        private void Update()
        {
            modifier.Active = Input.GetKey(key);
        }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            return this.modifier.Apply(oldSelection, newSelection, actualSelection, type);
        }

        public class Modifier
        {
            public SelectionType Type { get; set; }
            public bool Active { get; set; }
            public Vector2 InitialViewportPoint { get; set; }
            public Vector2 FinalViewportPoint { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IAreaSelectionType AreaSelectionType { get; set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                if (Active)
                    return GetAllGroupableFromSameType(oldSelection, newSelection, actualSelection);
                return actualSelection;
            }

            public virtual ISelectable[] GetAllGroupableFromSameType(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                ISelectable[] allFromSameType = null;
                if (newSelection.Length == 1)
                {
                    ISelectable selected = newSelection.First();
                    allFromSameType = GetAllGroupableOnScreen(selected);
                }
                else
                {
                    allFromSameType = actualSelection;
                }
                return allFromSameType;
            }

            public virtual ISelectable[] GetAllGroupableOnScreen(ISelectable selected)
            {
                List<ISelectable> list = new List<ISelectable>();
                if (selected != null)
                {
                    list.Add(selected);
                    List<ISelectable> allFromSameType = GetFromSameTypeOnScreen(selected);
                    list = list.Union(allFromSameType).ToList();
                }
                return list.ToArray();
            }

            public virtual List<ISelectable> GetFromSameTypeOnScreen(ISelectable selected)
            {
                return GroupableUtil.GetFromSameTypeOnScreen(GetMainList(), selected, IsInsideViewportPoints);
            }

            public virtual bool IsInsideViewportPoints(ISelectable selected)
            {
                return AreaSelectionType.IsInsideViewportPoints(InitialViewportPoint, FinalViewportPoint, selected);
            }

            public virtual ISelectable[] GetMainList()
            {
                return MainList.GetAllItems().ToArray();
            }
        }
    }
}
