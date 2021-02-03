using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectionManager<T, ST> : IDisposable where T : ISelectable
    {
        void SetMainList(HashSet<T> list);
        void SetSettings(ISelectionSettings settings);
        void SetScreenBoundries(Vector2 minScreenPoint, Vector2 maxScreenPoint);
        void SetKeysPressed(bool additiveKeyPressed, bool sameTypeKeyPressed);
        void SetGroupNumperPressed(int keyPressed);
        void SetDoubleClick(bool doubleClick);

        bool IsSelecting();
        bool IsAdditive();
        bool IsSameType();

        ISelectionSettings GetSettings();
        int GetGroupSetNumberPressed();
        Vector2 GetInitialScreenPosition();
        Vector2 GetFinalScreenPosition();
        HashSet<T> GetCurrentSelection();
        HashSet<T> GetGroupSet(int groupSetId);
        Dictionary<int, HashSet<ISelectableObject>> GetAllGroupSets();
        void CreateGroupSet(int keyPressed);
        void DoSelection(Vector3 mousePosition);
        void StartOfSelection(Vector3 mousePosition);
        void DoPreSelection(Vector3 mousePosition);
    }
}
