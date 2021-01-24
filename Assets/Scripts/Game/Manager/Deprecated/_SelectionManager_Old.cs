using UnityEngine;

namespace RTSEngine.Manager.Old
{
    public class SelectionManager_Old : MonoBehaviour
    {

        // [SerializeField] private Camera mainCamera;
        // [SerializeField] private IAbstractSelectionSettingsSO<ISelectable, SelectableTypeEnum> selectionSettings;
        // [SerializeField] private RectTransform selectionBox;
        // [SerializeField] private Transform mods;

        // private List<ISelectable> selection = new List<ISelectable>();
        // private List<ISelectable> preSelection = new List<ISelectable>();
        // private Dictionary<int, List<ISelectable>> groups = new Dictionary<int, List<ISelectable>>();
        // private bool isSelecting;
        // private Vector3 initialClickPosition;
        // private Vector3 finalClickPosition;

        // public static SelectionManager_Old Instance { get; private set; }
        // public bool IsAditiveSelection { get; set; }
        // public bool IsSameTypeSelection { get; set; }
        // public bool IsDoubleClick { get; set; }
        // public bool IsSelecting
        // {
        //     get { return isSelecting; }
        //     set
        //     {
        //         selectionBox.gameObject.SetActive(value);
        //         isSelecting = value;
        //     }
        // }

        // public ISelectable ObjectClicked { get; private set; }
        // public IAbstractSelectionSettingsSO<ISelectable, SelectableTypeEnum> SelectionSettings { get => selectionSettings; }


        // void Awake()
        // {
        //     if (Instance == null)
        //     {
        //         Instance = this;
        //         DontDestroyOnLoad(this);
        //     }
        //     else
        //     {
        //         Destroy(gameObject);
        //     }
        // }



        // void Update()
        // {
        //     if (IsSelecting)
        //     {
        //         DrawSelectionBox();
        //     }
        // }

        // public void StartOfSelection(Vector3 initialPos)
        // {
        //     initialClickPosition = initialPos;
        //     Instance.IsSelecting = true;
        // }

        // public void EndOfSelection(Vector3 finalPos)
        // {
        //     finalClickPosition = finalPos;
        //     DoSelection();
        //     FinalizeSelection();
        // }

        // private void FinalizeSelection()
        // {
        //     HideSelectionBox();
        //     preSelection.ForEach(a => a.IsPreSelected = false);
        //     preSelection = new List<ISelectable>();
        //     Instance.IsSelecting = false;
        //     Instance.IsDoubleClick = false;
        // }


        // public void DoSelection()
        // {
        //     ISelectable clicked = GetSelectableObjectClicked();
        //     List<ISelectable> newSelection = GetPrimarySelection(clicked);
        //     newSelection = PerformSelection(newSelection, clicked);
        // }

        // private List<ISelectable> PerformSelection(List<ISelectable> newSelection, ISelectable clicked)
        // {
        //     newSelection = ApplyModsToSelection(selection, newSelection, clicked);
        //     SwitchSelectionStatusFromOldToNewList(selection, newSelection);
        //     selection = newSelection;
        //     return newSelection;
        // }

        // public void DoPreSelection(Vector3 finalPos)
        // {
        //     finalClickPosition = finalPos;
        //     List<ISelectable> newSelection = GetPrimaryPreSelection();
        //     newSelection = ApplyModsToPreSelection(preSelection, newSelection);
        //     SwitchPreSelectionStatusFromOldToNewList(preSelection, newSelection);
        //     preSelection = newSelection;
        // }

        // private List<ISelectable> GetPrimarySelection(ISelectable clicked)
        // {
        //     List<ISelectable> newSelection = new List<ISelectable>();
        //     if (clicked)
        //     {
        //         newSelection.Add(clicked);
        //     }
        //     else
        //     {
        //         newSelection = SelectionUtil.GetAllObjectsInsideSelectionArea<ISelectable>(SelectableObjectMainList.Instance.List, initialClickPosition, finalClickPosition, mainCamera);
        //     }
        //     return newSelection;
        // }
        // private List<ISelectable> GetPrimaryPreSelection()
        // {
        //     return SelectionUtil.GetAllObjectsInsideSelectionArea<ISelectable>(SelectableObjectMainList.Instance.List, initialClickPosition, finalClickPosition, mainCamera);
        // }

        // private List<ISelectable> ApplyModsToSelection(List<ISelectable> oldSelection, List<ISelectable> newSelection, ISelectable clicked)
        // {
        //     SelectionArgs args = GetSelectionArgs(oldSelection, newSelection);
        //     args.Clicked = clicked;
        //     args.IsPreSelection = false;
        //     return ApplyMods(args);
        // }

        // private List<ISelectable> ApplyModsToPreSelection(List<ISelectable> oldSelection, List<ISelectable> newSelection)
        // {
        //     SelectionArgs args = GetSelectionArgs(oldSelection, newSelection);
        //     args.Clicked = null;
        //     args.IsPreSelection = true;
        //     return ApplyMods(args);
        // }

        // private List<ISelectable> ApplyMods(SelectionArgs args)
        // {
        //     if (args.NewList.Count != 0 || args.IsAditive || args.IsSameType)
        //     {
        //         if (args.IsAditive)
        //         {
        //             args.NewList = args.OldList.Union(args.NewList).ToList();
        //         }
        //         foreach (var mod in mods.GetComponents<IAbstractSelectionMod<ISelectable, SelectableTypeEnum>>())
        //         {
        //             // args.NewList = mod.ApplyMod(args);
        //         };
        //     }
        //     return args.NewList;
        // }

        // private void SwitchSelectionStatusFromOldToNewList(List<ISelectable> oldSelection, List<ISelectable> newSelection)
        // {
        //     oldSelection.ForEach(a =>
        //     {
        //         a.IsSelected = newSelection.Contains(a);
        //     });
        //     newSelection.ForEach(a =>
        //     {
        //         a.IsSelected = true;
        //     });
        // }

        // private void SwitchPreSelectionStatusFromOldToNewList(List<ISelectable> oldSelection, List<ISelectable> newSelection)
        // {
        //     oldSelection.ForEach(a => a.IsPreSelected = newSelection.Contains(a));
        //     newSelection.ForEach(a => a.IsPreSelected = true);
        // }

        // private ISelectable GetSelectableObjectClicked()
        // {
        //     return SelectionUtil.GetObjectClicked<ISelectable>(initialClickPosition, finalClickPosition, mainCamera);
        // }

        // private void DrawSelectionBox()
        // {
        //     selectionBox.position = SelectionUtil.GetAreaCenter(initialClickPosition, finalClickPosition);
        //     selectionBox.sizeDelta = SelectionUtil.GetAreaSize(initialClickPosition, finalClickPosition);
        // }

        // private void HideSelectionBox()
        // {
        //     selectionBox.position = Vector3.zero;
        //     selectionBox.sizeDelta = Vector2.zero;
        // }

        // private SelectionArgs GetSelectionArgs(List<ISelectable> oldSelection, List<ISelectable> newSelection)
        // {
        //     SelectionArgs args = new SelectionArgs();
        //     args.MainList =SelectableObjectMainList.Instance.List;
        //     args.NewList = newSelection;
        //     args.IsAditive = IsAditiveSelection;
        //     args.PreSelectionList = preSelection;
        //     if (IsAditiveSelection)
        //     {
        //         args.OldList = oldSelection;
        //     }
        //     else
        //     {
        //         args.OldList = new List<ISelectable>();
        //     }
        //     args.IsDoubleClick = IsDoubleClick;
        //     args.IsSameType = IsSameTypeSelection;
        //     args.Camera = mainCamera;
        //     return args;
        // }

        // public Vector3 GetSelectionMainPoint()
        // {
        //     if (selection.Count == 0)
        //     {
        //         return Vector3.zero;
        //     }
        //     return selection[0].transform.position;
        // }

        // public void SetGroup(int keyPressed)
        // {
        //     groups[keyPressed] = selection;
        // }

        // public void GetGroup(int keyPressed)
        // {
        //     List<ISelectable> list;
        //     groups.TryGetValue(keyPressed, out list);
        //     if (list == null)
        //     {
        //         list = new List<ISelectable>();
        //     }
        //     PerformSelection(list, null);
        // }

        // public List<ISelectable> GetSelection()
        // {
        //     return selection;
        // }
    }


}