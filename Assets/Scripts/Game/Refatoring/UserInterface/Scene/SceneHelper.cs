using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Refactoring;
using RTSEngine.Utils;
using Zenject;

public class SceneHelper : MonoBehaviour
{
    public RectTransform portrait;
    public GridLayoutGroup grid;
    public GridLayoutGroup banner;
    public BannerButton bannerPrefab;
    public MiniatureButton miniaturePrefab;
    public PortraitButton portraitPrefab;

    private List<ISelectable> mainList = new List<ISelectable>();
    const string WIZZARD = "Wizzard";
    const string WARRIOR = "Warrior";
    const string ARCHER = "Archer";

    private UserInterface _userInterface;
    private SignalBus _signalBus;
    private Dictionary<object, ISelectable[]> groups;

    [Inject]
    public void Construct(SignalBus signalBus, UserInterface userInterface)
    {
        this._signalBus = signalBus;
        this._userInterface = userInterface;
    }

    private void Start()
    {
        for (var i = 0; i < 5; i++)
        {
            mainList.Add(CreateMiniature(WIZZARD, i));
            mainList.Add(CreateMiniature(WARRIOR, i + 5));
            mainList.Add(CreateMiniature(ARCHER, i + 10));
        }

        groups = new Dictionary<object, ISelectable[]>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _signalBus.Fire(new SelectionUpdateSignal() { Selection = GetRandomSelection() });
            UpdateAll();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _signalBus.Fire(new AlternateSubGroupSignal() { Previous = Input.GetKey(KeyCode.LeftShift) });
            UpdateAll();
        }

        int groupKeyPressed = GameUtils.GetAnyGroupKeyPressed();
        if (groupKeyPressed > 0 && Input.GetKey(KeyCode.Z))
        {
            if (_userInterface.Selection.Length > 0)
                groups[groupKeyPressed] = _userInterface.Selection;
            else
                groups.Remove(groupKeyPressed);
            _signalBus.Fire(new GroupUpdateSignal() { Groups = this.groups });
            UpdateAll();
        }

    }

    public void UpdateAll()
    {
        UpdateGrid();
        UpdatePortrait();
        UpdateBanners();
    }

    private void UpdatePortrait()
    {
        RemoveChildren(portrait.transform);
        if (_userInterface.Highlighted != null)
        {
            var instance = GameObject.Instantiate(portraitPrefab);
            instance.transform.SetParent(portrait.transform, false);
            var miniaure = _userInterface.Highlighted as MiniatureClass;
            var p = CreatePortrait(miniaure.Type);
            instance.Selectable = p;
        }
    }

    private void UpdateGrid()
    {
        RemoveChildren(grid.transform);
        foreach (ISelectable item in _userInterface.Selection)
        {
            var instance = GameObject.Instantiate(miniaturePrefab);
            instance.transform.SetParent(grid.transform, false);
            instance.Selectable = item as MiniatureClass;
        }
    }

    private void UpdateBanners()
    {
        RemoveChildren(banner.transform);
        foreach (var item in _userInterface.Groups)
        {
            var instance = GameObject.Instantiate(bannerPrefab);
            instance.transform.SetParent(banner.transform, false);
            instance.GroupId = item.Key;
        }
    }

    private void RemoveChildren(Transform transf)
    {
        foreach (Transform item in transf)
        {
            GameObject.Destroy(item.gameObject);
        }
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

    private MiniatureClass CreateMiniature(string type, int index)
    {
        var miniature = new MiniatureClass();
        miniature.Type = type;
        miniature.Index = index;
        miniature.Position = new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
        return miniature;
    }

    private PortraitClass CreatePortrait(string name)
    {
        var portrait = new PortraitClass();
        portrait.Name = name;
        portrait.Position = new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
        return portrait;
    }

    private class Comparer : IComparer<ISelectable>
    {
        public int Compare(ISelectable x, ISelectable y)
        {
            return x.Index - y.Index;
        }
    }
}
