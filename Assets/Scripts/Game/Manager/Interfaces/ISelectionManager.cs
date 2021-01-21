using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager.Enums;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Interfaces;
using RTSEngine.Manager.Impls;

namespace RTSEngine.Manager.Interfaces
{
    public interface ISelectionManager<T, S, O> where T : SelectableObject
    {
        Vector3 InitialScreenPosition { get; set; }
        Vector3 FinalScreenPosition { get; set; }
        int KeyPressed { get; set; }
        T Cliked { get; set; }
        List<T> CurrentSelection { get; set; }
        IRuntimeSet<T> SelectableList { get; set; }
        ISelectionSettings<T, S, O> Settings { get; set; }
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
