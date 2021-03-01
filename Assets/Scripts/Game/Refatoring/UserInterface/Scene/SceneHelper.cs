using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Commons;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Refactoring;
using RTSEngine.Utils;
using Zenject;

public class SceneHelper : MonoBehaviour
{
    private List<ISelectable> mainList = new List<ISelectable>();
    const string WIZZARD = "Wizzard";
    const string WARRIOR = "Warrior";
    const string ARCHER = "Archer";

    private UserInterface _userInterface;
    private SignalBus _signalBus;
    private Dictionary<object, ISelectable[]> parties;

    private EqualityComparerComponent _equalityComparer;
    private GroupingComparerComponent _groupingComparer;

    [Inject]
    public void Construct(SignalBus signalBus, UserInterface userInterface, EqualityComparerComponent equalityComparer, GroupingComparerComponent groupingComparer)
    {
        this._signalBus = signalBus;
        this._userInterface = userInterface;
        this._equalityComparer = equalityComparer;
        this._groupingComparer = groupingComparer;
    }

    private void Start()
    {
        for (var i = 0; i < 5; i++)
        {
            mainList.Add(CreateSelectable(WIZZARD, i));
            mainList.Add(CreateSelectable(WARRIOR, i + 5));
            mainList.Add(CreateSelectable(ARCHER, i + 10));
        }

        parties = new Dictionary<object, ISelectable[]>();
    }

    private void Update()
    {
        AddRandomSelection();
        ChangeSubGroup();
        AddRemoveSelectParty();

    }

    private void AddRemoveSelectParty()
    {
        int groupKeyPressed = GameUtils.GetAnyPartyKeyPressed();
        if (groupKeyPressed > 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                if (_userInterface.Selection.Length > 0)
                    parties[groupKeyPressed] = _userInterface.Selection;
                else
                    parties.Remove(groupKeyPressed);
                _signalBus.Fire(new PartyUpdateSignal() { Parties = this.parties });
            }
            else
            {
                ISelectable[] selectables;
                if (!parties.TryGetValue(groupKeyPressed, out selectables))
                {
                    selectables = new ISelectable[0];
                }
                _signalBus.Fire(new SelectionUpdateSignal() { Selection = selectables });
            }
        }
    }

    private void ChangeSubGroup()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _signalBus.Fire(new AlternateSubGroupSignal() { Previous = Input.GetKey(KeyCode.LeftShift) });
        }
    }

    private void AddRandomSelection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _signalBus.Fire(new SelectionUpdateSignal() { Selection = GetRandomSelection() });
        }
    }

    public void UpdateAll(ChangeSelectionSignal obj)
    {
        ISelectable[] selection = GameUtils.GetOrderedSelection(obj.Selection, _equalityComparer, _groupingComparer);
        _signalBus.Fire(new SelectionUpdateSignal() { Selection = selection });
    }

    private GameSelectable CreateSelectable(string type, int index)
    {
        var selectable = new GameSelectable();
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
