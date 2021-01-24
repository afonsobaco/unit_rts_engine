using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectionManager<T, ST> : IDisposable where T : SelectableObject
    {
        IRuntimeSet<T> SelectableList { get; set; }
        List<ScriptableObject> ScriptableObjectMods { get; set; }
        List<T> CurrentSelection { get; set; }
        T Cliked { get; set; }
        Vector3 FinalScreenPosition { get; set; }
        Vector3 InitialScreenPosition { get; set; }
        bool IsAditiveSelection { get; set; }
        bool IsDoubleClick { get; set; }
        bool IsSameTypeSelection { get; set; }
        bool IsSelecting { get; set; }
        int KeyPressed { get; set; }

        List<T> GetNewSelection();
        List<T> UpdateCurrentSelection(List<T> value);
        List<T> UpdatePreSelection(List<T> value);
        SelectionTypeEnum GetSelectionType();
        Vector3 GetSelectionMainPoint();
        void AddSelectableObject(SelectableObjectCreatedSignal signal);
        void DoPreSelection(Vector3 mousePosition);
        void EndOfSelection(Vector3 mousePosition);
        void RemoveSelectableObject(SelectableObjectDeletedSignal signal);
        void SetGroup(int key);
        void StartOfSelection(Vector3 mousePosition);
    }
}
