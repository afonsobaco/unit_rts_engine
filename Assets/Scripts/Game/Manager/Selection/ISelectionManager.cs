using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Selection;

namespace RTSEngine.Manager
{
    public interface ISelectionManager<T> where T : SelectableObject 
    {
        Vector3 InitialScreenPosition { get; set; }
        Vector3 FinalScreenPosition { get; set; }
        int KeyPressed { get; set; }
        T Cliked { get; set; }
        List<T> CurrentSelection { get; set; }
        IRuntimeSet<T> SelectableList { get; set; }
        bool IsAditiveSelection { get; set; }
        bool IsSameTypeSelection { get; set; }
        bool IsDoubleClick { get; set; }

        List<T> GetGroup(int key);
        List<T> GetNewSelection();
        SelectionTypeEnum GetSelectionType();
        void SetGroup(int key);
        List<T> UpdateCurrentSelection(List<T> value);
        List<T> UpdatePreSelection(List<T> value);
        Vector3 GetSelectionMainPoint();
        void StartOfSelection(Vector3 mousePosition);
        void DoPreSelection(Vector3 mousePosition);
        void EndOfSelection(Vector3 mousePosition);
    }
}
