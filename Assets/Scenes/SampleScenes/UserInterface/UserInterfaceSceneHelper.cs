using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Commons;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneHelper : DefaultUserInterfaceInput
    {
        private List<ISelectable> mainList = new List<ISelectable>();
        const string WIZZARD = "Wizzard";
        const string WARRIOR = "Warrior";
        const string ARCHER = "Archer";

        private UserInterface _userInterface;
        private SignalBus _signalBus;
        private IEqualityComparer<ISelectable> _equalityComparer;
        private IComparer<IGrouping<ISelectable, ISelectable>> _groupSortComparer;

        [Inject]
        public void Construct(SignalBus signalBus, UserInterface userInterface, IEqualityComparer<ISelectable> equalityComparer, IComparer<IGrouping<ISelectable, ISelectable>> groupSortComparer)
        {
            this._signalBus = signalBus;
            this._userInterface = userInterface;
            this._equalityComparer = equalityComparer;
            this._groupSortComparer = groupSortComparer;
        }

        private void Start()
        {
            for (var i = 0; i < 5; i++)
            {
                mainList.Add(CreateSelectable(WIZZARD, i));
                mainList.Add(CreateSelectable(WARRIOR, i + 5));
                mainList.Add(CreateSelectable(ARCHER, i + 10));
            }
        }

        public override void GetOtherInputs()
        {
            AddRandomSelection();
            GetHighlightedSelection();
            ClearSelection();
        }

        private void AddRandomSelection()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _signalBus.Fire(new SelectionUpdateSignal() { Selection = GetRandomSelection() });
            }
        }

        private void GetHighlightedSelection()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new SelectionUpdateSignal() { Selection = new ISelectable[] { _userInterface.Highlighted } });
            }
        }

        private void ClearSelection()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _signalBus.Fire(new SelectionUpdateSignal() { Selection = new ISelectable[] { } });
            }
        }

        private UserInterfaceSceneObject CreateSelectable(string type, int index)
        {
            var selectable = new UserInterfaceSceneObject();
            selectable.Type = type;
            selectable.Index = index;
            selectable.Position = new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
            return selectable;
        }

        private ISelectable[] GetRandomSelection()
        {
            var result = new List<ISelectable>(0);
            var length = Random.Range(0, 10);
            for (var i = 0; i < length; i++)
            {
                var a = mainList[Random.Range(0, mainList.Count)];
                if (!result.Contains(a))
                    result.Add(a);
            }
            result.Sort(new Comparer());
            return result.ToArray();
        }

        private class Comparer : IComparer<ISelectable>
        {
            public int Compare(ISelectable x, ISelectable y)
            {
                return x.Index - y.Index;
            }
        }
    }
}