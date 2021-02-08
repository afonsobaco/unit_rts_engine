using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public interface ISelectionManager
    {
        void AddSelectableObject(SelectableObjectCreatedSignal signal);
        SelectionArguments ApplyModifiers(SelectionArguments args);
        void Construct(SignalBus signalBus);
        void CreateGroupSet(int number);
        void Dispose();
        void DoSelectedMiniatureClick(SelectedMiniatureClickSignal signal);
        void DoPreSelection(Vector2 finalPos);
        void DoSelection(Vector2 finalPos);
        Dictionary<int, HashSet<ISelectableObject>> GetAllGroupSets();
        HashSet<ISelectableObject> GetCurrentSelection();
        HashSet<ISelectableObject> GetDragSelection();
        Vector2 GetFinalScreenPosition();
        HashSet<ISelectableObject> GetFinalSelection(SelectionArguments args);
        HashSet<ISelectableObject> GetGroupSet(int number);
        int GetGroupSetNumberPressed();
        Vector2 GetInitialScreenPosition();
        List<ISelectionModifier> GetModifiersToBeApplied(SelectionTypeEnum type);
        ISelectableObject GetObjectClicked();
        IEnumerable<ISelectableObject> GetPreSelection();
        HashSet<ISelectableObject> GetSelection(HashSet<ISelectableObject> newSelection);
        SelectionArguments GetSelectionArgs(HashSet<ISelectableObject> newSelection);
        HashSet<ISelectableObject> GetSelectionBySelectionType();
        SelectionTypeEnum GetSelectionType();
        ISelectionSettings GetSettings();
        HashSet<ISelectableObject> GetUpdatedCurrentSelection(HashSet<ISelectableObject> newSelection);
        HashSet<ISelectableObject> GetUpdatedPreSelection(HashSet<ISelectableObject> newSelection);
        bool IsAdditive();
        bool IsSameType();
        bool IsSelecting();
        HashSet<ISelectableObject> OrderSelection(HashSet<ISelectableObject> newSelection);
        HashSet<ISelectableObject> PerformSelection(Vector2 finalPos);
        void RemoveSelectableObject(SelectableObjectDeletedSignal signal);
        void SetClicked(ISelectableObject selected);
        void SetCurrentSelection(HashSet<ISelectableObject> selection);
        void SetDoubleClick(bool doubleClick);
        void SetGroupNumperPressed(int groupNumber);
        void SetIsPreSelection(bool value);
        void SetKeysPressed(bool additiveKeyPressed, bool sameTypeKeyPressed);
        void SetLastClicked(ISelectableObject selected);
        void SetMainList(HashSet<ISelectableObject> list);
        void SetPreSelection(HashSet<ISelectableObject> preSelection);
        void SetSelectionModifiers(List<ISelectionModifier> list);
        void SetSelectionType(SelectionTypeEnum type);
        void SetSettings(ISelectionSettings value);
        void StartOfSelection(Vector2 initialPos);
    }

}