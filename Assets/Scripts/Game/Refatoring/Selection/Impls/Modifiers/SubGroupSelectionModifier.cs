using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;
using System;

namespace RTSEngine.Refactoring
{
    public class SubGroupSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;

        [Space]
        [Header("Modifier attributes")]
        [SerializeField] private KeyCode key = KeyCode.LeftControl;
        [SerializeField] private Vector2 initialViewportPoint = Vector2.zero;
        [SerializeField] private Vector2 finalViewportPoint = Vector2.one;
        [SerializeField] private EqualityComparerComponent equalityComparer;
        [Inject] private IRuntimeSet<ISelectable> mainList;
        [Inject] private IAreaSelectionType areaSelectionType;

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
            modifier.EqualityComparer = equalityComparer;
        }

        public ISelectable[] Apply(ref ISelectable[] oldSelection, ref ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            return this.modifier.Apply(Input.GetKey(key), actualSelection);
        }

        public class Modifier
        {
            public SelectionType Type { get; set; }
            public Vector2 InitialViewportPoint { get; set; }
            public Vector2 FinalViewportPoint { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IAreaSelectionType AreaSelectionType { get; set; }
            public EqualityComparerComponent EqualityComparer { get; internal set; }

            public ISelectable[] Apply(bool active, ISelectable[] actualSelection)
            {
                if (active && actualSelection.Length == 1)
                    return GetAllGroupableOnScreen(actualSelection.First());
                return actualSelection;
            }

            public virtual ISelectable[] GetAllGroupableOnScreen(ISelectable selected)
            {
                List<ISelectable> list = new List<ISelectable>();
                if (selected != null)
                {
                    list.Add(selected);
                    List<ISelectable> allFromSubGroup = GetFromSubGroupOnScreen(selected);
                    list = list.Union(allFromSubGroup).ToList();
                }
                return list.ToArray();
            }

            public virtual List<ISelectable> GetFromSubGroupOnScreen(ISelectable selected)
            {
                return SubGroupUtil.GetFromSubGroupOnScreen(GetMainList(), selected, IsInsideViewportPoints, EqualityComparer);
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
