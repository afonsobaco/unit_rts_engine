using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using RTSEngine.Core;

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
            {
                StartVariables();
            }
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


        public class Modifier : ISelectionModifier
        {
            public SelectionType Type { get; set; }
            public bool Active { get; set; }
            public Vector2 InitialViewportPoint { get; set; }
            public Vector2 FinalViewportPoint { get; set; }
            public IRuntimeSet<ISelectable> MainList { get; set; }
            public IAreaSelectionType AreaSelectionType { get; set; }

            public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
            {
                if (Active && newSelection.Length == 1)
                {
                    return GetAllFromSameTypeThatCanGroup(oldSelection, newSelection, actualSelection);
                }

                return actualSelection;
            }

            public virtual ISelectable[] GetAllFromSameTypeThatCanGroup(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
            {
                ISelectable selected = newSelection.First();
                List<ISelectable> allFromSameType = new List<ISelectable>();
                if (selected is IGroupable)
                {
                    allFromSameType = GetAllFromSameType(selected);
                }
                else
                {
                    allFromSameType.Add(selected);
                }
                return allFromSameType.ToArray();
            }

            public virtual List<ISelectable> GetAllFromSameType(ISelectable selected)
            {
                List<ISelectable> list = new List<ISelectable>();
                if (selected != null)
                {
                    list.Add(selected);
                    List<ISelectable> allFromSameType = GetFromSameTypeInViewport(selected);
                    list = list.Union(allFromSameType).ToList();
                }
                return list;
            }

            public virtual List<ISelectable> GetFromSameTypeInViewport(ISelectable selected)
            {
                List<ISelectable> result = new List<ISelectable>();
                foreach (var item in GetAllInsideViewportPoints(selected))
                {
                    if (item is IGroupable)
                    {
                        var obj = item as IGroupable;
                        if (obj.IsCompatible(selected))
                        {
                            result.Add(item);
                        }
                    }
                }
                return result;
            }

            public virtual List<ISelectable> GetAllInsideViewportPoints(ISelectable selected)
            {
                List<ISelectable> result = new List<ISelectable>();
                foreach (var item in GetMainList())
                {
                    if (AreaSelectionType.IsInsideViewportPoints(InitialViewportPoint, FinalViewportPoint, item))
                    {
                        result.Add(item);
                    }
                }

                ISelectable[] selectables = DistanceHelper.SortWorldSpace(result, selected.Position);
                return selectables.ToList();
            }

            public virtual HashSet<ISelectable> GetMainList()
            {
                return MainList.GetAllItems();
            }
        }
    }
}
