using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Utils;
using RTSEngine.Core;
using RTSEngine.Signal;
using UnityEngine.EventSystems;
using Zenject;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneHelper : DefaultUserInterfaceInput
    {
        // private List<ISelectable> mainList = new List<ISelectable>();
        // const string WIZZARD = "Wizzard";
        // const string WARRIOR = "Warrior";
        // const string ARCHER = "Archer";

        // private UserInterface _userInterface;
        // private UserInterfaceBase _userInterfaceBase;
        // private SignalBus _signalBus;
        // private IEqualityComparer<ISelectable> _equalityComparer;
        // private IComparer<IGrouping<ISelectable, ISelectable>> _groupSortComparer;
        // private IUserInterfaceLogManager _logManager;
        // private DefaultUserInterfaceInfoManager _infoManager;

        // [Inject]
        // public void Construct(SignalBus signalBus, UserInterface userInterface, UserInterfaceBase userInterfaceBase, IEqualityComparer<ISelectable> equalityComparer, IComparer<IGrouping<ISelectable, ISelectable>> groupSortComparer, IUserInterfaceLogManager logManager)
        // {
        //     this._signalBus = signalBus;
        //     this._userInterface = userInterface;
        //     this._equalityComparer = equalityComparer;
        //     this._groupSortComparer = groupSortComparer;
        //     this._userInterfaceBase = userInterfaceBase;
        //     this._logManager = logManager;
        // }

        // private void Start()
        // {
        //     for (var i = 0; i < 5; i++)
        //     {
        //         mainList.Add(CreateSelectable(WIZZARD, i));
        //         mainList.Add(CreateSelectable(WARRIOR, i + 5));
        //         mainList.Add(CreateSelectable(ARCHER, i + 10));
        //     }
        //     var infoPanel = _userInterfaceBase.UserInterfaceBaseComponent.InfoPanel;
        //     if (infoPanel)
        //     {
        //         _infoManager = infoPanel.GetComponent<DefaultUserInterfaceInfoManager>();
        //     }
        //     StartCoroutine(StartInfo());
        // }

        // private IEnumerator StartInfo()
        // {
        //     yield return new WaitForSeconds(.1f);
        //     foreach (var info in _sceneInfoList)
        //     {
        //         ExecuteEvents.Execute<IInfoMessageTarget>(_infoManager.gameObject, null, (x, y) => x.AddInfo(CreateInfoButton(info)));
        //         _logManager.AddLog("New hint!");
        //         yield return new WaitForSeconds(.1f);
        //     }
        // }

        // public override void GetOtherInputs()
        // {
        //     AddRandomSelection();
        //     GetHighlightedSelection();
        //     ClearSelection();
        //     AddInfoInput();
        //     AddLogInput();
        // }

        // private void AddInfoInput()
        // {
        //     if (Input.GetKeyDown(KeyCode.I))
        //     {
        //         AddInfo();
        //     }
        // }

        // private void AddLogInput()
        // {
        //     if (Input.GetKeyDown(KeyCode.L))
        //     {
        //         _logManager.AddLog(fakeLogs[Random.Range(0, fakeLogs.Length)]);
        //     }
        // }

        // private string[] fakeLogs = new string[]{
        //     "More <b><color=orange>Gold</color></b> is required",
        //     "More <b><color=brown>Lumber</color></b> is required",
        //     "More <b><color=red>Food</color></b> is required",
        //     "More <b><color=gray>Minerals</color></b> is required",
        //     "<b><color=lightblue>Metal sword</color></b> research complete",
        //     "<b><color=yellow>Farm</color></b> build complete",
        //     "<b><color=yellow>Lumber Mill</color></b> build complete",
        // };

        // private void AddRandomSelection()
        // {
        //     if (Input.GetKeyDown(KeyCode.Q))
        //     {
        //         _signalBus.Fire(new SelectionUpdateSignal() { Selection = GetRandomSelection() });
        //     }
        // }

        // private void GetHighlightedSelection()
        // {
        //     if (Input.GetKeyDown(KeyCode.W))
        //     {
        //         _signalBus.Fire(new SelectionUpdateSignal() { Selection = new ISelectable[] { _userInterface.Highlighted } });
        //     }
        // }

        // private void ClearSelection()
        // {
        //     if (Input.GetKeyDown(KeyCode.R))
        //     {
        //         _signalBus.Fire(new SelectionUpdateSignal() { Selection = new ISelectable[] { } });
        //     }
        // }

        // private ISelectable[] GetRandomSelection()
        // {
        //     var result = new List<ISelectable>(0);
        //     var length = Random.Range(0, 10);
        //     for (var i = 0; i < length; i++)
        //     {
        //         var a = mainList[Random.Range(0, mainList.Count)];
        //         if (!result.Contains(a))
        //             result.Add(a);
        //     }
        //     result.Sort(new Comparer());
        //     return result.ToArray();
        // }

        // private void AddInfo()
        // {
        //     DefaultInfoButton button = GetInfoButton();
        //     if (button)
        //     {
        //         ExecuteEvents.Execute<IInfoMessageTarget>(_infoManager.gameObject, null, (x, y) => x.AddInfo(button));
        //     }
        // }

        // private DefaultInfoButton GetInfoButton()
        // {
        //     DefaultInfoButton button = null;
        //     string[] info = GetNextInfo();
        //     if (info != null)
        //     {
        //         button = CreateInfoButton(info);
        //     }
        //     return button;
        // }

        // private UserInterfaceSceneObject CreateSelectable(string type, int index)
        // {
        //     var selectable = new UserInterfaceSceneObject();
        //     selectable.Type = type;
        //     selectable.Index = index;
        //     selectable.Position = new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
        //     return selectable;
        // }

        // private DefaultInfoButton CreateInfoButton(string[] info)
        // {
        //     DefaultInfoButton button = _userInterfaceBase.InfoFactory.Create();
        //     GameUtils.FindInComponent<Text>(button.Title.gameObject).text = info[0];
        //     GameUtils.FindInComponent<Text>(button.Text.gameObject).text = info[1];
        //     GameUtils.FindInComponent<Text>(button.SubText.gameObject).text = info[2];
        //     GameUtils.FindInComponent<Text>(button.ToolTip.gameObject).text = info[3];
        //     return button;
        // }

        // private string[] GetNextInfo()
        // {
        //     foreach (var item in _sceneInfoList)
        //     {
        //         if (!_infoManager.PanelContainsInfo(item[1]))
        //         {
        //             return item;
        //         }
        //     }
        //     return null;
        // }

        // private List<string[]> _sceneInfoList = new List<string[]>{
        //     new string[]{"I","Show next hint", "click to dismiss", "I \n Show next hint"},
        //     new string[]{"Q","Create a random selection", "click to dismiss", "Q \n Create a random selection"},
        //     new string[]{"W","Get the highlighted in selection", "click to dismiss", "W \n Get the highlighted in selection"},
        //     new string[]{"R","Clear selection", "click to dismiss", "R \n Clear selection"},
        //     new string[]{"Space","Center to highlighted", "click to dismiss", "Space \n Center to highlighted"},
        //     new string[]{"Tab","Change sub group", "click to dismiss", "Tab \n Change sub group"},
        //     new string[]{"Z + [number]","Create/Remove party at [number] with selection", "click to dismiss", "Z + [number] \n Create/Remove party at [number] with selection"},
        //     new string[]{"Click [Miniature/banner]","Do miniature/banner click action", "click to dismiss", "Click [Miniature/banner] \n Do miniature/banner click action"},
        //     new string[]{"Shift + Click [Miniature/banner]","Do miniature/banner shift+click action", "click to dismiss", "Shift + Click [Miniature/banner] \n Do miniature/banner shift+click action"},
        //     new string[]{"Ctrl+Click[Miniature/banner]","Do miniature/banner ctrl+click action", "click to dismiss", "Ctrl+Click[Miniature/banner] \n Do miniature/banner ctrl+click action"},
        //     new string[]{"HoldClick [Portrait]","Center to highlighted1", "click to dismiss", "HoldClick [Portrait] \n Center to highlighted1"},
        // };

        // private class Comparer : IComparer<ISelectable>
        // {
        //     public int Compare(ISelectable x, ISelectable y)
        //     {
        //         return x.Index - y.Index;
        //     }
        // }
    }
}